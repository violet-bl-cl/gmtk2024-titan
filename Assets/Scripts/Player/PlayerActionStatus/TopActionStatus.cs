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
            case Action.TopIdle:
                {

                    _playerAnimator.SetBool("IsWalk", false);
                    _playerAnimator.SetBool("IsJump", false);
                    _playerAnimator.SetBool("IsDown", false);
                    _playerAnimator.SetBool("IsUp", false);
                    break;
                }
            case Action.TopAimUp:
                {
                    _playerAnimator.SetBool("IsUp", true);
                    _playerAnimator.SetBool("IsDown", false);
                    _playerAnimator.SetBool("IsWalk", false);
                    _playerAnimator.SetBool("IsJump", false);
                    break;
                }
            case Action.TopAimDown:
                {

                    _playerAnimator.SetBool("IsDown", true);
                    _playerAnimator.SetBool("IsWalk", false);
                    _playerAnimator.SetBool("IsJump", false);
                    _playerAnimator.SetBool("IsUp", false);
                    break;
                }
            case Action.TopMoveRight:
                {
                    _playerAnimator.SetBool("IsWalk", true);
                    _playerAnimator.SetBool("IsJump", false);
                    _playerAnimator.SetBool("IsDown", false);
                    _playerAnimator.SetBool("IsUp", false);
                    break;
                }
            case Action.TopJumpRight:
                {
                    _playerAnimator.SetBool("IsJump", true);
                    _playerAnimator.SetBool("IsWalk", false);
                    _playerAnimator.SetBool("IsJump", false);
                    break;
                }
        }

    }
}