using UnityEngine;
using System.Collections;
public class EnemyZombie : Enemy
{
    [SerializeField]
    private EnemyAction _enemyAction = EnemyAction.Idle;
    [SerializeField]
    private float _enemyHealth = 100.0f;
    [SerializeField]
    private float _movementSpeed = 10.0f;
    [SerializeField]
    private float _shootRange = 10.0f;
    private Coroutine attackCoroutine;
    private bool isAttack;
    private Coroutine hitCoroutine;
    private bool isHit;
    void Awake()
    {
        enemyObj = transform;
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            _enemyAction = EnemyAction.Hit;
            _enemyHealth -= 20.0f;
            other.transform.gameObject.SetActive(false);
            print($"{other.transform.name}");
        }
    }
    public EnemyZombie(Transform currentObj) : base(currentObj)
    {
    }

    public override void UpdateAction(Transform targetObj, float deltaTime)
    {
        bool isDead = _enemyHealth < 0.0f;
        if (isDead)
        {
            _enemyAction = EnemyAction.Death;
        }
        float distance = (enemyObj.position - targetObj.position).magnitude;
        float followDistance = 5.0f;
        float attackDistance = 2.0f;
        switch (_enemyAction)
        {
            case EnemyAction.Idle:
                {
                    if (distance > followDistance && !isDead)
                    {
                        _enemyAction = EnemyAction.MoveToPlayer;
                    }
                    else
                    {
                        _enemyAction = EnemyAction.Idle;
                    }
                    break;
                }
            case EnemyAction.Flee:
                {

                    break;
                }
            case EnemyAction.Stroll:
                {

                    break;
                }
            case EnemyAction.Hit:{
                    if(hitCoroutine == null && !isDead){
                        hitCoroutine  = StartCoroutine(HitDelay(0.3f));
                    }
                break;
            }
            case EnemyAction.Attack:
                {
                    if (attackCoroutine == null && !isDead)
                    {
                        attackCoroutine = StartCoroutine(AttackDelay(1.0f));
                    }
                    break;
                }
            case EnemyAction.MoveToPlayer:
                {
                    if (distance > followDistance && !isDead)
                    {
                        _enemyAction = EnemyAction.MoveToPlayer;
                    }
                    else
                    {
                        _enemyAction = EnemyAction.Attack;
                    }
                    break;
                }
        }
        //Animation State!
        ExecuteAction(targetObj, _enemyAction, deltaTime);
    }

    private IEnumerator AttackDelay(float delaySecond)
    {
        isAttack = true;
        enemyAnimator.SetBool("IsWalk", false);
        enemyAnimator.SetTrigger("IsAttack");
        yield return new WaitForSeconds(delaySecond);
        isAttack = false;
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        _enemyAction = EnemyAction.MoveToPlayer;
    }
    private IEnumerator HitDelay(float delaySecond){

        enemyAnimator.SetBool("IsWalk", false);
        enemyAnimator.SetTrigger("IsHit");
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(hitCoroutine);
        hitCoroutine = null;
        _enemyAction = EnemyAction.MoveToPlayer;
    }
}