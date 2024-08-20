using UnityEngine;

public class FullActionStatus : MonoBehaviour
{
    public Action PlayerAction = Action.FullCrouch;
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
            case Action.FullCrouch:
                {
                    _playerAnimator.SetBool("IsCrouch", true);
                    _playerAnimator.SetBool("IsWalk", false);
                    break;
                }
            case Action.FullCrouchMoveRight:
                {
                    _playerAnimator.SetBool("IsCrouch", true);
                    _playerAnimator.SetBool("IsWalk", true); ;
                    break;
                }
            case Action.FullCrouchShoot:
                {
                    _playerAnimator.SetTrigger("ShootRight");
                    break;
                }
                case Action.FullDeath:{
                    Vector2 origin = (Vector2)transform.localPosition;
                    Vector2 newPosition = new Vector2(0,0.73f);
                    origin.y = newPosition.y;
                    transform.localPosition =  origin;
                    break;
                }
        }

    }
}