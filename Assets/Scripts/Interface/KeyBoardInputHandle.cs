using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInputHandle : MonoBehaviour, IInputHandle
{
    public Vector3 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
    }

    public bool GetKeyInput(KeyCode input)
    {
        throw new System.NotImplementedException();
    }
}
