
using System;
using System.Collections.Generic;

public class Civil : Unit
{
    protected override List<Command> defineCommands()
    {
        return new List<Command>() { createWonder, sacrifice };
    }

    private void createWonder() { }
    private void sacrifice() { }
}