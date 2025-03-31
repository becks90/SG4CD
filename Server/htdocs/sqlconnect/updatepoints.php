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

$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

if (!$success) {
    echo "Verbindung zur Datenbank fehlgeschlagen: " . mysqli_connect_error();
    exit();
}

// Erhalte die POST-Daten
$userid = isset($_POST['userId']) ? (int) $_POST['userId'] : 0;
$points = isset($_POST['points']) ? (int) $_POST['points'] : 0;
$level = isset($_POST['level']) ? (int) $_POST['level'] : 0;

// Überprüfen, ob die ID, Punkte und Level gültig sind
if ($userid <= 0 || $points < 0 || $level < 0) {
    echo json_encode(["status" => "error", "message" => "Ungültige Daten"]);
    exit();
}

// SQL-Abfrage, um die Punkte und das Level zu aktualisieren
$query = "UPDATE users SET userpoints = ?, userlevel = ? WHERE id = ?";
$stmt = $con->prepare($query);

// Bereite die Parameter vor und führe das Update aus
$stmt->bind_param("iii", $points, $level, $userid);

if ($stmt->execute()) {
    // Erfolgreich aktualisiert

  // Führe eine zusätzliche Abfrage aus, um das level aus der Tabelle userlevels zu holen
    $query_level = "SELECT * FROM userlevels WHERE level = ?";
    $stmt_level = $con->prepare($query_level);
    $stmt_level->bind_param("i", $level);
    $stmt_level->execute();
    
    // Hole das Ergebnis und speichere es in einer Variablen
    $result = $stmt_level->get_result();
    if ($result->num_rows > 0) {
        $levelData = $result->fetch_assoc();  // Hole die erste Zeile der Abfrage
        
        // Gib das Ergebnis als JSON zurück
        echo json_encode([
        "status" => "success",
        "level" => (int)$levelData['level'],
        "points" => (int)$levelData['points'],
        "reward" => $levelData['reward']
        ]);
    } else {
        // Falls keine Daten für das Level gefunden wurden
        echo json_encode([
            "status" => "error",
            "message" => "Benutzerdaten erfolgreich aktualisiert, aber kein entsprechendes Level gefunden"
        ]);
    }
    
} else {
    // Fehler bei der Ausführung der Abfrage
    echo json_encode(["status" => "error", "message" => "Fehler beim Aktualisieren der Benutzerdaten"]);
}

// Schließe die Verbindung
$stmt->close();
$con->close();
?>