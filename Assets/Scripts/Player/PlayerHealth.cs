using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float CurrentHealth = 100.0f;
    private FullActionStatus _fullActionStatus;
    private TopActionStatus _topActionStatus;
    private BottomActionStatus _botActionStatus;
    public bool isDead;
    void Awake()
    {
        _fullActionStatus = GetComponentInChildren<FullActionStatus>();
        _botActionStatus = GetComponentInChildren<BottomActionStatus>();
        _topActionStatus = GetComponentInChildren<TopActionStatus>();

    }
    void Update()
    {
    }
    public void TakeDamage(float damageAmount)
    {   
        if(CurrentHealth < 0){
            _topActionStatus.gameObject.SetActive(false);
            _botActionStatus.gameObject.SetActive(false);
            _fullActionStatus.gameObject.SetActive(true);
            _fullActionStatus.PlayerAction = Action.FullDeath;
            isDead = true;
            return;
        }
        CurrentHealth -= damageAmount;
    }
}