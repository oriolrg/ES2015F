using UnityEngine;

public class Action
{
    protected Command command;
    protected Sprite sprite;

    public Action( Command command, Sprite sprite )
    {
        this.command = command;
        this.sprite = sprite;
    }

    public void execute()
    {
        command();
    }
}