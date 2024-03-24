using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum InputKey
{
    W = 1 << 0,
    A = 1 << 1,
    S = 1 << 2,
    D = 1 << 3,
    Attack = 1 << 4,
}

public interface IInput
{
    bool GetKey(InputKey key);
    void ConsumeFrame();
}

public class RecordInput : IInput
{
    readonly List<InputKey> actions = new();
    int currentFrame = 0;

    public RecordInput(List<InputKey> actions)
    {
        this.actions = actions;
    }

    public bool GetKey(InputKey key)
    {
        // assert key only contains one bit
        Assert.IsTrue(((int)key & ((int)key - 1)) == 0);
        return currentFrame < actions.Count && (actions[currentFrame] & key) != 0;
    }

    public void ConsumeFrame()
    {
        currentFrame++;
    }
}

public class ActualInput : IInput
{
    public bool GetKey(InputKey key)
    {
        // assert key only contains one bit
        Assert.IsTrue(((int)key & ((int)key - 1)) == 0);
        switch (key)
        {
            case InputKey.W:
                return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.JoystickButton0);
            case InputKey.A:
                return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") == 1;
            case InputKey.S:
                return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            case InputKey.D:
                return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") == -1;
            case InputKey.Attack:
                return Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.JoystickButton1);
            default:
                return false;
        }
    }

    public void ConsumeFrame()
    {
    }
}
