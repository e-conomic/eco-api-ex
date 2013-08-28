<?php

header('Content-type: text/html; charset="utf-8"');

?>

<html>

<head>

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

	$me = $_SERVER['PHP_SELF'];

	

	$wsdlUrl = 'https://api.e-conomic.com/secure/api1/EconomicWebservice.asmx?WSDL';

		

	$client = new SoapClient($wsdlUrl, array("trace" => 1, "exceptions" => 1));    		

	

	$client->ConnectWithToken(array(

		'token' => $_REQUEST['token'],

		'appToken'        => $_REQUEST['appToken']));



	if ($_SERVER['REQUEST_METHOD'] == 'POST' && $_POST['action'] == 'create_debtor')

	{

		try

		{

			$debtorGroupHandles = $client->debtorGroup_GetAll()->DebtorGroup_GetAllResult->DebtorGroupHandle;

			$firstDebtorGroup = $debtorGroupHandles[0];



			$newDebtorHandle = $client->Debtor_Create(array(

				'number'            => $_POST['debtor_number'],

				'debtorGroupHandle' => $firstDebtorGroup,

				'name'              => $_POST['debtor_name'],

				'vatZone'           => 'EU'))->Debtor_CreateResult;



			$client->Debtor_SetAddress(array(

				'debtorHandle' => $newDebtorHandle,

				'value'        => $_POST['debtor_address']));



			print("<p>A new debtor has be created.</p>");

		}

		catch(Exception $exception)

		{

			print("<p><b>Could not create debtor.</b></p>");

			print("<p><i>" . $exception->getMessage() . "</i></p>");

		}

	}



	// Fetch list of all debtors.

	$debtorHandles = $client->Debtor_GetAll()->Debtor_GetAllResult->DebtorHandle;

	$debtorDataObjects =$client->Debtor_GetDataArray(array('entityHandles' => $debtorHandles))->Debtor_GetDataArrayResult->DebtorData;

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

			<td class="white_field"></td>

		</tr>

<?php



	foreach ($debtorDataObjects as $i => $debtorData) {

?>

		<tr class="<?php if($i % 2 == 0) echo 'even_row'; else echo 'odd_row' ?>">

		<form action="<?php echo $me . "?token=" . $_REQUEST['token'] . "&appToken=" . $_REQUEST['appToken'] . "&password=" ;?>" method="post">

				<td><?php print $debtorData->Number ?>&nbsp;</td>

				<td><?php print $debtorData->Name ?>&nbsp;</td>

				<td><?php print $debtorData->Address ?>&nbsp;</td>

				<td><?php print $debtorData->PostalCode ?>&nbsp;</td>

				<td><?php print $debtorData->City ?>&nbsp;</td>

				<td><?php print $debtorData->Country ?>&nbsp;</td>

				<td class="white_field">

					<input type="hidden" name="action" value="show_orders">

					<input type="hidden" name="debtor_number" value="<?php print $debtorData->Number ?>">

					<input type="submit" value="Show orders">

				</td>

		</form>

	</tr>

<?php

	}



?>

	</table>



<?php	//Delete an order

	if ($_SERVER['REQUEST_METHOD'] == 'POST' && $_POST['action'] == 'delete_order')

	{

		try

		{

		$orderHandle = $client->Order_FindByNumber(array('number' => $_POST['order_number']))->Order_FindByNumberResult;

		$client->Order_Delete(array('orderHandle' => $orderHandle));

		echo "Order deleted.";

		}

		catch(Exception $exception)

		{

			print("<p><b>Error delting order with order number: ". $_POST['order_number']. "</b></p>");

			print("<p><i>" . $exception->getMessage() . "</i></p>");

		}

	}

?>



<?php	//Show a debtor's orders

	if ($_SERVER['REQUEST_METHOD'] == 'POST' && $_POST['action'] == 'show_orders')

	{

		try

		{	

			$debtorHandle = $client->Debtor_FindByNumber(array('number' => $_POST['debtor_number']))->Debtor_FindByNumberResult;

			$orderHandles = $client->Debtor_GetOrders(array('debtorHandle' => $debtorHandle))->Debtor_GetOrdersResult->OrderHandle;

			

			$num_orders = count($orderHandles);



			if($num_orders > 0)

			{	

				if($num_orders > 1)

				{

					$orderDataObjects = $client->Order_GetDataArray(array('entityHandles' => $orderHandles))->Order_GetDataArrayResult->OrderData;

				}

				else

				{

					$orderDataObjects[] = $client->Order_GetData(array('entityHandle' => $orderHandles))->Order_GetDataResult;

				}

				?>		

				<h1>Orders</h1>

				<table border="0">

					<tr class="header_row">

						<td><b>Order date</b></td>

						<td><b>Debtor name</b></td>

						<td><b>Delivery addr.</b></td>

						<td><b>Order total</b></td>

						<td class="white_field"></td> 

					</tr>

				<?php

				foreach ($orderDataObjects as $i => $orderData)

				{

					?>

					<tr class="<?php if($i % 2 == 0) echo 'even_row'; else echo 'odd_row' ?>">

				<form action="<?php echo $me . "?token=" . $_REQUEST['token'] . "&appToken=" . $_REQUEST['appToken'];?>" method="post">

				<td><?php print substr($orderData->Date, 0,10); ?>&nbsp;</td>

				<td ><?php print $orderData->DebtorName ?>&nbsp;</td>

				<td ><?php print $orderData->DeliveryAddress ?>&nbsp;</td>

				<td ><?php print $orderData->NetAmount ?>&nbsp;</td>

				<td  class="white_field">

				<input type="hidden" name="action" value="delete_order">

				<input type="hidden" name="order_number" value="<?php print $orderData->Number ?>">

				<input type="submit" value="Delete order">

				</td>

				</form>

			</tr>

					<?php

				}

				echo "</table>";

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

<form action="<?php echo $me . "?token=" . $_REQUEST['token'] . "&appToken=" . $_REQUEST['appToken'];?>" method="post">

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