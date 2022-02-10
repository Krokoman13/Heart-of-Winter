using System.Collections;
using System.Collections.Generic;

public static class PlayerInfo
{
    public static int ID = 0;
    public static string name
    {
        get { return "Player " + ID.ToString(); }
    }
    
    public static string shortName
    {
        get { return "P" + ID.ToString(); }
    }

    public static string character;
}
