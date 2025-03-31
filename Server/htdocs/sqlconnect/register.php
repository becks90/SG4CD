<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

// Fehleranzeige aktivieren
ini_set('display_errors', 1);
error_reporting(E_ALL);

// Verbindungsvariablen
$user = 'root';
$password = 'root';
$db = 'sqlconnect';
$host = 'localhost';
$port = 3306;

// Verbindung zur Datenbank
$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

if (!$success) {
    echo json_encode(["status" => "error", "message" => "Verbindung zur Datenbank fehlgeschlagen: " . mysqli_connect_error()]);
    exit();
}

// Übermittelte Werte
$username = $_POST['Name'];
$password = $_POST['password'];  // Das Passwort wird noch unverschlüsselt übergeben
$email = $_POST['email'];
$regdate = $_POST['registrationDate'];

// Überprüfung auf bereits registrierte Benutzernamen oder E-Mails
$query = "SELECT username FROM users WHERE username=? OR email=?";
$stmt = mysqli_prepare($con, $query);
mysqli_stmt_bind_param($stmt, "ss", $username, $email);
mysqli_stmt_execute($stmt);
$result = mysqli_stmt_get_result($stmt);

if (mysqli_num_rows($result) > 0) {
    echo json_encode(["status" => "error", "message" => "Benutzername oder E-Mail bereits registriert!"]);
    exit();
}

// Passwort hashen
$hashed_password = password_hash($password, PASSWORD_DEFAULT);  // Das Passwort wird sicher gehasht

// Neuen Benutzer einfügen
$insertquery = "INSERT INTO users (username, password, lastlogin, name, email, registration, userlevel) 
                VALUES (?, ?, ?, ?, ?, ?, '1')";
$stmt_insert = mysqli_prepare($con, $insertquery);
mysqli_stmt_bind_param($stmt_insert, "ssssss", $username, $hashed_password, $regdate, $username, $email, $regdate);

if (mysqli_stmt_execute($stmt_insert)) {
    echo json_encode(["status" => "success", "message" => "Benutzer erfolgreich angelegt!"]);
} else {
    echo json_encode(["status" => "error", "message" => "Fehler beim Anlegen des Benutzers: " . mysqli_error($con)]);
}

mysqli_close($con);
?>