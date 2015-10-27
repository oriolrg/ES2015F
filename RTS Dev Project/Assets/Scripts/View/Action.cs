
public class Action
{
    /*
    public ActionData Data { get; private set; }*/
    private Command command;
    private float remainingTime;
    //public float TimeRatio { get { return remainingTime / Data.requiredTime; } }
    public bool isDone { get { return remainingTime <= 0; } }
    /*
    public Action( ActionData data, Command command )
    {
        this.Data = data;
        this.command = command;
        this.remainingTime = data.requiredTime;
    } */

    public void updateRemainingTime( float timeGone )
    {
        remainingTime -= timeGone;
    }
    /*
    public ActionData getData() { return Data;  }
    */
    public void execute()
    {
        command();
    }
}