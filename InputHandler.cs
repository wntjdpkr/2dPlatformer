using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Dictionary<KeyCode, ICommand> commandMap;

    private void Start()
    {
        var player = FindFirstObjectByType<PlayerController>();
        commandMap = new Dictionary<KeyCode, ICommand> {
            { KeyCode.A, new MoveLeftCommand(player) },
            { KeyCode.LeftArrow, new MoveLeftCommand(player) },
            { KeyCode.D, new MoveRightCommand(player) },
            { KeyCode.RightArrow, new MoveRightCommand(player) },
            { KeyCode.Space, new JumpCommand(player) }
        };
    }

    private void Update()
    {
        foreach (var entry in commandMap)
        {
            if (Input.GetKey(entry.Key))
            {
                entry.Value.Execute();
            }
        }
    }
}
