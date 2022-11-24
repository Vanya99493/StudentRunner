using UnityEngine;

public class EnemyController : MonoBehaviour, IMovable
{
    private int _directionLever;

    public void Initialize(bool directionToRight)
    {
        _directionLever = directionToRight ? 1 : -1;
    }

    public void Move(float speed, float deltaTime)
    {
        transform.position += new Vector3(
            deltaTime * speed * _directionLever,
            0,
            0
            );
    }
}