<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

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

if (!isset($_POST['userId']) || !filter_var($_POST['userId'], FILTER_VALIDATE_INT)) {
    echo json_encode(["status" => "error", "message" => "Ungültige Benutzer-ID"]);
    exit();
}

$userId = (int)$_POST['userId'];

// SQL-Abfrage zum Abrufen der Szenarien-Daten
$query = "
    SELECT 
        ps.id,
        s.name,
        s.question,
        ec.name AS category_name,
        sa.answer,
        ac.name AS answercategory_name,
        ps.date
    FROM playedscenarios ps
    JOIN scenarios s ON ps.scenarioId = s.id
    JOIN ercategories ec ON s.categoryId = ec.id
    JOIN scenarioanswers sa ON ps.answerId = sa.id
    JOIN answercategories ac ON sa.answercategory = ac.id
    WHERE ps.userId = ?
    ORDER BY ps.date DESC
";

$stmt = mysqli_prepare($con, $query);
mysqli_stmt_bind_param($stmt, "i", $userId);

mysqli_stmt_execute($stmt);

$result = mysqli_stmt_get_result($stmt);

if (mysqli_num_rows($result) > 0) {
    $scenarios = [];
    while ($row = mysqli_fetch_assoc($result)) {
        $scenarios[] = [
            "id" => (int)$row['id'],
            "name" => $row['name'] ?: '',
            "question" => $row['question'] ?: '',
            "category_name" => $row['category_name'] ?: '',
            "answer" => $row['answer'] ?: '',
            "answercategory_name" => $row['answercategory_name'] ?: '',
            "date" => $row['date'] ?: ''
        ];
    }

    echo json_encode([
        "status" => "success",
        "scenariosPlayed" => $scenarios
    ]);
} else {
    // Keine Szenarien gefunden
    echo json_encode([
        "status" => "success",
        "scenariosPlayed" => []      // Leere Liste für Szenarien
    ]);
}

// Schließen der Verbindung
mysqli_stmt_close($stmt);
mysqli_close($con);

?>