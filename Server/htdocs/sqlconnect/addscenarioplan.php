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
if (!isset($_POST['userId']) || !isset($_POST['scenarioId']) || !isset($_POST['categoryId']) || !isset($_POST['orderNumber'])) {
    echo json_encode(["status" => "error", "message" => "Ungültige Eingabedaten"]);
    exit();
}

$userId = (int)$_POST['userId'];
$scenarioId = !empty($_POST['scenarioId']) ? (int)$_POST['scenarioId'] : null;
$categoryId = !empty($_POST['categoryId']) ? (int)$_POST['categoryId'] : null;
$orderNumber = (int)$_POST['orderNumber'];

$query = "INSERT INTO scenarioplan (userId, nextScenarioId, nextCategoryId, questionOrder) VALUES (?, ?, ?, ?)";
$stmt = mysqli_prepare($con, $query);

if ($stmt === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der SQL-Abfrage: " . mysqli_error($con)]);
    exit();
}

mysqli_stmt_bind_param($stmt, "iiii", $userId, $scenarioId, $categoryId, $orderNumber);
if (mysqli_stmt_execute($stmt)) {
    echo json_encode(["status" => "success", "message" => "Szenarioplan wurde erfolgreich hinzugefügt"]);
} else {
    echo json_encode(["status" => "error", "message" => "Fehler beim Hinzufügen des Szenarioplans: " . mysqli_error($con)]);
}

// Schließen der Verbindung
mysqli_stmt_close($stmt);
mysqli_close($con);

?>