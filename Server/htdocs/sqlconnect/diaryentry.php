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

// Eingabewerte validieren
if (!isset($_POST['userId'], $_POST['feeling'], $_POST['entry'], $_POST['date']) || 
    !is_numeric($_POST['userId']) || 
    !is_numeric($_POST['feeling']) ||
    !strtotime($_POST['date'])) {
    echo json_encode(["status" => "error", "message" => "Ungültige Eingabedaten."]);
    exit();
}

$id = (int)$_POST['userId'];
$feeling = (int)$_POST['feeling'];
$entry = $_POST['entry'];
$entrydate = $_POST['date'];

$entryquery = "INSERT INTO diary (userid, entrydate, feeling, entry) VALUES (?, STR_TO_DATE(?, '%d-%m-%Y %H:%i:%s'), ?, ?)";

$stmt = mysqli_prepare($con, $entryquery);
if ($stmt === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der SQL-Abfrage."]);
    exit();
}

mysqli_stmt_bind_param($stmt, "isss", $id, $entrydate, $feeling, $entry);

if (mysqli_stmt_execute($stmt)) {
    echo json_encode(["status" => "success", "message" => "Eintrag erfolgreich angelegt!"]);
} else {
    echo json_encode(["status" => "error", "message" => "Fehler beim Anlegen des Eintrags."]);
}

mysqli_stmt_close($stmt);
mysqli_close($con);

?>