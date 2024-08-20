using UnityEngine;
using System.Collections;
public class EnemyShooterStationary : Enemy
{
    [SerializeField]
    private Color _bulletColor;
    [SerializeField]
    private EnemyAction _enemyAction = EnemyAction.Idle;
    [SerializeField]
    private float _enemyHealth = 100.0f;
    [SerializeField]
    private float _movementSpeed = 10.0f;
    [SerializeField]
    private float _shootRange = 10.0f;
    [SerializeField]
    Vector3 shootPosition;
    private Coroutine attackCoroutine;
    private Coroutine hitCoroutine;
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

    public EnemyShooterStationary(Transform currentObj) : base(currentObj)
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
        float differenceX = enemyObj.position.x - targetObj.position.x;
        float distance = (enemyObj.position - targetObj.position).magnitude;
        float followDistance = 3.0f;
        float attackDistance = 10.0f;
        switch (_enemyAction)
        {
            case EnemyAction.Idle:
                {
                    if (distance < _shootRange && enemyStatus)
                    {
                        _enemyAction = EnemyAction.Attack;
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
            case EnemyAction.Hit:
                {
                    if (hitCoroutine == null && enemyStatus)
                    {
                        hitCoroutine = StartCoroutine(HitDelay(0.5f));
                    }
                    break;
                }
            case EnemyAction.Attack:
                {
                    if (attackCoroutine == null && enemyStatus)
                    {
                        float randomShootTime = Random.Range(0.3f, 2.5f);
                        attackCoroutine = StartCoroutine(AttackDelay(randomShootTime, differenceX > 0));
                    }
                    break;
                }
           
        }
        //Animation State!
        ExecuteAction(targetObj, _enemyAction, deltaTime);
    }

    private IEnumerator AttackDelay(float delaySecond, bool flip)
    {

        Direction direction = Direction.Right;
        Vector2 projectilePosition = Vector2.zero;
        if (flip) //right
        {
            projectilePosition = (Vector2)transform.position + new Vector2(-shootPosition.x, shootPosition.y);
            direction = Direction.Left;
        }
        else
        {
            projectilePosition = (Vector2)transform.position + new Vector2(shootPosition.x, shootPosition.y);
            direction = Direction.Right;
        }
        GameObject bullet = ObjectPool.Instance.GetObjectPool();
        if (bullet != null)
        {
            enemyAnimator.SetBool("IsWalk", false);
            enemyAnimator.SetTrigger("IsAttack");
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.targetName = "Player";
            projectile.BulletDirection = direction;
            projectile.BulletActiveTime = 2.0f;
            projectile.SpriteRenderer.color = _bulletColor;
            projectile.transform.position = projectilePosition;
            projectile.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        _enemyAction = EnemyAction.Idle;
    }
    private IEnumerator HitDelay(float delaySecond)
    {
        enemyAnimator.SetBool("IsWalk", false);
        enemyAnimator.SetTrigger("IsHit");
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(hitCoroutine);
        hitCoroutine = null;
        _enemyAction = EnemyAction.Idle;
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
        DrawHelper.DrawRaySphere(Vector2.up, 0, _shootRange, Color.red);
        DrawHelper.DrawRaySphere(Vector2.up, 0.0f, playerDetection, Color.green);
        DrawHelper.DrawRaySphere(Vector2.right, shootPosition.x, 0.5f, Color.green);
        //        DrawHelper.DrawCapsule(capsuleCollider.size.y, capsuleCollider.size.x);
    }
}