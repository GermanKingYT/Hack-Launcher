<?php
if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid']) && isset($_POST['token']))
{
	if(!@include("settings.php")){
		die("Code: 140483");
	}
	
	$encrypted_user	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['username']),"aes-256-gcm",$encryptionPass);
	$encrypted_pass	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['password']),"aes-256-gcm",$encryptionPass);
	$encrypted_hwid	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['hwid']),"aes-256-gcm",$encryptionPass);
    $token = mysqli_real_escape_string($mysqli,$_POST['token']);
	$active	= "true";

	$sql = "SELECT * FROM ".$dbtable." WHERE username='".$encrypted_user."'";
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
			$cmd = "UPDATE ".$dbtable." SET try='1' WHERE username='".$encrypted_user."'";
			$mysqli->query($cmd);
			die("Code: 974498");
		}
		if ($userRow['try'] == "1") {
			$cmd = "UPDATE ".$dbtable." SET try='2' WHERE username='".$encrypted_user."'";
			$mysqli->query($cmd);
			die("Code: 375292");
		}
		if ($userRow['try'] == "2") {
			$cmd = "UPDATE ".$dbtable." SET try='3' WHERE username='".$encrypted_user."'";
			$mysqli->query($cmd);
			die("Code: 865696");
		}
		if ($userRow['try'] == "3") {
			$cmd = "UPDATE ".$dbtable." SET try='4' WHERE username='".$encrypted_user."'";
			$mysqli->query($cmd);
			die("Code: 548234");
		}
	}

	$cmd = "UPDATE ".$dbtable." SET try='0' WHERE username='".$encrypted_user."'";
	$mysqli->query($cmd);
	
	if ($encrypted_hwid != $userRow['hwid']) {
		die("Code: 526798");
	}
	
	if ($active != $userRow['active']) {
		die("Code: 913280");
	}		
			
	die($token);
	
	$mysqli->Close();
}
?>
