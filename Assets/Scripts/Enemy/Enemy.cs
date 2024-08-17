using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform enemyObj;
    float _movementSpeed = 10.0f;
    public virtual void UpdateAction(Transform targetObj){
        
    }

    protected void ExecuteAction(Transform targetObj, EnemyAction enemyAction)
    {
        switch(enemyAction){
            case EnemyAction.Idle:{
                break;
            }
            case EnemyAction.Shoot:{
                break;
            }
            case EnemyAction.Jump:{
                break;
            }
            case EnemyAction.Walk:{
                break;
            }
        }
    }
}