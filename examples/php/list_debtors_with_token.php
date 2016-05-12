<?php
header('Content-type: text/html; charset="utf-8"');
?>
<html>
<head>
	<title>e-conomic SOAP API PHP example (token)</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
	<style type="text/css">
 		body { font-family: Verdana; }

 		h1 { font-size: 15px; }

 		td { font-size: 12px; padding: 5px; }

 		.header_row { background-color: FFCABD; }

 		.even_row { background-color: #FFFEAB; }

 		.odd_row { background-color: #FFFD82; }

 		.white_field { background-color: #FFFFFF;}
	</style>
</head>
<body>
<?php
try
{
	// Helper function to check query parameters.
	function checkParameter($param)
	{
		if (!isset($_REQUEST[$param]))
		{
			echo "Missing <code>" . $param . "</code> parameter in query string.";

			exit(0);
		}
	}

	checkParameter("agreementGrantToken");
	checkParameter("appSecretToken");
	$me = $_SERVER['PHP_SELF'];

	$wsdlUrl = 'https://api.e-conomic.com/secure/api1/EconomicWebservice.asmx?WSDL';

	$client = new SoapClient(
		$wsdlUrl,
		array(
			"trace" => 1,
			"exceptions" => 1,
			"features" => SOAP_SINGLE_ELEMENT_ARRAYS,
			"stream_context" => stream_context_create(
				array(
					"http" => array(
						"header" => "X-EconomicAppIdentifier: Awesomeness to the max"
					)
				)
			)
		)
	);
	$client->ConnectWithToken(
		array(
			'token' 	=> $_REQUEST['agreementGrantToken'],
			'appToken'	=> $_REQUEST['appSecretToken']
		)
	);

	if ($_SERVER['REQUEST_METHOD'] == 'POST' && $_POST['action'] == 'create_debtor')
	{
		try
		{
			//Fetch first DebtorGroup
			$debtorGroupHandles = $client->debtorGroup_GetAll()->DebtorGroup_GetAllResult->DebtorGroupHandle;

			//Fetch first TermOfPayment (Highly cacheable data)
			$termOfPaymentHandles = $client->TermOfPayment_GetAll()->TermOfPayment_GetAllResult->TermOfPaymentHandle;

			$newDebtorFromData = $client->Debtor_CreateFromData(
								array( 'data' =>
									array(
										'Handle'				=> array('Number' => $_POST['debtor_number']),
										'Number'            	=> $_POST['debtor_number'],
										'DebtorGroupHandle' 	=> array_values($debtorGroupHandles)[0],
										'Name'              	=> $_POST['debtor_name'],
										'VatZone'           	=> 'EU',
										'CurrencyHandle'		=> array('Code' => 'DKK'),
										'IsAccessible'			=> true,
										'Ean'					=> null,
										'Address'				=> $_POST['debtor_address'],
										'PostalCode'			=> $_POST['debtor_postalcode'],
										'City'					=> $_POST['debtor_city'],
										'Country'				=> $_POST['debtor_country'],
										'CINumber'				=> $_POST['debtor_cinumber'],
										'TermOfPaymentHandle'	=> array_values($termOfPaymentHandles)[0]
									)
								)
							)->Debtor_CreateFromDataResult;

			print("<p>A new debtor has been created.</p>");
		}
		catch(Exception $exception)
		{
			print("<p><b>Could not create debtor.</b></p>");
			print("<p><i>" . $exception->getMessage() . "</i></p>");
		}
	}

	// Fetch list of all debtors.
	$debtorHandles = $client->Debtor_GetAll()->Debtor_GetAllResult->DebtorHandle;
	$debtorDataObjects = $client->Debtor_GetDataArray(
							array('entityHandles' => $debtorHandles)
						)->Debtor_GetDataArrayResult->DebtorData;
?>
	<h1>Debtors</h1>
	<table width="864px" border="0">
		<tr class="header_row">
			<td><b>Number</b></td>
			<td><b>Name</b></td>
			<td><b>Address</b></td>
			<td><b>PostalCode</b></td>
			<td><b>City</b></td>
			<td><b>Country</b></td>
			<td><b>CINumber/CVR</b></td>
			<td class="white_field"></td>
		</tr>
	<?php foreach ($debtorDataObjects as $i => $debtorData) : ?>
		<tr class="<?php if($i % 2 == 0) echo 'even_row'; else echo 'odd_row' ?>">
			<form action="<?php echo $me . "?agreementGrantToken=" . $_REQUEST['agreementGrantToken'] . "&appSecretToken=" . $_REQUEST['appSecretToken'];?>" method="post">
					<td><?php if (property_exists($debtorData,'Number')) { print $debtorData->Number; } ?>&nbsp;</td>
					<td><?php if (property_exists($debtorData,'Name')) { print $debtorData->Name; } ?>&nbsp;</td>
					<td><?php if (property_exists($debtorData,'Address')) { print $debtorData->Address; } ?>&nbsp;</td>
					<td><?php if (property_exists($debtorData,'PostalCode')) { print $debtorData->PostalCode; } ?>&nbsp;</td>
					<td><?php if (property_exists($debtorData,'City')) { print $debtorData->City; } ?>&nbsp;</td>
					<td><?php if (property_exists($debtorData,'Country')) { print $debtorData->Country; } ?>&nbsp;</td>
					<td><?php if (property_exists($debtorData,'CINumber')) { print $debtorData->CINumber; } ?>&nbsp;</td>
					<td class="white_field">
						<input type="hidden" name="action" value="show_orders">
						<input type="hidden" name="debtor_number" value="<?php print $debtorData->Number ?>">
						<input type="submit" value="Show orders">
					</td>
			</form>
		</tr>
	<?php endforeach; ?>
	</table>
<?php	//Delete an order

	if ($_SERVER['REQUEST_METHOD'] == 'POST' && $_POST['action'] == 'delete_order')
	{
		try
		{
			$client->Order_Delete(array('orderHandle' => array('Id' => $_POST['order_id'])));

			echo "Order deleted.";
		}
		catch(Exception $exception)
		{
			print("<p><b>Error deleting order number ". $_POST['order_number']. " with order id: ". $_POST['order_id']. "</b></p>");
			print("<p><i>" . $exception->getMessage() . "</i></p>");
		}
	}

	//Show a debtor's orders
	if ($_SERVER['REQUEST_METHOD'] == 'POST' && $_POST['action'] == 'show_orders')
	{
		try
		{
			$orderHandles = $client->Debtor_GetOrders(
								array('debtorHandle' => array('Number' => $_POST['debtor_number']))
							)->Debtor_GetOrdersResult->OrderHandle;

			$num_orders = count($orderHandles);

			if ($num_orders > 0)
			{
				$orderDataObjects = $client->Order_GetDataArray(
											array('entityHandles' => $orderHandles)
									)->Order_GetDataArrayResult->OrderData;
				?>
				<h1>Orders</h1>
				<table border="0">
					<tr class="header_row">
						<td><b>Order number</b></td>
						<td><b>Order date</b></td>
						<td><b>Debtor name</b></td>
						<td><b>Delivery addr.</b></td>
						<td><b>Order total</b></td>
						<td class="white_field"></td>
					</tr>
					<?php foreach ($orderDataObjects as $i => $orderData) : ?>
					<tr class="<?php if($i % 2 == 0) echo 'even_row'; else echo 'odd_row' ?>">
						<form action="<?php echo $me . "?agreementGrantToken=" . $_REQUEST['agreementGrantToken'] . "&appSecretToken=" . $_REQUEST['appSecretToken'];?>" method="post">
						<td ><?php print $orderData->Number ?>&nbsp;</td>
						<td><?php print substr($orderData->Date, 0,10); ?>&nbsp;</td>
						<td ><?php print $orderData->DebtorName ?>&nbsp;</td>
						<td ><?php print $orderData->DeliveryAddress ?>&nbsp;</td>
						<td ><?php print $orderData->NetAmount ?>&nbsp;</td>
						<td  class="white_field">
							<input type="hidden" name="action" value="delete_order">
							<input type="hidden" name="order_id" value="<?php print $orderData->Id ?>">
							<input type="submit" value="Delete order">
						</td>
						</form>
					</tr>
					<?php endforeach; ?>
				</table>
			<?php
			}
			else
			{
				echo "This debtor has no orders";
			}
		}
		catch(Exception $exception)
		{
			print("<p><b>Error fetching orders for the selected debtor.</b></p>");
			print("<p><i>" . $exception->getMessage() . "</i></p>");
		}
	}
?>
<h1>Create debtor</h1>
<form action="<?php echo $me . "?agreementGrantToken=" . $_REQUEST['agreementGrantToken'] . "&appSecretToken=" . $_REQUEST['appSecretToken'];?>" method="post">
	<table border="0">
		<tr>
			<td>Number</td><td><input type="text" name="debtor_number"></td>
		</tr>
		<tr>
			<td>Name</td><td><input type="text" name="debtor_name"></td>
		</tr>
		<tr>
			<td>Address</td><td><input type="text" name="debtor_address"></td>
		</tr>
		<tr>
			<td>Postal Code</td><td><input type="text" name="debtor_postalcode"></td>
		</tr>
		<tr>
			<td>City</td><td><input type="text" name="debtor_city"></td>
		</tr>
		<tr>
			<td>Country</td><td><input type="text" name="debtor_country"></td>
		</tr>
		<tr>
			<td>Corporate Identification No. (CVR)</td><td><input type="text" name="debtor_cinumber"></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<input type="hidden" name="action" value="create_debtor">
				<input type="submit" value="Create">
			</td>
		</tr>
	</table>
</form>
<?php
	$client->Disconnect();
}
catch(Exception $exception)
{
	print("<p><i>" . $exception->getMessage() . "</i></p>");

	$client->Disconnect();
}
?>
</body>
</html>