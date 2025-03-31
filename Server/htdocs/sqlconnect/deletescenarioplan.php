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
if (!isset($_POST['id']) || !is_numeric($_POST['id'])) {
    echo json_encode(["status" => "error", "message" => "Ungültige ID"]);
    exit();
}

$id = (int)$_POST['id'];

$query = "DELETE FROM scenarioplan WHERE id = $id";

// Ausführen der Abfrage
$result = mysqli_query($con, $query);

if ($result && mysqli_affected_rows($con) > 0) {
    echo json_encode([
        "status" => "success",
        "message" => "Szenarioplan erfolgreich gelöscht."
    ]);
} else {
    echo json_encode([
        "status" => "error",
        "message" => "Fehler beim Löschen des Szenarioplans oder der Eintrag wurde nicht gefunden."
    ]);
}

mysqli_close($con);

?>