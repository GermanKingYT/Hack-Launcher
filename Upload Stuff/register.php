<?php
if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['hwid'])) 
{
	if(!@include("settings.php")){
		die("Code: 140483");
	}
	
	$encrypted_user	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['username']),"AES-128-ECB",$encryptionPass);
	$encrypted_pass	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['password']),"AES-128-ECB",$encryptionPass);
	$encrypted_hwid	= openssl_encrypt(mysqli_real_escape_string($mysqli,$_POST['hwid']),"AES-128-ECB",$encryptionPass);
	$active	= "false";
	$try	= "0";
	
	$check_hwid			= "SELECT * FROM ".$dbtable." WHERE hwid='".$encrypted_hwid."'";
	$check_username 	= "SELECT * FROM ".$dbtable." WHERE username='".$encrypted_user."'";
	$result_hwid		= $mysqli->query($check_hwid);
	$result_username 	= $mysqli->query($check_username);
	
	if (mysqli_num_rows($result_hwid) > 0)  {
		die("Code: 269598");
	}
	
	if (mysqli_num_rows($result_username) > 0)  {
		die("Code: 708385");
	}
	
	$sql = "INSERT INTO `TestDB`.`Users` (`username`, `password`, `hwid`, `try`, `active`) VALUES ('".$encrypted_user."', '".$encrypted_pass."', '".$encrypted_hwid."', '".$try."', '".$active."')";
	$result = $mysqli->query($sql);

	if (!$result) {
		die("Code: 160674");
	} else {
		die("Code: 934045");
	}

	$mysqli->Close();
}
?>
