using UnityEngine;

public class TilesMover : MonoBehaviour, IMovable
{
    private float _xPositionToRepeatTiles;
    private int _directionLever;

    public void Initialize(float xPositionToRepeatTiles, bool directionToRight)
    {
        _xPositionToRepeatTiles = Mathf.Abs(xPositionToRepeatTiles);
        _directionLever = directionToRight ? 1 : -1;
    }

    public void Move(float speed, float deltaTime)
    {
        transform.position = new Vector3(
            Mathf.Repeat(Mathf.Abs(transform.position.x) + (deltaTime * speed), _xPositionToRepeatTiles) * _directionLever,
            transform.position.y,
            transform.position.z
            );
    }
}