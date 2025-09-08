using UnityEngine;
public interface IMove
{
    public void Init();
    public void Move(Vector3 direction);
    public bool IsMove { get; }

    public void Stop();
}
