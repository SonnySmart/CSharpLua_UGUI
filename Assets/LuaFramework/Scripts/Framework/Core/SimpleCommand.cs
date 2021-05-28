using System;

/// <summary>
/// 事件命令
/// </summary>
public class SimpleCommand : ICommand
{
    public virtual void Execute(IMessage message) {
    }
}