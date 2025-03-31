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

// Übermittelte Daten validieren
if (!isset($_POST['userlevel']) || !is_numeric($_POST['userlevel'])) {
    echo json_encode(["status" => "error", "message" => "Ungültiger oder fehlender Wert für userlevel."]);
    exit();
}

$userlevel = (int)$_POST['userlevel']; // sicherstellen, dass userlevel ein Integer ist

// SQL-Abfrage mit Platzhaltern zur Vermeidung von SQL-Injektion
$getlevelquery = "SELECT level, points, reward FROM userlevels WHERE level = ?";

$stmt = mysqli_prepare($con, $getlevelquery);
if ($stmt === false) {
    // Fehlerbehandlung und Logging
    error_log("Fehler bei der Vorbereitung der SQL-Abfrage: " . mysqli_error($con));
    echo json_encode(["status" => "error", "message" => "Ein Fehler ist aufgetreten. Bitte versuchen Sie es später noch einmal."]);
    exit();
}

// Binden der Parameter und Ausführen der Abfrage
mysqli_stmt_bind_param($stmt, "i", $userlevel); // 'i' für Integer
mysqli_stmt_execute($stmt);

// Ergebnis abholen
$result = mysqli_stmt_get_result($stmt);

// Überprüfen, ob ein Ergebnis vorhanden ist
if (mysqli_num_rows($result) == 1) {
    $row = mysqli_fetch_assoc($result);

    // Rückgabe des Ergebnisses als JSON
    echo json_encode([
        "status" => "success",
        "level" => (int)$row['level'],
        "points" => (int)$row['points'],
        "reward" => $row['reward']
    ]);
} else {
    // Keine Daten gefunden
    echo json_encode(["status" => "error", "message" => "Kein Level gefunden für die angegebene Level-ID."]);
}

// Schließen der Verbindung
mysqli_stmt_close($stmt);
mysqli_close($con);

?>