<?
class display
{

public function show_all()
    {
        
        
        $query = mysql_query("SELECT code_name, real_name, price,  description, image_src FROM item_mall WHERE service=1");
		
		
        echo'<table border= "01" width=30px>
            <tr>
	    <td width=32px height=32px></td>
            <td>Name</td>
            <td>Description</td>
            <td>Price</td>


            </tr>
            <tr>';
		while($wyniki = mysql_fetch_array($query)):
	$image= str_replace('ddj', 'png', $wyniki['image_src']);
			echo '
					
					<td width=32px height=32px><img src=images/'.$image.' />
					<td width=30px>'.$wyniki['real_name'].'</td>
                    			<td width=30px>'.$wyniki['description'].'</td>
                    			<td width=30px>'.$wyniki['price'].'</td>
				  </tr> '
                 ;
			
		endwhile;
        echo'</table>';
	
    }
}
?>