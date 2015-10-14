
using System;
using System.Collections.Generic;

public class Civil : Unit
{
    protected override List<Command> defineCommands()
    {
        return new List<Command>() { createWonder, sacrifice };
    }

    private void createWonder()
    {
        print("wonder");
    }
    private void sacrifice() { print("sacrifice"); GameController.Instance.ClearSelection(); }
}