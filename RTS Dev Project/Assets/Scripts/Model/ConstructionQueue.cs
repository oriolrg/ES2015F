
using System.Collections.Generic;
using UnityEngine;

public class ConstructionQueue : MonoBehaviour
{
    public Queue<Action> Queue { get; private set; }

    private int maxQueueLength = 5;

    void Awake()
    {
        Queue = new Queue<Action>();
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

    public void Enqueue(Action action)
    {
        if (Queue.Count >= maxQueueLength)
            Debug.LogWarning("Queue maximum length reached.");
        else
            Queue.Enqueue(action);
    }
}