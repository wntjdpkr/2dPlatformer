using UnityEngine;

public class Command
{
    
}
public class MoveLeftCommand : ICommand
{
    private PlayerController player;
    public MoveLeftCommand(PlayerController p) => player = p;
    public void Execute() => player.MoveLeft();
}

public class MoveRightCommand : ICommand
{
    private PlayerController player;
    public MoveRightCommand(PlayerController p) => player = p;
    public void Execute() => player.MoveRight();
}

public class JumpCommand : ICommand
{
    private PlayerController player;
    public JumpCommand(PlayerController p) => player = p;
    public void Execute() => player.Jump();
}