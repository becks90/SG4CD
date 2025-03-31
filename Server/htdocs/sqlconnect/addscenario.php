<?php

header("Access-Control-Allow-Origin: *"); // Erlaubt Anfragen von überall
header("Access-Control-Allow-Methods: POST, GET, OPTIONS"); // Erlaubt diese Methoden
header("Access-Control-Allow-Headers: Content-Type"); // Erlaubt bestimmte Header

ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);

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

$userId = $_POST['userId'];
$name = $_POST['name'];
$question = $_POST['question'];
$categoryId = $_POST['categoryId'];

$a1 = $_POST['a1'];
$a2 = $_POST['a2'];
$a3 = $_POST['a3'];
$a4 = $_POST['a4'];
$a5 = $_POST['a5'];
$a6 = $_POST['a6'];

$r1 = $_POST['r1'];
$r2 = $_POST['r2'];
$r3 = $_POST['r3'];
$r4 = $_POST['r4'];
$r5 = $_POST['r5'];
$r6 = $_POST['r6'];

$c1 = $_POST['c1'];
$c2 = $_POST['c2'];
$c3 = $_POST['c3'];
$c4 = $_POST['c4'];
$c5 = $_POST['c5'];
$c6 = $_POST['c6'];

// Beginne eine Transaktion
mysqli_begin_transaction($con);

try {
    // Füge das Szenario in die Tabelle `scenarios` ein
    $insertScenarioQuery = "INSERT INTO scenarios (name, question, categoryId, creator) VALUES (?, ?, ?, ?)";
    $stmt = mysqli_prepare($con, $insertScenarioQuery);
    mysqli_stmt_bind_param($stmt, "ssii", $name, $question, $categoryId, $userId);
    if (!mysqli_stmt_execute($stmt)) {
        throw new Exception("Fehler beim Einfügen des Szenarios: " . mysqli_error($con));
    }

    // ID des eingefügten Szenarios
    $scenarioId = mysqli_insert_id($con);

    // Kategorie-ID und Punkte
    $getAnswerCategoryQuery = "
        SELECT ac.id AS categoryId, ac.defaultPoints
        FROM answercategories ac
        WHERE ac.name = ?
    ";

    // Array der Antworten und Kategorien
    $answers = [
    [$a1, $r1, $c1],
    [$a2, $r2, $c2],
    [$a3, $r3, $c3],
    [$a4, $r4, $c4],
    [$a5, $r5, $c5],
    [$a6, $r6, $c6]
    ];

// defaultPoints aus `answercategories` und füge die Antworten in `scenarioanswers` ein
$insertAnswerQuery = "INSERT INTO scenarioanswers (scenarioId, answer, response, answercategory, points) VALUES (?, ?, ?, ?, ?)";

$stmtInsert = mysqli_prepare($con, $insertAnswerQuery);

foreach ($answers as $answer) {
    $categoryId = $answer[2];
    
    $getCategoryPointsQuery = "SELECT defaultPoints FROM answercategories WHERE id = ?";
    $categoryStmt = mysqli_prepare($con, $getCategoryPointsQuery);
    mysqli_stmt_bind_param($categoryStmt, "i", $categoryId);
    mysqli_stmt_execute($categoryStmt);
    $categoryResult = mysqli_stmt_get_result($categoryStmt);
    $category = mysqli_fetch_assoc($categoryResult);

    if ($category) {
        $points = $category['defaultPoints'];
    } else {
        die("Antwortkategorie mit ID $categoryId nicht gefunden.");
    }

    // Füge die Antwort in die `scenarioanswers`-Tabelle ein
    mysqli_stmt_bind_param($stmtInsert, "isssi", $scenarioId, $answer[0], $answer[1], $categoryId, $points);
    if (!mysqli_stmt_execute($stmtInsert)) {
        die("Fehler beim Einfügen der Antwort: " . mysqli_error($con));
    }
}

    // Transaktion abschlißen
    mysqli_commit($con);

    echo json_encode(["status" => "success", "message" => "Szenario und Antworten erfolgreich gespeichert!"]);

} catch (Exception $e) {
    // Fehler -> rolle die Transaktion zurück
    mysqli_roll_back($con);
    echo json_encode(["status" => "error", "message" => $e->getMessage()]);
}

// Schließen der Verbindung
mysqli_close($con);

?>