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
    echo json_encode(["message" => "Verbindung zur Datenbank fehlgeschlagen!"]);
    exit();
}

// Übermittelte Daten
$advisorId = $_POST['id'];

// Abrufen der Benutzer, die betreut werden
$query = "
    SELECT users.id, users.username, users.name, users.email, users.lastlogin, users.streak, users.recordstreak, 
           users.registration, users.userlevel, users.userpoints, users.usertype
    FROM advisedperson
    JOIN users ON advisedperson.userid = users.id
    WHERE advisedperson.advisorid = ?
    ORDER BY users.id ASC";
$stmt = $con->prepare($query);
$stmt->bind_param("i", $advisorId);
$stmt->execute();
$result = $stmt->get_result();

$users = [];
while ($row = $result->fetch_assoc()) {
    $users[] = $row;
}

echo json_encode($users);

$stmt->close();
$con->close();

?>