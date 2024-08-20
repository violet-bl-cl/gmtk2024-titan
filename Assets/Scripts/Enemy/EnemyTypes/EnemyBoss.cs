using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class EnemyBoss : Enemy
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
    public EnemyBoss(Transform currentObj) : base(currentObj)
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
        float attackDistance = 13.8f;
        switch (_enemyAction)
        {
            case EnemyAction.Idle:
                {
                    if (distance < attackDistance && enemyStatus)
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
                        hitCoroutine = StartCoroutine(HitDelay(0.1f));
                    }
                    break;
                }
            case EnemyAction.Attack:
                {
                    enemyAnimator.SetBool("IsAttack", true);
                    if (attackCoroutine == null && enemyStatus)
                    {
                        float randomShootTime = Random.Range(0.3f, 0.8f);
                        attackCoroutine = StartCoroutine(AttackDelay(randomShootTime, differenceX > 0));
                    }
                    break;
                }
            case EnemyAction.MoveToPlayer:
                {

                    break;
                }
            case EnemyAction.Death:
                {
                    enemyAnimator.SetBool("IsAttack", false);
                    gameObject.SetActive(false);
                    break;
                }
        }
        //Animation State!
        ExecuteAction(targetObj, _enemyAction, deltaTime);
    }

    private IEnumerator AttackDelay(float delaySecond, bool flip)
    {

        Direction direction = Direction.Right;
        Vector2 projectilePositionLeft = transform.localPosition;
        Vector2 projectilePositionRight = transform.localPosition;

        if (flip) //right
        {
            projectilePositionLeft.x = projectilePositionLeft.x - 2.0f;
            projectilePositionLeft.y = projectilePositionLeft.y - 2.0f;
            direction = Direction.Left;
        }
        else
        {
            projectilePositionRight.x = projectilePositionRight.x + 2.0f;
            projectilePositionRight.y = projectilePositionRight.y - 2.1f;
            direction = Direction.Right;
        }
        Shoot(direction, projectilePositionLeft);
        Shoot(direction, projectilePositionRight);
        yield return new WaitForSeconds(delaySecond);
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
    }
    private void Shoot(Direction direction, Vector3 projectilePosition)
    {
        GameObject bullet = ObjectPool.Instance.GetObjectPool();

        if (bullet != null)
        {
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.BulletDirection = direction;
            projectile.BulletActiveTime = 2.0f;
            projectile.SpriteRenderer.color = _bulletColor;
            projectile.transform.position = projectilePosition;
            projectile.targetName = "Player";
            projectile.gameObject.SetActive(true);
        }
    }
    private IEnumerator HitDelay(float delaySecond)
    {
        // enemyAnimator.SetBool("IsAttack", true);
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
            if (other.CompareTag("Bullet") && projectile.targetName =="Enemy")
            {
                //_enemyAction = EnemyAction.Hit;
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
    }
}