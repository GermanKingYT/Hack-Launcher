<?php
// Connection Variables
$dbhost  = "DB-HOST";
$dbname  = "DB-NAME";
$dbuser  = "DB-USER";
$dbpass  = "DB-PASSWORD";
$dbtable = "DB-TABLE";

// Encryption Password
$encryptionPass	= "ENCRYPTION-PASSWORD";

// Connect to Database
$mysqli = new mysqli($dbhost, $dbuser, $dbpass) or die("Code: 731446");
$verb = $mysqli->select_db($dbname);
?>