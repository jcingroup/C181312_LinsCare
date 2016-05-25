<?php
require_once '../jq-config.php';
// Connection to the server
$conn = new PDO(DB_DSN,DB_USER,DB_PASSWORD);
// Tell the db that we use utf-8
$conn->query("SET NAMES utf8");

$rowid = $_POST["rowid"];
if(!$rowid) die("Missed parameters");
// Get details
$SQL = "SELECT * FROM employees WHERE EmployeeID=".(int)$rowid;

$qres = $conn->query($SQL);

$result = $qres->fetch( PDO::FETCH_ASSOC);
$s = "<table><tbody>";
$s .= "<tr><td><b>First Name</b></td><td>".$result["FirstName"]."</td>";
$s .= "<td rowspan='9' valign='top'><img src='images/".trim($rowid).".jpg'/></td></tr>";
$s .= "<tr><td><b>Last Name</b></td><td>".$result["LastName"]."</td></tr>";
$s .= "<tr><td><b>Title</b></td><td>".$result["Title"]."</td></tr>";
$s .= "<tr><td><b>Title of Courtesy</b></td><td>".$result["TitleOfCourtesy"]."</td></tr>";
$s .= "<tr><td><b>Birth Date</b></td><td>".$result["BirthDate"]."</td></tr>";
$s .= "<tr><td><b>Hire Date</b></td><td>".$result["HireDate"]."</td></tr>";
$s .= "<tr><td><b>Address</b></td><td>".$result["Address"]."</td></tr>";
$s .= "<tr><td><b>City</b></td><td>".$result["City"]."</td></tr>";
$s .= "<tr><td><b>Postal Code</b></td><td>".$result["PostalCode"]."</td></tr>";
$s .= "</tbody></table>";
echo $s;
?>
