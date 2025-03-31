<?php

header("Access-Control-Allow-Origin: *"); // Erlaubt Anfragen von überall (testen)
header("Access-Control-Allow-Methods: POST, GET, OPTIONS"); // Erlaubt diese Methoden
header("Access-Control-Allow-Headers: Content-Type"); // Erlaubt bestimmte Header

// Verbindung zur Datenbank
$user = 'root';
$password = 'root';
$db = 'sqlconnect';
$host = 'localhost';
$port = 3306;

$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

if (!$success) {
    echo json_encode(["status" => "error", "message" => "Verbindung zur Datenbank fehlgeschlagen!"]);
    exit();
}

// Eingabedaten validieren
if (!isset($_POST['userId']) || !is_numeric($_POST['userId'])) {
    echo json_encode(["status" => "error", "message" => "Ungültige Benutzer-ID"]);
    exit();
}

$userid = (int)$_POST['userId'];

// Vorbereitetes Statement für die SQL-Abfrage
$getentriesquery = "SELECT id, entrydate, feeling, entry FROM diary WHERE userid = ?";
$stmt = mysqli_prepare($con, $getentriesquery);

if ($stmt === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der SQL-Abfrage."]);
    exit();
}

mysqli_stmt_bind_param($stmt, "i", $userid);
mysqli_stmt_execute($stmt);

$result = mysqli_stmt_get_result($stmt);

// Überprüfen, ob Ergebnisse vorhanden sind
if (mysqli_num_rows($result) > 0) {
    $entries = [];

    // Daten aus der Datenbank abholen
    while ($row = mysqli_fetch_assoc($result)) {
        $entries[] = [
            "id" => $row['id'],
            "entrydate" => $row['entrydate'],
            "feeling" => (int)$row['feeling'],
            "entry" => $row['entry']
        ];
    }

    // Erfolgreiche Antwort zurückgeben
    echo json_encode([
        "status" => "success",
        "entries" => $entries
    ]);
} else {
    // Keine Einträge gefunden
    echo json_encode([
        "status" => "success",
        "entries" => [] // Leere Liste zurückgeben
    ]);
}

// Schließen der Verbindung und des Statements
mysqli_stmt_close($stmt);
mysqli_close($con);

?>