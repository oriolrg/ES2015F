
public class QueuedAction
{
    private Action action;
    private float remainingTime;
    private Command command;
    public float TimeRatio { get { return remainingTime / action.requiredTime; } }
    
    public QueuedAction( Command command, Action action )
    {
        this.action = action;
        this.command = command;
        this.remainingTime = action.requiredTime;
    } 

    public void updateRemainingTime( float timeGone )
    {
        remainingTime -= timeGone;
        if (remainingTime < 0)
            command();
    }
}