<?php

$dbhost = "HOST";
$dbname = "DB-NAME";
$dbuser = "DB-USER";
$dbpass = "DB-PASS";

$encryptionPass	= "ENCRYPTION-PASSWORD";

if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid'])) 
{
	$mysqli = new mysqli($dbhost, $dbuser , $dbpass, $dbname);
	
	$encrypted_user	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['username']),"AES-128-ECB",$encryptionPass);
	$encrypted_pass	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['password']),"AES-128-ECB",$encryptionPass);
	$encrypted_hwid	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['hwid']),"AES-128-ECB",$encryptionPass);
	$active	= "true";
	
	if ($mysqli->connect_errno)  {
		die("Code: 731446");
	}

	$sql = "SELECT * FROM Users WHERE username='" . mysqli_real_escape_string($mysqli,$encrypted_user) . "'";
	$result = $mysqli->query($sql);

	if (!$result) {
		die("Code: 694143");
	}

	$userRow = $result->fetch_assoc();

	if (!$userRow)  {
		die("Code: 107177");
	}
	
	if ($userRow['try'] == "4") {
		die ("Code: 933545");
	}
	
	if ($encrypted_pass != $userRow['password']) {
		if ($userRow['try'] == "0") {
			$cmd = "UPDATE Users SET try='1' WHERE username='" . mysqli_real_escape_string($mysqli,$encrypted_user) . "'";
			$mysqli->query($cmd);
			die("Code: 974498");
		}
		if ($userRow['try'] == "1") {
			$cmd = "UPDATE Users SET try='2' WHERE username='" . mysqli_real_escape_string($mysqli,$encrypted_user) . "'";
			$mysqli->query($cmd);
			die("Code: 375292");
		}
		if ($userRow['try'] == "2") {
			$cmd = "UPDATE Users SET try='3' WHERE username='" . mysqli_real_escape_string($mysqli,$encrypted_user) . "'";
			$mysqli->query($cmd);
			die("Code: 865696");
		}
		if ($userRow['try'] == "3") {
			$cmd = "UPDATE Users SET try='4' WHERE username='" . mysqli_real_escape_string($mysqli,$encrypted_user). "'";
			$mysqli->query($cmd);
			die("Code: 548234");
		}
	}

	$cmd = "UPDATE Users SET try='0' WHERE username='" . mysqli_real_escape_string($mysqli,$encrypted_user) . "'";
	$mysqli->query($cmd);
	
	if ($encrypted_hwid != $userRow['hwid']) {
		die("Code: 526798");
	}
	
	if ($active != $userRow['active']) {
		die("Code: 913280");
	}		
			
	die("Code: 959201");	
	
	$mysqli->Close();
}

?>
