using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab = null;

    public void SpawnEnemy(int health, float speed)
    {
        AudioManager.instance.PlaySoundEffect("Bush");
        GameObject temp = Instantiate(_enemyPrefab);
        temp.transform.position = transform.position;
        temp.GetComponent<Enemy>().SetVariables(health, speed);
    }
}
