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
    echo json_encode(["status" => "error", "message" => "Ungültige Benutzer-ID"]);
    exit();
}

$userId = (int)$_POST['id']; // Sicherstellen, dass id eine Ganzzahl ist

// SQL-Abfrage
$query = "SELECT * FROM scenarioplan WHERE userId = $userId";
$result = mysqli_query($con, $query);

// Überprüfen, ob Daten vorhanden sind
if (mysqli_num_rows($result) > 0) {
    $scenarios = [];
    while ($row = mysqli_fetch_assoc($result)) {
        $scenarios[] = [
            "id" => (int)$row['id'],
            "userId" => (int)$row['userId'],
            "nextScenarioId" => $row['nextScenarioId'] ? (int)$row['nextScenarioId'] : null,
            "nextCategoryId" => $row['nextCategoryId'] ? (int)$row['nextCategoryId'] : null,
            "questionOrder" => (int)$row['questionOrder']
        ];
    }

    // Erfolgreiche Antwort
    echo json_encode([
        "status" => "success", 
        "scenarioplan" => $scenarios
    ]);
} else {
    // Keine Szenarien gefunden
    echo json_encode([
        "status" => "success", 
        "scenarioplan" => []
    ]);
}

// Verbindung schließen
mysqli_close($con);

?>