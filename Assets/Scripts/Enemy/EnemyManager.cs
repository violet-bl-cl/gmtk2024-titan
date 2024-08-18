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
            if(enemy.GetComponent<EnemyZombie>() != null){
                 Debug.Log(allEnemies[i].name + $"Enemey {i}");
                _enemies.Add(enemy.GetComponent<EnemyZombie>());
            }
        }
         Debug.Log(allEnemies[0].name);
    }
    void Update()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].UpdateAction(TargetObj, Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        //for(int i = 0; i < _enemies.Count; i++){
        //DrawHelper.SetTransform(_enemies[i]);
        //DrawHelper.DrawRaySphere(Vector2.up, 0, _shootRange);
//
        //}
    }
}