using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform MeleeEnemy;
    public Transform ProjectileEnemy;

    public Transform TargetObj;
    [SerializeField]
    private List<Enemy> _enemies;
    void Start()
    {
        _enemies = new List<Enemy>();
        float distance = (MeleeEnemy.position - TargetObj.position).magnitude;  
        // Bad coding
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        for(int i = 0; i < allEnemies.Length; i++){
            Enemy enemy = allEnemies[i];
            if(enemy.GetComponent<EnemyMelee>() != null){
                _enemies.Add(enemy.GetComponent<EnemyMelee>());
            }
            else if(enemy.GetComponent<EnemyShooter>() != null){
                _enemies.Add(enemy.GetComponent<EnemyShooter>());
            }
            else if(enemy.GetComponent<EnemyBoss>() != null){
                _enemies.Add(enemy.GetComponent<EnemyBoss>());
            }
        }
    }
    void Update()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].UpdateAction(TargetObj, Time.deltaTime);
        }
    }
}