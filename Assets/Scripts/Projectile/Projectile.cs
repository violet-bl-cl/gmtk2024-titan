using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.0f, 100.0f)]
    private float _destroyDistance;
    public Direction BulletDirection;
    [Range(0.0f, 100.0f)]
    public float ProjectileSpeed;
    public float BulletActiveTime;
    public Vector2 diffDistance;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start(){
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
    }
    void OnEnable()
    {
        Invoke(nameof(DeactivateCurrentObject), BulletActiveTime);
    }
    void OnDisable()
    {
        CancelInvoke(nameof(DeactivateCurrentObject));
    }
    // Update is called once per frame
    void Update()
    {
        switch (BulletDirection)
        {
            case Direction.Left:
                {
                    transform.position += -transform.right * Time.deltaTime * ProjectileSpeed;
                    _spriteRenderer.transform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
                    break;
                }
            case Direction.Right:
                {
                    transform.position += transform.right * Time.deltaTime * ProjectileSpeed;
                    _spriteRenderer.transform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
                    break;
                }
            case Direction.Up:
                {
                    transform.position += transform.up * Time.deltaTime * ProjectileSpeed;
                      _spriteRenderer.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
                    break;
                }
            case Direction.Down:
                {
                    transform.position += -transform.up * Time.deltaTime * ProjectileSpeed;
                     _spriteRenderer.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    private void DeactivateCurrentObject()
    {
        gameObject.SetActive(false);
    }

}
