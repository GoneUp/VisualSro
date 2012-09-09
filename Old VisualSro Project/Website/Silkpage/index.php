<?
include("core/security.php"); //including security class?
$check = new security(); //definition security class
//checking GET values.
 if(empty($_GET["jid"]) or empty($_GET["key"]) or $check->is_secure($_GET["jid"]) == false or $check->is_secure($_GET["key"]) == false) {
echo"Please contact administrator";
} else {
$jid = $_GET["jid"];
$key = $_GET["key"];
}


include("config.php"); //including config
include("core/database.php"); //including database class
include("core/display.php"); // including display class
$database = new database; // definition class database
$database->connect($cfg['DBHOST'], $cfg['DBUSER'], $cfg['DBPASS']); //connecting to database(by database class)
$database->select_database($cfg['DBNAME']); //Selecting database( by database class)
$display = new display;

//if variables exist 
if (isset($jid, $key)) {
    if (empty($jid) or empty($key)) 
    {
        print"<br/>";
     echo"Please contact administrator";   
    } else  
    {
	$query = mysql_num_rows(mysql_query("select * FROM users WHERE id='$jid' AND sessionID='$key'"));
				if($query <= 0) {
    			print "Please contact administrator";
} else {

        $display->show_all();
       }
    }
}

?>