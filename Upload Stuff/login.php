<?php

$dbhost   	= "HOST";
$dbname   	= "DB-NAME";
$dbuser   	= "DB-USER";
$dbpass   	= "DB-PASS";
$encryptionPass	= "ENCRYPTION-PASSWORD";

if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid'])) 
{
	$mysqli 	= new mysqli($dbhost, $dbuser , $dbpass, $dbname);
	
	$user		= mysqli_real_escape_string($mysqli, $_POST['username']);
	$pass		= mysqli_real_escape_string($mysqli, $_POST['password']);
	$hwid      	= mysqli_real_escape_string($mysqli, $_POST['hwid']);
	$active		= "true";

	$encrypted_user	= openssl_encrypt($user,"AES-128-ECB",$encryptionPass);
	$encrypted_pass	= openssl_encrypt($pass,"AES-128-ECB",$encryptionPass);
	$encrypted_hwid	= openssl_encrypt($hwid ,"AES-128-ECB",$encryptionPass);
	
	if ($mysqli->connect_errno)  {
		die("Connection Failed: Could not Connect to Server");
	}

	$sql = "SELECT * FROM Users WHERE username='" . $encrypted_user . "'";
	$result = $mysqli->query($sql);

	if (!$result) {
		die("Error: failed executing command");
	}

	$userRow = $result->fetch_assoc();

	if (!$userRow)  {
		die("User does not exist");
	}
	
	if ($userRow['try'] == "4") {
		die ("Account is Locked please message an Admin");
	}
	
	if ($encrypted_pass != $userRow['password']) {
		if ($userRow['try'] == "0") {
			$cmd = "UPDATE Users SET try='1' WHERE username='" . $encrypted_user . "'";
			$mysqli->query($cmd);
			die("Wrong Password, 3 Try's Left");
		}
		if ($userRow['try'] == "1") {
			$cmd = "UPDATE Users SET try='2' WHERE username='" . $encrypted_user . "'";
			$mysqli->query($cmd);
			die("Wrong Password, 2 Try's Left");
		}
		if ($userRow['try'] == "2") {
			$cmd = "UPDATE Users SET try='3' WHERE username='" . $encrypted_user . "'";
			$mysqli->query($cmd);
			die("Wrong Password, 1 Try Left");
		}
		if ($userRow['try'] == "3") {
			$cmd = "UPDATE Users SET try='4' WHERE username='" . $encrypted_user . "'";
			$mysqli->query($cmd);
			die("Account is now Locked please Message an Admin");
		}
	}

	$cmd = "UPDATE Users SET try='0' WHERE username='" . $encrypted_user . "'";
	$mysqli->query($cmd);
	
	if ($encrypted_hwid != $userRow['hwid']) {
		die("HWID Changed");
	}
	
	if ($active != $userRow['active']) {
		die("No Subscription");
	}		
			
	die("Successfully Logged in.");	
	
	$mysqli->Close();
}

?>
