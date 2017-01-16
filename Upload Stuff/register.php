<?php

$dbhost   	= "HOST";
$dbname   	= "DB-NAME";
$dbuser   	= "DB-USER";
$dbpass   	= "DB-PASS";

if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid'])) 
{
	$user       = $_POST['username'];
	$pass       = $_POST['password'];
	$hwid      	= $_POST['hwid'];
	$active		= "false";
		
	$mysqli = new mysqli($dbhost, $dbuser , $dbpass, $dbname);

	if ($mysqli->connect_errno)  {
		die("Connection Failed: " . $mysqli->connect_error);
	}

	$check_username 	= "SELECT * FROM Users WHERE username='" . $user . "'";
	$check_password		= "SELECT * FROM Users WHERE password='" . $pass . "'";
	$result_username 	= $mysqli->query($check_username);
	$result_password	= $mysqli->query($check_password);
	
	if (mysqli_num_rows($result_username) > 0)  {
		die("User already exists");
	}
	
	if (mysqli_num_rows($result_password) > 0)  {
		die("You already have an Account");
	}
	
	$sql = "INSERT INTO `TestDB`.`Users` (`username`, `password`, `hwid`, `active`) VALUES ('" . $user . "', '" . $pass . "', '" . $hwid . "', '" . $active . "')";
	$result = $mysqli->query($sql);

	if (!$result) {
		die("Error: ".$mysqli->error);
	} else {
		die("Registered");
	}

	$mysqli->Close();
}

?>