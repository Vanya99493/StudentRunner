using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCollisionController))]
public class PlayerController : MonoBehaviour
{
    public event System.Action OnDie;

    private PlayerCollisionController _playerCollisionController;
    private Animator _playerAnimator;
    private Rigidbody2D _playerRigidbody;
    private PlayerState _playerState;
    private float _jumpForce;
    private float _crouchingTime;
    private bool _isOnGround;

    public PlayerState PlayerState { get { return _playerState; } }

    public void Initialize(float jumpForce, float crouchingTime)
    {
        _playerCollisionController = GetComponent<PlayerCollisionController>();
        _playerAnimator = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody2D>();

        _jumpForce = jumpForce;
        _crouchingTime = crouchingTime;

        _playerCollisionController.CollisionWithGround += () =>
        {
            _isOnGround = true;
            if(_playerState != PlayerState.Idle && _playerState != PlayerState.Die)
            {
                SetState(PlayerState.Run);
            }
        };
        _playerCollisionController.CollisionWithEnemy += () => SetState(PlayerState.Die);

        SetState(PlayerState.Idle);
    }

    public void SetState(PlayerState newState)
    {
        _playerState = newState;

        switch (_playerState)
        {
            case PlayerState.Idle:
                ResetAnimatorSettings();
                _playerAnimator.SetFloat("speed_f", 0f);
                break;
            case PlayerState.Run:
                ResetAnimatorSettings();
                _playerAnimator.SetFloat("speed_f", 1f);
                break;
            case PlayerState.Jump:
                ResetAnimatorSettings();
                Jump();
                _playerAnimator.SetBool("isJump_b", true);
                break;
            case PlayerState.Crouch:
                ResetAnimatorSettings();
                _playerAnimator.SetBool("isCrouch_b", true);
                StartCoroutine(CrouchingCoroutine());
                break;
            case PlayerState.Die:
                ResetAnimatorSettings();
                _playerAnimator.SetTrigger("die_t");
                _playerAnimator.SetFloat("speed", 0f);
                OnDie?.Invoke();
                break;
            case PlayerState.WakeUp:
                ResetAnimatorSettings();
                _playerAnimator.SetTrigger("wakeUp_t");
                SetState(PlayerState.Idle);
                break;
            default:
                break;
        }
    }

    private void ResetAnimatorSettings()
    {
        _playerAnimator.SetBool("isJump_b", false);
        _playerAnimator.SetBool("isCrouch_b", false);
    }

    private void Jump()
    {
        if (!_isOnGround)
        {
            return;
        }
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, _jumpForce);
        _isOnGround = false;
    }

    private IEnumerator CrouchingCoroutine()
    {
        yield return new WaitForSeconds(_crouchingTime);
        if(_playerState == PlayerState.Crouch)
        {
            SetState(PlayerState.Run);
        }
    }
}