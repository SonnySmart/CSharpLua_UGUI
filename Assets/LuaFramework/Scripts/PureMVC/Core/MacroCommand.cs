using System;
using System.Collections.Generic;

public class MacroCommand : ICommand
{
    private IList<Type> m_subCommands = new List<Type>();

    public MacroCommand()
    {
        this.InitializeMacroCommand();
    }

    protected void AddSubCommand(Type commandType)
    {
        this.m_subCommands.Add(commandType);
    }

    public void Execute(IMessage message)
    {
        while (this.m_subCommands.Count > 0)
        {
            Type type = this.m_subCommands[0];
            object instance = Activator.CreateInstance(type);
            if (instance is ICommand)
            {
                ((ICommand)instance).Execute(message);
            }
            this.m_subCommands.RemoveAt(0);
        }
    }

    protected virtual void InitializeMacroCommand()
    {
    }
}