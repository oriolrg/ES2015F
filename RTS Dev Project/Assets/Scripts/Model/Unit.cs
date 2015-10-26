using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected UnitData data;
    private List<Command> commands;
    public Queue<Action> Queue { get; private set; }

    [SerializeField]
    protected ResourceTextDictionary resourceCosts;


    [SerializeField] protected float health;
    [SerializeField] public string Name { get; protected set; }
    public float HealthRatio { get { return health * 1f / data.stats[Stat.Health]; }}
    public Sprite Preview { get { return data.preview; } }

    protected bool inConstruction; //Indicates if a building is being constructed or if a unit is constructing a building

    protected bool construct; //Indicates if a unit has the order to construct a building
    protected bool constructionOnGoing; //Indicates if a building construction is on going

    protected GameObject buildingToConstruct;

    private int maxQueueLength = 5;

    protected List<string> greekNames = new List<string>() { "Agapetus", "Anacletus", "Eustathius", "Helene", "Herodes", "Isidora", "Kosmas", "Lysimachus", "Lysistrata", "Nereus", "Niketas", "Theodoro", "Zephyros" };
    protected List<string> greekAdjectives = new List<string>() { "Important", "Lazy", "Popular", "Historical", "Scared", "Old", "Traditional", "Strong", "Helpful", "Competitive", "Legal", "Obvious" };

    void Awake()
    {
        if (data == null) Debug.LogError("data asset missing on " + name+".");
        health = data.stats[Stat.Health];
        name = string.Format("{0}, The {1}", greekNames[Random.Range(0,greekNames.Count)], greekAdjectives[Random.Range(0,greekAdjectives.Count)]);

        Queue = new Queue<Action>();
        
        // Get the commands set in the subclass
        List<Command> list = defineCommands();

        // Check correct number of commands set
        if (list.Count != data.actions.Count)
        {
            // Incorrect, print error and use default command
            Debug.LogError("Incorrect number of commands assigned: " + list.Count + " given vs " + data.actions.Count + " required.");
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
        constructionOnGoing = false;
    }


    void FixedUpdate()
    {
        if (Queue.Count > 0)
        {
            Action currentAction = Queue.Peek();
            currentAction.updateRemainingTime(Time.deltaTime);

            if (currentAction.isDone)
            {
                currentAction.execute();
                Queue.Dequeue();
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

        if (actionData.requiredTime == 0) action.execute();

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
        inConstruction = b;
        Debug.Log(b);
    }

    public void setConstruct(bool b)
    {
        construct = b;
    }

    public void setConstructionOnGoing(bool b)
    {
        constructionOnGoing = b;
    }

    public bool getConstruct()
    {
        return construct;
    }

    public bool getInConstruction()
    {
        return inConstruction;
    }

    public bool getConstructionOnGoing()
    {
        return constructionOnGoing;
    }

    public void SetBuildingToConstruct(GameObject b)
    {
        buildingToConstruct = b;
    }

    public GameObject getBuildingToConstruct()
    {
        return buildingToConstruct;
    }

}
