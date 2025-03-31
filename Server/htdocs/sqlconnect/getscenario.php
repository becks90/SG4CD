<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

error_reporting(E_ALL);    
ini_set('display_errors', 0);  // Fehlerausgabe unterdrücken

// Verbindung zur Datenbank herstellen
$user = 'root';
$password = 'root';
$db = 'sqlconnect';
$host = 'localhost';
$port = 3306;

$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

// Überprüfen, ob die Verbindung zur Datenbank erfolgreich war
if (!$success) {
    echo json_encode(["status" => "error", "message" => "Verbindung zur Datenbank fehlgeschlagen: " . mysqli_connect_error()]);
    exit();
}

// Eingabewerte prüfen
$userId = isset($_POST['userId']) ? (int) $_POST['userId'] : null;

if (is_null($userId)) {
    echo json_encode(["status" => "error", "message" => "Fehlende Eingabewerte!"]);
    exit();
}

// Schritt 1: Prüfen, ob es einen Eintrag im scenarioplan mit userId und scenarioId gibt
$sqlScenarioCheck = "SELECT sp.nextScenarioId, sp.nextCategoryId
                     FROM scenarioplan sp
                     WHERE sp.userId = ? AND sp.nextScenarioId IS NOT NULL
                     LIMIT 1";
$stmtScenarioCheck = $con->prepare($sqlScenarioCheck);
if ($stmtScenarioCheck === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage: " . $con->error]);
    exit();
}

$stmtScenarioCheck->bind_param("i", $userId);
$stmtScenarioCheck->execute();
$resultScenarioCheck = $stmtScenarioCheck->get_result();

if ($resultScenarioCheck->num_rows > 0) {
    $row = $resultScenarioCheck->fetch_assoc();
    $nextScenarioId = $row['nextScenarioId'];
    
    // Abfrage, um die Antworten des Szenarios zu holen
    $sqlAnswers = "SELECT sa.id AS answerId, sa.answer, sa.answercategory, sa.response, sa.points
                   FROM scenarioanswers sa
                   WHERE sa.scenarioId = ?";

    $stmtAnswers = $con->prepare($sqlAnswers);
    if ($stmtAnswers === false) {
        echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage für Antworten: " . $con->error]);
        exit();
    }
    $stmtAnswers->bind_param("i", $nextScenarioId);
    $stmtAnswers->execute();
    $resultAnswers = $stmtAnswers->get_result();

    $answers = [];
    while ($answer = $resultAnswers->fetch_assoc()) {
        $answers[] = [
            'answerId' => $answer['answerId'],
            'answer' => $answer['answer'],
            'answercategory' => $answer['answercategory'],
            'response' => $answer['response'],
            'points' => $answer['points']
        ];
    }

    // Szenario-Details holen
    $sqlScenario = "SELECT s.id AS scenarioId, s.name, s.question, s.categoryId
                    FROM scenarios s
                    WHERE s.id = ?";
    $stmtScenario = $con->prepare($sqlScenario);
    if ($stmtScenario === false) {
        echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage für Szenario: " . $con->error]);
        exit();
    }
    $stmtScenario->bind_param("i", $nextScenarioId);
    $stmtScenario->execute();
    $resultScenario = $stmtScenario->get_result();

    if ($resultScenario->num_rows > 0) {
        $rowScenario = $resultScenario->fetch_assoc();
        $scenario = [
            'scenarioId' => $rowScenario['scenarioId'],
            'Name' => $rowScenario['name'],
            'question' => $rowScenario['question'],
            'categoryId' => $rowScenario['categoryId'],
            'answers' => $answers
        ];

        echo json_encode([
            "status" => "success",
            "scenario" => $scenario
        ]);
    } else {
        echo json_encode(["status" => "error", "message" => "Szenario nicht gefunden."]);
    }

} else {
    // Schritt 2: Prüfen, ob es einen Eintrag mit userId und nextCategoryId gibt
    $sqlCategoryCheck = "SELECT s.id AS scenarioId
                         FROM scenarios s
                         JOIN scenarioplan sp ON s.categoryId = sp.nextCategoryId
                         WHERE sp.userId = ? AND sp.nextCategoryId IS NOT NULL
                         LIMIT 1";
    $stmtCategoryCheck = $con->prepare($sqlCategoryCheck);
    if ($stmtCategoryCheck === false) {
        echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage: " . $con->error]);
        exit();
    }

    $stmtCategoryCheck->bind_param("i", $userId);
    $stmtCategoryCheck->execute();
    $resultCategoryCheck = $stmtCategoryCheck->get_result();

    if ($resultCategoryCheck->num_rows > 0) {
        $row = $resultCategoryCheck->fetch_assoc();
        $categoryScenarioId = $row['scenarioId'];

        // Abfrage, um die Antworten des Szenarios zu holen
        $sqlAnswers = "SELECT sa.id AS answerId, sa.answer, sa.answercategory, sa.response, sa.points
                       FROM scenarioanswers sa
                       WHERE sa.scenarioId = ?";

        $stmtAnswers = $con->prepare($sqlAnswers);
        if ($stmtAnswers === false) {
            echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage für Antworten: " . $con->error]);
            exit();
        }

        $stmtAnswers->bind_param("i", $categoryScenarioId);
        $stmtAnswers->execute();
        $resultAnswers = $stmtAnswers->get_result();

        $answers = [];
        while ($answer = $resultAnswers->fetch_assoc()) {
            $answers[] = [
                'answerId' => $answer['answerId'],
                'answer' => $answer['answer'],
                'answercategory' => $answer['answercategory'],
                'response' => $answer['response'],
                'points' => $answer['points']
            ];
        }

        // Szenario-Details holen
        $sqlScenario = "SELECT s.id AS scenarioId, s.name, s.question, s.categoryId
                        FROM scenarios s
                        WHERE s.id = ?";
        $stmtScenario = $con->prepare($sqlScenario);
        if ($stmtScenario === false) {
            echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage für Szenario: " . $con->error]);
            exit();
        }
        $stmtScenario->bind_param("i", $categoryScenarioId);
        $stmtScenario->execute();
        $resultScenario = $stmtScenario->get_result();

        if ($resultScenario->num_rows > 0) {
            $rowScenario = $resultScenario->fetch_assoc();
            $scenario = [
                'scenarioId' => $rowScenario['scenarioId'],
                'Name' => $rowScenario['name'],
                'question' => $rowScenario['question'],
                'categoryId' => $rowScenario['categoryId'],
                'answers' => $answers
            ];

            echo json_encode([
                "status" => "success",
                "scenario" => $scenario
            ]);
        } else {
            echo json_encode(["status" => "error", "message" => "Szenario nicht gefunden."]);
        }
    } else {
        // Schritt 3: Keine Übereinstimmungen gefunden, gebe ein zufälliges Szenario aus
        $sqlRandomScenario = "SELECT id AS scenarioId
                              FROM scenarios
                              ORDER BY RAND()
                              LIMIT 1";
        $resultRandomScenario = $con->query($sqlRandomScenario);

        if ($resultRandomScenario->num_rows > 0) {
            $row = $resultRandomScenario->fetch_assoc();
            $randomScenarioId = $row['scenarioId'];

            // Abfrage, um die Antworten des Szenarios zu holen
            $sqlAnswers = "SELECT sa.id AS answerId, sa.answer, sa.answercategory, sa.response, sa.points
                           FROM scenarioanswers sa
                           WHERE sa.scenarioId = ?";

            $stmtAnswers = $con->prepare($sqlAnswers);
            if ($stmtAnswers === false) {
                echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage für Antworten: " . $con->error]);
                exit();
            }

            $stmtAnswers->bind_param("i", $randomScenarioId);
            $stmtAnswers->execute();
            $resultAnswers = $stmtAnswers->get_result();

            $answers = [];
            while ($answer = $resultAnswers->fetch_assoc()) {
                $answers[] = [
                    'answerId' => $answer['answerId'],
                    'answer' => $answer['answer'],
                    'answercategory' => $answer['answercategory'],
                    'response' => $answer['response'],
                    'points' => $answer['points']
                ];
            }

            // Szenario-Details holen
            $sqlScenario = "SELECT s.id AS scenarioId, s.name, s.question, s.categoryId
                            FROM scenarios s
                            WHERE s.id = ?";
            $stmtScenario = $con->prepare($sqlScenario);
            if ($stmtScenario === false) {
                echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der Abfrage für Szenario: " . $con->error]);
                exit();
            }
            $stmtScenario->bind_param("i", $randomScenarioId);
            $stmtScenario->execute();
            $resultScenario = $stmtScenario->get_result();

            if ($resultScenario->num_rows > 0) {
                $rowScenario = $resultScenario->fetch_assoc();
                $scenario = [
                    'scenarioId' => $rowScenario['scenarioId'],
                    'Name' => $rowScenario['name'],
                    'question' => $rowScenario['question'],
                    'categoryId' => $rowScenario['categoryId'],
                    'answers' => $answers
                ];

                echo json_encode([
                    "status" => "success",
                    "scenario" => $scenario
                ]);
            } else {
                echo json_encode(["status" => "error", "message" => "Kein Szenario gefunden."]);
            }
        } else {
            echo json_encode(["status" => "error", "message" => "Kein Szenario gefunden."]);
        }
    }
}

// Schließen der Verbindung
$stmtScenarioCheck->close();
if (isset($stmtCategoryCheck)) {
    $stmtCategoryCheck->close();
}
if (isset($stmtAnswers)) {
    $stmtAnswers->close();
}
$con->close();
?>