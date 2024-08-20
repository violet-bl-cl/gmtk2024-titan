
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform enemyObj;
    protected SpriteRenderer enemySpriteRenderer;
    protected Animator enemyAnimator;
    protected CapsuleCollider2D capsuleCollider;
    protected Rigidbody2D enemyRigidBody;
    // ??? why are they 0 from the beginning ?
    [SerializeField]
    private float _walkSpeed = 10.0f;
    [SerializeField]
    private float _runSpeed = 1.0f;
    [SerializeField]
    private float _attackSpeed = 5.0f;

    public Enemy(Transform currentObj)
    {
        this.enemyObj = currentObj;
    }
    public virtual void UpdateAction(Transform targetObj, float deltaTime)
    {

    }

    //this is animation status.
    protected void ExecuteAction(Transform targetObj, EnemyAction enemyAction, float deltaTime)
    {
        float differenceX = enemyObj.position.x - targetObj.position.x;
        float walkSpeed = 3.0f;
        float runSpeed = 4.5f;
        float attackSpeed = 5.0f;
        if (enemyAction != EnemyAction.Death)
        {
            enemySpriteRenderer.flipX = differenceX > 0;
        }
        switch (enemyAction)
        {
            case EnemyAction.Blink:{
                break;
            }
            case EnemyAction.Flee:
                {
                    break;
                }
            case EnemyAction.Attack:
                {
                    enemyAnimator.SetBool("IsWalk", false);
                    break;
                }
            case EnemyAction.MoveToPlayer:
                {
                    float direction = (differenceX > 0) ? -1 : 1;
                    enemyObj.transform.position += enemyObj.right * direction * walkSpeed * Time.deltaTime;
                    enemyAnimator.SetBool("IsWalk", true);
                    break;
                }
            case EnemyAction.Stroll:
                {
                    break;
                }
            case EnemyAction.Idle:
                {
                    enemyAnimator.SetBool("IsWalk", false);
                    break;
                }
            case EnemyAction.Death:
                {
                    enemyRigidBody.velocity = Vector3.zero;
                    enemyRigidBody.isKinematic = true;
                    capsuleCollider.enabled = false;
                    enemyAnimator.SetBool("IsWalk", false);
                    enemyAnimator.SetBool("IsDead", true);
                    break;
                }
        }
    }
}