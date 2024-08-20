using UnityEngine;
using System.Collections;
public class EnemyMelee : Enemy
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
    private Coroutine hitCoroutine;
    private Coroutine blinkCoroutine;
    private bool isPlayerDetected = false;
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField, Range(0.0f, 100.0f)]
    private float playerDetection = 0.0f;
    void Awake()
    {
        enemyObj = transform;
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }
   
    public EnemyMelee(Transform currentObj) : base(currentObj)
    {
    }

    public override void UpdateAction(Transform targetObj, float deltaTime)
    {

        bool isDead = _enemyHealth < 0.0f;
        if (isDead)
        {
            _enemyAction = EnemyAction.Death;
        }
        if (RaycastHelper.CheckCircleSide(transform.position, Vector2.up, playerDetection, 0.0f, targetMask) && !isPlayerDetected)
        {
            isPlayerDetected = true;
        }
        bool enemyStatus = !isDead && isPlayerDetected;
        float distance = (enemyObj.position - targetObj.position).magnitude;
        float followDistance = 3.0f;
        float attackDistance = 4.0f;
        switch (_enemyAction)
        {
            case EnemyAction.Idle:
                {
                    if (distance > followDistance && enemyStatus)
                    {
                        _enemyAction = EnemyAction.MoveToPlayer;
                    }
                    else if (distance < attackDistance && enemyStatus)
                    {
                        _enemyAction = EnemyAction.Attack;
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
            case EnemyAction.Blink:
                {
                    break;
                }
            case EnemyAction.Stroll:
                {

                    break;
                }
            case EnemyAction.Hit:
                {
                    if (hitCoroutine == null && enemyStatus)
                    {
                        hitCoroutine = StartCoroutine(HitDelay(0.1f));
                    }
                    break;
                }
            case EnemyAction.Attack:
                {
                    if (attackCoroutine == null && enemyStatus)
                    {
                        float randomTime = Random.Range(0.6f, 1.3f);
                        attackCoroutine = StartCoroutine(AttackDelay(randomTime));
                    }
                    break;
                }
            case EnemyAction.MoveToPlayer:
                {
                    if (distance < attackDistance && enemyStatus)
                    {
                        _enemyAction = EnemyAction.Attack;
                    }
                    else if (distance > followDistance && enemyStatus)
                    {
                        //remove physics here
                        if (blinkCoroutine == null)
                        {
                            float randomTime = Random.Range(2f, 3f);
                            float randomX = Random.Range(-7.0f, 7.0f);
                            Vector2 origin = transform.localPosition;
                            origin.x = randomX + targetObj.transform.localPosition.x;
                            transform.localPosition = origin;
                            blinkCoroutine = StartCoroutine(BlinkDelay(randomTime));
                        }
                    }
                    else
                    {
                        _enemyAction = EnemyAction.Idle;
                    }
                    break;
                }
        }
        //Animation State!
        ExecuteAction(targetObj, _enemyAction, deltaTime);
    }
    private IEnumerator BlinkDelay(float delaySecond)
    {
        _enemyAction = EnemyAction.MoveToPlayer;
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
        _enemyAction = EnemyAction.Idle;
    }
    private IEnumerator AttackDelay(float delaySecond)
    {
        enemyAnimator.SetBool("IsWalk", false);
        enemyAnimator.SetTrigger("IsAttack");
        RaycastHit2D hitInfo = RaycastHelper.GetCircleHit(transform.position, Vector3.up, 4.0f, 0f, targetMask);
        if (hitInfo.collider != null)
        {
            PlayerHealth playerStatus = hitInfo.collider.GetComponent<PlayerHealth>();
            float damage = Random.Range(10.0f, 20.0f);
            playerStatus.TakeDamage(damage);
        }
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        _enemyAction = EnemyAction.MoveToPlayer;
    }
    private IEnumerator HitDelay(float delaySecond)
    {
        enemyAnimator.SetBool("IsWalk", false);
        enemyAnimator.SetTrigger("IsHit");
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(hitCoroutine);
        hitCoroutine = null;
        _enemyAction = EnemyAction.MoveToPlayer;
    }
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (other.CompareTag("Bullet") && projectile.targetName == "Enemy")
            {
                _enemyAction = EnemyAction.Hit;
                _enemyHealth -= 20.0f;
                other.transform.gameObject.SetActive(false);
            }
        }
    }
    void OnDrawGizmos()
    {
        DrawHelper.SetTransform(transform);
        DrawHelper.DrawRaySphere(Vector2.up, 0, 4.0f, Color.red);
        DrawHelper.DrawRaySphere(Vector2.up, 0.0f, playerDetection, Color.green);
    }
}