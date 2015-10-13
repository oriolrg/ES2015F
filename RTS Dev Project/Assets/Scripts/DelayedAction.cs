using UnityEngine;

public class DelayedAction : Action
{
    private float totalTime;
    private float time;
    public float TimeRatio { get { return time / totalTime; } }
    
    public DelayedAction( Command command, Sprite sprite, float totalTime ) : base(command, sprite)
    {
        this.totalTime = totalTime;
        this.time = totalTime;
    } 
}