<?php
header('Content-type: text/html; charset="utf-8"');
?>
<html>
<head>
	<title>e-conomic PHP REST Example</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
</head>
<body>

	<pre id="mememe"></pre>

	<?php
	//Adapted from https://stackoverflow.com/questions/9802788/call-a-rest-api-in-php
	// Method: POST, PUT, GET etc
	// Data: array("param" => "value") ==> index.php?param=value

	//next example will recieve all messages for specific conversation
	$service_url = 'https://restapi.e-conomic.com/self';
	$curl = curl_init($service_url);
	curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
	curl_setopt($curl, CURLOPT_HTTPHEADER, array(
	    	'X-AppSecretToken:demo',
	    	'X-AgreementGrantToken:demo',
	    	'Content-Type:application/json'
	    ));
	$curl_response = curl_exec($curl);
	if ($curl_response === false) {
	    $info = curl_getinfo($curl);
	    curl_close($curl);
	    die('Error occured during curl exec. Additional info: ' . var_export($info));
	}
	curl_close($curl);
	$decoded = json_decode($curl_response);
	if (isset($decoded->response->status) && $decoded->response->status == 'ERROR') {
	    die('Error occured: ' . $decoded->response->errormessage);
	}
	
	echo "<script type='text/javascript'>
		var responseBody =" . $curl_response . "
		document.getElementById('mememe').innerHTML = JSON.stringify(responseBody, null, 4)
	</script>"
?>
</body>
</html>