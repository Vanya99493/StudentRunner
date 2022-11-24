using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public event System.Action CollisionWithGround;
    public event System.Action CollisionWithEnemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Ground>() != null)
        {
            CollisionWithGround?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyController>() != null)
        {
            CollisionWithEnemy?.Invoke();
        }
    }
}