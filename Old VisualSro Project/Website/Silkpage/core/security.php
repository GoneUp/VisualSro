<?php
class security
{
function is_secure($string)
            {
            $pattern = "#[^a-zA-Z0-9]#";
                if(preg_match($pattern,$string)==true)return false;
                        else
                        return true;
                }            
}
?> 