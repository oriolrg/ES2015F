
public enum Victory
{
    Wonder,
    MapControl,
    Annihilation
}

public static class VictoryExtensions
{
    public static int countdownTime( this Victory victory )
    {
        return 50;
    }

    public static string ToString( this Victory victory )
    {
        string res = "";
        switch(victory)
        {
            case Victory.Wonder: res = "Wonder costructed";
                break;
            case Victory.MapControl: res = "Map Control";
                break;
            case Victory.Annihilation: res = "Annihilation";
                break;            
        }
        return res;
    }

}