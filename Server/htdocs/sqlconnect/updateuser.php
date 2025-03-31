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
if (!isset($_POST['userId']) || !isset($_POST['email']) || !isset($_POST['name'])) {
    echo json_encode(["status" => "error", "message" => "Ungültige Eingabedaten"]);
    exit();
}

$userId = (int)$_POST['userId'];
$email = $_POST['email'];
$name = $_POST['name'];
$password = isset($_POST['password']) ? $_POST['password'] : null;  // Passwort optional

// Wenn das Passwort übergeben wurde und eine gültige Länge hat, hash es
if ($password !== null && strlen($password) >= 8) {
    // Passwort mit bcrypt hashen
    $hashedPassword = password_hash($password, PASSWORD_BCRYPT);
    $query = "UPDATE users SET email = ?, name = ?, password = ? WHERE id = ?";
} else {
    // Wenn kein Passwort übergeben wurde, nur email und name aktualisieren
    $hashedPassword = null;
    $query = "UPDATE users SET email = ?, name = ? WHERE id = ?";
}

// Vorbereiten der SQL-Anweisung
$stmt = mysqli_prepare($con, $query);

if ($stmt === false) {
    echo json_encode(["status" => "error", "message" => "Fehler bei der Vorbereitung der SQL-Abfrage"]);
    exit();
}

// Binden der Parameter
if ($hashedPassword !== null) {
    // Wenn Passwort vorhanden ist, binde das Passwort mit ein
    mysqli_stmt_bind_param($stmt, "sssi", $email, $name, $hashedPassword, $userId);
} else {
    // Wenn kein Passwort, binde nur email und name
    mysqli_stmt_bind_param($stmt, "ssi", $email, $name, $userId);
}

// Ausführen der SQL-Anweisung
if (mysqli_stmt_execute($stmt)) {
    // Erfolgreich aktualisiert
    echo json_encode(["status" => "success", "message" => "Benutzerdaten wurden erfolgreich aktualisiert"]);
} else {
    // Fehler beim Aktualisieren
    echo json_encode(["status" => "error", "message" => "Fehler beim Aktualisieren der Benutzerdaten: " . mysqli_error($con)]);
}

// Schließen der Verbindung
mysqli_stmt_close($stmt);
mysqli_close($con);

?>