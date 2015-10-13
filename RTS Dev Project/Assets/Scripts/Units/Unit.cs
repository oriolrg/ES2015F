using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private UnitData data;
    public Queue<QueuedAction> DelayedActions { get; private set; }
    private List<Command> commands;

    [SerializeField] private float health;

    public float HealthRatio { get { return health * 1f / data.stats[Stat.Health]; }}
    public Sprite Preview { get { return data.preview; } }

    protected abstract List<Command> defineCommands();

    void Awake()
    {
        health = data.stats[Stat.Health];
        
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
    
    private void NoCommand()
    {
        Debug.LogError("No command assigned to this action.");
    }
    
    public UnitData getData() { return data; }
    public List<Command> getCommands() { return commands; }

}
