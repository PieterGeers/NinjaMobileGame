using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 _waveDelay = Vector2.zero;
    [SerializeField]
    private List<EnemySpawner> _spawners = null;
    private float _timer = 0f;

    private void Start()
    {
        _timer = Random.Range(_waveDelay.x, _waveDelay.y);
    }


    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            Spawn();
            _timer = Random.Range(_waveDelay.x, _waveDelay.y);
        }
    }

    private void Spawn()
    {
        int rand = Random.Range(0, _spawners.Count);
        _spawners[rand].SpawnEnemy(Random.Range(2,4), 4.0f);
    }
}
