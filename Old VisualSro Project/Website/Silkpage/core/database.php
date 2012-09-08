<?php
class database {
  
    public function connect($host, $user, $pass)
    {
       $this->connect  =  mysql_connect($host, $user, $pass);
       if (!$this->connect) 
       {
         die("Cannot connect to database"); 
         
       } else 
       {
        return $this->connect;
       }
    }
    public function select_database($dbname)
    {
       $this->select_db = mysql_select_db($dbname);
       if (!$this->select_db)
       {
        die("Cannot connecy to database $dbname");
       } else 
       {
            return $this->select_db;
       }
    }
    public function query($query)
    {
        $this->query = mysql_query($query);
        return $this->query;
    }
}
?>