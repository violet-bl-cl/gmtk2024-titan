using UnityEngine;

public class HitDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.transform.gameObject.SetActive(false);
            print($"{other.transform.name}");
        }
    }
}
