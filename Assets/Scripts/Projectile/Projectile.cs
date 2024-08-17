using UnityEngine;
[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.0f, 100.0f)]
    private float _destroyDistance;
    public Direction BulletDirection;
    [Range(0.0f, 100.0f)]
    public float ProjectileSpeed;
    public Vector2 diffDistance; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (BulletDirection)
        {
            case Direction.Left:
                {
                    transform.position +=  -transform.right * Time.deltaTime * ProjectileSpeed;
                    break;
                }
            case Direction.Right:
                {
                    transform.position += transform.right * Time.deltaTime * ProjectileSpeed;   
                    break;
                }
            case Direction.Up:
                {
                    transform.position += transform.up * Time.deltaTime * ProjectileSpeed;   
                    break;
                }
            case Direction.Down:
                {
                    transform.position += -transform.up * Time.deltaTime * ProjectileSpeed;   
                    break;
                }
            default:
                {
                    break;
                }
        }
        Vector2 origin = (Vector2)transform.position;
        Vector2 distance = new Vector2(_destroyDistance, _destroyDistance);

        if (CompareDistance(origin, distance * origin))
        {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    bool CompareDistance(Vector2 origin, Vector2 distance)
    {
        return (Mathf.Abs(origin.x) > (Mathf.Abs(distance.x)) || (Mathf.Abs(origin.y) > Mathf.Abs(distance.y)));
    }
}
