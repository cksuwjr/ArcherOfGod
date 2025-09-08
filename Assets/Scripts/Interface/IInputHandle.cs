using UnityEngine;

public interface IInputHandle
{
    public Vector3 GetInput();

    public bool GetKeyInput(KeyCode input);
}
