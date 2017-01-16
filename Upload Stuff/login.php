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
	$active		= "true";

	if ($mysqli->connect_errno)  {
		die("Connection Failed: Could not Connect to Server");
	}

	$sql = "SELECT * FROM Users WHERE username='" . $user . "'";
	$result = $mysqli->query($sql);

	if (!$result) {
		die("Error: failed executing command");
	}

	$userRow = $result->fetch_assoc();

	if (!$userRow)  {
		die("User does not exist");
	}
	
	if ($pass != $userRow['password']) {
		die("Wrong Password");
	}

	if ($hwid != $userRow['hwid']) {
		die("HWID Changed");
	}
	
	if ($active != $userRow['active']) {
		die("No Subscription");
	}		
			
	die("Successfully Logged in.");	
	
	$mysqli->Close();
}

?>