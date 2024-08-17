using UnityEngine;

public class TopActionStatus : MonoBehaviour
{
    public Action PlayerAction = Action.TopIdle;
    [SerializeField]
    private Animator _playerAnimator;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        switch (PlayerAction)
        {
            case Action.BotJumpIdle:
                {
                    _playerAnimator.SetBool("IsJump", true);
                    _playerAnimator.SetBool("IsWalk", false);
                    _playerAnimator.SetBool("IsIdle", false);
                    break;
                }
            case Action.BotJumpRight:
                {
                    _playerAnimator.SetBool("IsJump", true);
                    _playerAnimator.SetBool("IsWalk", true);
                    _playerAnimator.SetBool("IsIdle", false);
                    break;
                }
            case Action.BotMoveRight:
                {
                    _playerAnimator.SetBool("IsWalk", true);
                    _playerAnimator.SetBool("IsIdle", false);
                    _playerAnimator.SetBool("IsJump", false);
                    break;
                }
            case Action.BotIdle:
                {
                    _playerAnimator.SetBool("IsIdle", true);
                    _playerAnimator.SetBool("IsWalk", false);
                    _playerAnimator.SetBool("IsJump", false);
                    break;
                }
        }

    }
}