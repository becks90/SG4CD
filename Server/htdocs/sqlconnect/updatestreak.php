<?php

header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST, GET, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type");

error_reporting(E_ALL);
ini_set('display_errors', 1);

// Verbindung
$user = 'root';
$password = 'root';
$db = 'sqlconnect';
$host = 'localhost';
$port = 3306;

$con = mysqli_init();
$success = mysqli_real_connect($con, $host, $user, $password, $db, $port);

if (!$success) {
    echo "Verbindung zur Datenbank fehlgeschlagen!";
    exit();
}

// Speicher übermittelte Daten
$id = $_POST['userId'];
$streak = $_POST['streak'];
$recordstreak = $_POST['recordstreak'];
$level = $_POST['level'];
$points = $_POST['points'];
$lastlogin = $_POST['lastlogin'];

// Update streak und lastlogin
$updatestreakquery = "UPDATE users SET streak = " . $streak . ", lastlogin = STR_TO_DATE('" . $lastlogin . "', '%Y-%m-%d %H:%i:%s'), recordstreak = " . $recordstreak . ", userlevel = " . $level . ", userpoints = " . $points . " WHERE id = '" . $id . "';";
$updatestreakresult = mysqli_query($con, $updatestreakquery) or die("Aktualisierung der Datenbank fehlgeschlagen!");

// Aktuelles Level abrufen
$getlevelquery = "SELECT level, points, reward FROM userlevels WHERE level='" . $level . "';";

$getlevel = mysqli_query($con, $getlevelquery) or die("Datenbankabfrage gescheitert!");
if (mysqli_num_rows($getlevel) == 1) {
    $row = mysqli_fetch_assoc($getlevel);
    // Ändere die Antwort, damit sie direkt das Level enthält
    echo json_encode([
        "status" => "success",
        "level" => (int)$row['level'],
        "points" => (int)$row['points'],
        "reward" => $row['reward']
    ]);
}
exit();
?>
