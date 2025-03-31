<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

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

// Übermittelte Daten
$username = $_POST['name'];
$password_input = $_POST['password'];

// Datenbankeinträge mit username suchen
$loginquery = "SELECT users.id, username, lastlogin, streak, name, recordstreak, email, registration, userlevel, userpoints, usertype, password, ap.advisorid FROM users 
LEFT JOIN advisedperson ap ON ap.userid = users.id
WHERE username = ?";
$stmt = $con->prepare($loginquery);
$stmt->bind_param("s", $username); // Bindet den Parameter (String)
$stmt->execute();
$result = $stmt->get_result();

// Ergebnisse
if ($result->num_rows == 1) {
    $row = $result->fetch_assoc();

    // Ge-hashtes Passwort überprüfen
    if (password_verify($password_input, $row['password'])) {
        
        // Passwort aus den Ergebnissen entfernen
        unset($row['password']);
        
        // Vervollständigen der JSON-Antwort
        echo json_encode([
            "status" => "success",
            "id" => (int)$row['id'],
            "username" => $row['username'],
            "name" => $row['name'],
            "email" => $row['email'],
            "lastlogin" => $row['lastlogin'],
            "streak" => (int)$row['streak'],
            "recordstreak" => (int)$row['recordstreak'],
            "registration" => $row['registration'],
            "userlevel" => (int)$row['userlevel'],
            "userpoints" => (int)$row['userpoints'],
            "usertype" => (int)$row['usertype'],
            "advisorid" => isset($row['advisorid']) ? (int)$row['advisorid'] : null
        ]);
    } else {
        echo json_encode(["status" => "error", "message" => "Login-Daten falsch!"]);
    }
} else {
    echo json_encode(["status" => "error", "message" => "Benutzername nicht gefunden!"]);
}

$stmt->close();
$con->close();

?>
