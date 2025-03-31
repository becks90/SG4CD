<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

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
if (!isset($_POST['userId']) || !isset($_POST['advisorId'])) {
    echo json_encode(["status" => "error", "message" => "Ungültige Eingabedaten"]);
    exit();
}

$userId = (int)$_POST['userId'];
$advisorId = (int)$_POST['advisorId'];

$query = "INSERT INTO advisedperson (userid, advisorid) VALUES (?, ?)";

// Vorbereiten der SQL-Anweisung
$stmt = mysqli_prepare($con, $query);

if ($stmt === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der SQL-Abfrage"]);
    exit();
}

// Binden der Parameter
mysqli_stmt_bind_param($stmt, "ii", $userId, $advisorId);

// Ausführen der SQL-Anweisung
if (mysqli_stmt_execute($stmt)) {
    echo json_encode(["status" => "success", "message" => "Betreuer erfolgreich zugewiesen"]);
} else {
    echo json_encode(["status" => "error", "message" => "Fehler beim Hinzufügen des Betreuers: " . mysqli_error($con)]);
}

// Schließen der Verbindung
mysqli_stmt_close($stmt);
mysqli_close($con);

?>