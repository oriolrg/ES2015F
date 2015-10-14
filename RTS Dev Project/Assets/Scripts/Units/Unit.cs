using UnityEngine;
using System.Collections.Generic;
using System;

public struct ActionWithCommand
{
    public ActionData action;
    public Command command;
}

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private UnitData data;
    public List<QueuedAction> DelayedActions { get; private set; }
    private List<Command> commands;

    [SerializeField] private float health;

    public List<ActionWithCommand> getActionStruct()
    {
        List<ActionWithCommand> res = new List<ActionWithCommand>();

        for (int i = 0; i < data.actions.Count; i++)
            res.Add( new ActionWithCommand() { action = data.actions[i], command=commands[i] });

        return res;
    }

    public float HealthRatio { get { return health * 1f / data.stats[Stat.Health]; }}
    public Sprite Preview { get { return data.preview; } }

    protected abstract List<Command> defineCommands();

    void Awake()
    {
        health = data.stats[Stat.Health];

        DelayedActions = new List<QueuedAction>();
        
        // Get the commands set in the subclass
        List<Command> list = defineCommands();

        // Check correct number of commands set
        if (list.Count != data.actions.Count)
        {
            // Incorrect, print error and use default command
            Debug.LogError("Incorrect number of commands assigned." + commands.Count + " " + this.commands.Count);
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
        for (int i = DelayedActions.Count - 1; i >= 0; i--)
        {
            QueuedAction action = DelayedActions[i];
            if (action.updateRemainingTime(Time.deltaTime))
                DelayedActions.Remove(action);
        }
    }
    
    private void NoCommand()
    {
        Debug.LogError("No command assigned to this action.");
    }

    public void Enqueue(ActionData action, Command command)
    {
        DelayedActions.Add(new QueuedAction(command, action));
    }
}
