using UnityEngine;
using UnityEngine.Events;

public class Action
{

    public Sprite Preview { get; private set; }
    private UnityAction codeToExecute;
    private float requiredTime;
    private float remainingTime;
    
    public float TimeRatio { get { return remainingTime / requiredTime; } }
    public bool isDone { get { return remainingTime <= 0; } }
    
    public Action( Sprite preview, float requiredTime, UnityAction codeToExecute )
    {
        this.Preview = preview;

        this.requiredTime = requiredTime;
        this.remainingTime = requiredTime;

        this.codeToExecute = codeToExecute;
    }

    public void updateRemainingTime( float ammount )
    {
        remainingTime -= ammount;
    }

    public void execute()
    {
        codeToExecute.Invoke();
    }
}