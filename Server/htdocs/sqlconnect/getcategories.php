<?php

header("Access-Control-Allow-Origin: *"); // Erlaubt Anfragen von überall
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

// Vorbereitete Statements für die SQL-Abfragen
$getErCategoriesQuery = "SELECT id, name FROM ercategories";
$getAnswerCategoriesQuery = "SELECT id, name FROM answercategories";

// Erhalte die erCategories
$erCategories = [];
try {
    $resultErCategories = mysqli_query($con, $getErCategoriesQuery);
    if (!$resultErCategories) {
        throw new Exception("Fehler bei der Abfrage der erCategories: " . mysqli_error($con));
    }
    while ($row = mysqli_fetch_assoc($resultErCategories)) {
        $erCategories[] = [
            "id" => (int)$row['id'],
            "name" => $row['name']
        ];
    }
} catch (Exception $e) {
    echo json_encode(["status" => "error", "message" => $e->getMessage()]);
    mysqli_close($con);
    exit();
}

// Erhalte die answerCategories
$answerCategories = [];
try {
    $resultAnswerCategories = mysqli_query($con, $getAnswerCategoriesQuery);
    if (!$resultAnswerCategories) {
        throw new Exception("Fehler bei der Abfrage der answerCategories: " . mysqli_error($con));
    }
    while ($row = mysqli_fetch_assoc($resultAnswerCategories)) {
        $answerCategories[] = [
            "id" => (int)$row['id'],
            "name" => $row['name']
        ];
    }
} catch (Exception $e) {
    echo json_encode(["status" => "error", "message" => $e->getMessage()]);
    mysqli_close($con);
    exit();
}

// Erfolgreiche Antwort zurückgeben
echo json_encode([
    "status" => "success",
    "categories" => [
        "erCategories" => $erCategories,
        "answerCategories" => $answerCategories
    ]
]);

// Schließen der Verbindung
mysqli_close($con);

?>