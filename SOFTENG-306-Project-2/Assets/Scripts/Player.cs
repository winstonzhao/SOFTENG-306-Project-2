using System.Collections;
using System.Collections.Generic;


public class Player
{
    private string _username;
    
    // Other state info

    public Player()
    {
        _username = "Luna Lovegood";
    }

    public string Username
    {
        get { return _username; }
    }
}
