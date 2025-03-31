<?php

header("Access-Control-Allow-Origin: *"); // Erlaubt Anfragen von überall (testen)
header("Access-Control-Allow-Methods: POST, GET, OPTIONS"); // Erlaubt diese Methoden
header("Access-Control-Allow-Headers: Content-Type"); // Erlaubt bestimmte Header

// Verbindung
$user = 'root';
$password = 'root';
$db = 'sqlconnect';
$host = 'localhost';
$port = 3306;

// Verbinden
$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

if (!$success) {
    echo json_encode(["status" => "error", "message" => "Verbindung zur Datenbank fehlgeschlagen!"]);
    exit();
}

// SQL-Abfrage
$sql = "
SELECT s.id, s.name, s.categoryId, ec.name AS categoryName
FROM scenarios s
LEFT JOIN ercategories ec ON s.categoryId = ec.id
;";

// Vorbereitung der SQL-Anweisung
$stmt = mysqli_prepare($con, $sql);

if ($stmt === false) {
    error_log('Fehler bei der Vorbereitung der SQL-Abfrage: ' . mysqli_error($con)); // Fehler in Logdatei speichern
    die('Ein Fehler ist aufgetreten. Bitte versuchen Sie es später noch einmal.');
}

// Ausführen der Abfrage
$success = mysqli_stmt_execute($stmt);

if (!$success) {
    error_log('Fehler beim Ausführen der SQL-Abfrage: ' . mysqli_error($con)); // Fehler in Logdatei speichern
    die('Ein Fehler ist aufgetreten. Bitte versuchen Sie es später noch einmal.');
}

// Ergebnis abholen
$result = mysqli_stmt_get_result($stmt);

// Alle Szenarien in ein Array speichern
$scenarios = [];
while ($data = mysqli_fetch_assoc($result)) {
    $scenarios[] = $data; // Füge jede Zeile dem Array hinzu
}

// Überprüfen, ob Daten vorhanden sind
if (count($scenarios) > 0) {
    // Ergebnisse als JSON ausgeben
    echo json_encode(["status" => "success", "scenarios" => $scenarios], JSON_PRETTY_PRINT);
} else {
    echo json_encode(["status" => "error", "message" => "Keine Daten gefunden."]);
}

// Schließen der Verbindung
mysqli_stmt_close($stmt);
mysqli_close($con);

?>