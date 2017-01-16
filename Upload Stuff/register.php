<?php

$dbhost   = "HOST";
$dbname   = "DB-NAME";
$dbuser   = "DB-USER";
$dbpass   = "DB-PASS";

$encryptionPass	= "ENCRYPTION-PASSWORD";
	
if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid'])) 
{
	$mysqli = new mysqli($dbhost, $dbuser , $dbpass, $dbname);
	
	$encrypted_user	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['username']),"AES-128-ECB",$encryptionPass);
	$encrypted_pass	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['password']),"AES-128-ECB",$encryptionPass);
	$encrypted_hwid	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['hwid']),"AES-128-ECB",$encryptionPass);
	$active		= "false";
	$try		= "0";
	
	if ($mysqli->connect_errno)  {
		die("Connection Failed: " . $mysqli->connect_error);
	}

	$check_hwid		= "SELECT * FROM Users WHERE hwid='".mysqli_real_escape_string($mysqli,$encrypted_hwid)."'";
	$check_username 	= "SELECT * FROM Users WHERE username='".mysqli_real_escape_string($mysqli,$encrypted_user)."'";
	$result_hwid		= $mysqli->query($check_hwid);
	$result_username 	= $mysqli->query($check_username);
	
	if (mysqli_num_rows($result_hwid) > 0)  {
		die("You already have an Account");
	}
	
	if (mysqli_num_rows($result_username) > 0)  {
		die("User already exists");
	}
	
	$sql = "INSERT INTO `TestDB`.`Users` (`username`, `password`, `hwid`, `try`, `active`) VALUES ('".mysqli_real_escape_string($mysqli,$encrypted_user)."', '".mysqli_real_escape_string($mysqli,$encrypted_pass)."', '".mysqli_real_escape_string($mysqli,$encrypted_hwid)."', '".mysqli_real_escape_string($mysqli,$try)."', '".mysqli_real_escape_string($mysqli,$active)."')";
	$result = $mysqli->query($sql);

	if (!$result) {
		die("Error: ".$mysqli->error);
	} else {
		die("Registered");
	}

	$mysqli->Close();
}

?>
