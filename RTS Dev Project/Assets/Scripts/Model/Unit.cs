using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private UnitData data;
    private List<Command> commands;
    public Queue<Action> Queue { get; private set; }

    [SerializeField] private float health;
    public float HealthRatio { get { return health * 1f / data.stats[Stat.Health]; }}
    public Sprite Preview { get { return data.preview; } }

    protected abstract List<Command> defineCommands();
    private ActionData waitingForInputToEnqueue;

    void Awake()
    {
        health = data.stats[Stat.Health];

        Queue = new Queue<Action>();
        
        // Get the commands set in the subclass
        List<Command> list = defineCommands();

        // Check correct number of commands set
        if (list.Count != data.actions.Count)
        {
            // Incorrect, print error and use default command
            Debug.LogError("Incorrect number of commands assigned." + commands.Count + " vs " + this.commands.Count);
            commands = new List<Command>();
            for (int i = 0; i < data.actions.Count; i++)
                commands.Add(NoCommand);
        }
        else
        {
            // Correct
            commands = list;
        }
    }

    void FixedUpdate()
    {
        if( Queue.Count > 0)
            Queue.Peek().updateRemainingTime(Time.deltaTime);
    }
    
    private void NoCommand()
    {
        Debug.LogError("No command assigned to this action.");
    }

    public void Enqueue(ActionData actionData)
    {
        Command correspondingCommand = commands[data.actions.IndexOf(actionData)];
        Action action = new Action(actionData, correspondingCommand);
        Queue.Enqueue( action );

        // if unique action start working on it
        if( Queue.Count == 1 )
        {
            Invoke("nextAction", action.Data.requiredTime);
        }
    }

    public void EnqueueAfterInput( ActionData actionData )
    {
        waitingForInputToEnqueue = actionData;
    }

    public void InputDone()
    {
        if (waitingForInputToEnqueue != null)
        {
            Enqueue(waitingForInputToEnqueue);
            waitingForInputToEnqueue = null;
        }
        else
        {
            Debug.LogError("Calling InputDone when no action is waiting for input.");
        }
    }

    public void nextAction()
    {
        Action action = Queue.Dequeue();
        action.execute();

        // if there are more actions, start working on the first one
        if (Queue.Count > 0)
        {
            Action nextAction = Queue.Peek();
            Invoke("nextAction", nextAction.Data.requiredTime);
        }
    }

    public List<ActionData> getActionDatas() { return data.actions; }
}
