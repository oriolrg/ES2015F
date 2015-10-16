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

    protected bool inConstruction; //Indicates if a building is in construction or if a unit is constructing a building

    protected bool construct; //Indicates if a unit has the order to construct a building

    private int maxQueueLength = 5;

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

    protected abstract List<Command> defineCommands();

    void Start()
    {
        inConstruction = false;
        construct = false;
    }


    void FixedUpdate()
    {
        if (Queue.Count > 0)
        {
            Action currentAction = Queue.Peek();
            currentAction.updateRemainingTime(Time.deltaTime);

            if (currentAction.isDone)
            {
                Queue.Dequeue();
                currentAction.execute();
            }
        }
    }
    
    private void NoCommand()
    {
        Debug.LogError("No command assigned to this action.");
    }

    public void ActionClicked(ActionData actionData)
    {
        Command correspondingCommand = commands[data.actions.IndexOf(actionData)];
        Action action = new Action(actionData, correspondingCommand);

        if (actionData.requiredTime == 0)

            action.execute();

        else
        {
            if (Queue.Count >= maxQueueLength)
                Debug.LogWarning("Queue maximum length reached.");
            else
                Queue.Enqueue(action);
        }

            
    }

    public List<ActionData> getActionDatas() { return data.actions; }


    public void SetInConstruction(bool b)
    {
        inConstruction = b; Debug.Log(b);
    }

    public void setConstruct(bool b)
    {
        construct = b;
    }

    public bool getConstruct()
    {
        return construct;
    }

    public bool getInConstruction()
    {
        return inConstruction;
    }

}
