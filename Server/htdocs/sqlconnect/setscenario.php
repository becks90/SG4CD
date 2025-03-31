<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

error_reporting(E_ALL);    
ini_set('display_errors', 1);

//connection variables
$user = 'root';
$password = 'root';
$db = 'sqlconnect';
$host = 'localhost';
$port = 3306;

$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

if (!$success) {
    echo json_encode(["status" => "error", "message" => "Verbindung zur Datenbank fehlgeschlagen: " . mysqli_connect_error()]);
    exit();
}

// Eingabewerte prüfen
$userId = isset($_POST['userId']) ? (int) $_POST['userId'] : null;
$scenarioId = isset($_POST['scenarioId']) ? (int) $_POST['scenarioId'] : null;
$answerId = isset($_POST['answerId']) ? (int) $_POST['answerId'] : null;
$answerCategory = isset($_POST['answercategory']) ? (int) $_POST['answercategory'] : null;
$categoryId = isset($_POST['categoryId']) ? (int) $_POST['categoryId'] : null;

// Sicherstellen, dass alle benötigten Eingabewerte vorhanden sind
if (is_null($userId) || is_null($scenarioId) || is_null($answerId) || is_null($answerCategory)) {
    echo json_encode(["status" => "error", "message" => "Fehlende Eingabewerte!"]);
    exit();
}

$date = date('Y-m-d H:i:s');

// Löschen des Eintrags mit der niedrigsten orderId und passender nextScenarioId oder nextCategoryId
$sqlDelete = "
    DELETE scenarioplan 
    FROM scenarioplan
    JOIN (
        SELECT MIN(questionOrder) AS minOrderId
        FROM scenarioplan
        WHERE userId = ? 
        AND (nextScenarioId = ? OR nextCategoryId = ?)
    ) AS min_order
    ON scenarioplan.questionOrder = min_order.minOrderId
    WHERE scenarioplan.userId = ?";
    
$stmtDelete = $con->prepare($sqlDelete);

if ($stmtDelete === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung zum Löschen: " . $con->error]);
    exit();
}

$stmtDelete->bind_param("iiii", $userId, $scenarioId, $categoryId, $userId);
$successDelete = $stmtDelete->execute();

if (!$successDelete) {
    echo json_encode(["status" => "error", "message" => "Fehler beim Löschen des Eintrags: " . $stmtDelete->error]);
    exit();
}

// Einfügen des neuen Eintrags in playedscenarios
$sqlInsert = "INSERT INTO playedscenarios (userId, scenarioId, answerId, answercategory, date) VALUES (?, ?, ?, ?, ?)";
$stmtInsert = $con->prepare($sqlInsert);

if ($stmtInsert === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung zum Einfügen: " . $con->error]);
    exit();
}

$stmtInsert->bind_param("iiiis", $userId, $scenarioId, $answerId, $answerCategory, $date);
$successInsert = $stmtInsert->execute();

if ($successInsert) {
    // Hole das zuletzt eingefügte Szenario
    $sqlSelect = "
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
        WHERE ps.id = LAST_INSERT_ID()"; // Letzte eingefügte ID für diese Verbindung
    
    $result = $con->query($sqlSelect);
    
    if ($result->num_rows > 0) {
        // Die Szenarien in einem Array speichern
        $scenariosPlayed = [];
        
        while ($row = $result->fetch_assoc()) 
        {
        $row['id'] = (int) $row['id'];
        $row['category_name'] = (string) $row['category_name'];
        $row['answer'] = (string) $row['answer'];
        $row['answercategory_name'] = (string) $row['answercategory_name'];
        $row['date'] = (string) $row['date'];
        
        $scenariosPlayed[] = $row;
    }
        
        // Antwort zurückgeben mit Status und Szenarien
        echo json_encode([
            "status" => "success",
            "scenariosPlayed" => $scenariosPlayed
        ]);
    } else {
        echo json_encode(["status" => "error", "message" => "Kein Szenario gefunden"]);
    }
} else {
    echo json_encode(["status" => "error", "message" => "Fehler beim Speichern des absolvierten Szenarios: " . $stmtInsert->error]);
}

// Schließen der Statements und der Verbindung
$stmtDelete->close();
$stmtInsert->close();
$con->close();
?>