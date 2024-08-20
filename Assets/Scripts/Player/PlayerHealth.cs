using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float CurrentHealth = 100.0f;
    private FullActionStatus _fullActionStatus;
    private TopActionStatus _topActionStatus;
    private BottomActionStatus _botActionStatus;
    private PlayerController _playerController;

    public bool isDead;
    void Awake()
    {
        _playerController = GetComponentInChildren<PlayerController>();
        _fullActionStatus = GetComponentInChildren<FullActionStatus>();
        _botActionStatus = GetComponentInChildren<BottomActionStatus>();
        _topActionStatus = GetComponentInChildren<TopActionStatus>();

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            Projectile projectile = other.GetComponent<Projectile>();
            CheckPlayerDeath();
            float damageAmount = Random.Range(15.0f, 20.0f);
            if (other.CompareTag("Bullet") && projectile.targetName == "Player")
            {
                if (_playerController.HitCoroutine == null)
                {
                    CurrentHealth -= damageAmount;
                    StartCoroutine(_playerController.DelayHit(0.5f));
                }
                other.transform.gameObject.SetActive(false);
            }
        }
    }
    public void TakeDamage(float damageAmount)
    {
        CheckPlayerDeath();
        if (_playerController.HitCoroutine == null)
        {
            CurrentHealth -= damageAmount;
            StartCoroutine(_playerController.DelayHit(0.5f));
        }
    }

    public void CheckPlayerDeath()
    {
        if (CurrentHealth < 0)
        {
            _topActionStatus.gameObject.SetActive(false);
            _botActionStatus.gameObject.SetActive(false);
            _fullActionStatus.gameObject.SetActive(true);
            _fullActionStatus.PlayerAction = Action.FullDeath;
            //play death screen animation here
            UISpriteAnimation sprite = FindObjectOfType<UISpriteAnimation>(true);
            if (!sprite.gameObject.activeSelf && sprite != null)
            {
                sprite.gameObject.SetActive(true);
            }
            isDead = true;
            return;
        }
    }
}