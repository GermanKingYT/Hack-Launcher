<?php

$dbhost   	= "HOST";
$dbname   	= "DB-NAME";
$dbuser   	= "DB-USER";
$dbpass   	= "DB-PASS";

if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid'])) 
{
	$mysqli = new mysqli($dbhost, $dbuser , $dbpass, $dbname);
	
	$user       = mysqli_real_escape_string($mysqli, $_POST['username']);
	$pass       = mysqli_real_escape_string($mysqli, $_POST['password']);
	$hwid      	= mysqli_real_escape_string($mysqli, $_POST['hwid']);
	$active		= "false";

	if ($mysqli->connect_errno)  {
		die("Connection Failed: " . $mysqli->connect_error);
	}

	$check_hwid			= "SELECT * FROM Users WHERE hwid='" . $hwid . "'";
	$check_username 	= "SELECT * FROM Users WHERE username='" . $user . "'";
	$result_hwid		= $mysqli->query($check_hwid);
	$result_username 	= $mysqli->query($check_username);
	
	if (mysqli_num_rows($result_hwid) > 0)  {
		die("You already have an Account");
	}
	
	if (mysqli_num_rows($result_username) > 0)  {
		die("User already exists");
	}
	
	// Change TestDB to your DB Name
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
