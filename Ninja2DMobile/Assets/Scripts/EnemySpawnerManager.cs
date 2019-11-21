using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 _spawnAmount = Vector2.zero;
    [SerializeField]
    private Vector2 _waveDelay = Vector2.zero;
    [SerializeField]
    private Vector2 _spawnDelay = Vector2.zero;
    [SerializeField]
    private List<EnemySpawner> _spawners = null;
    private float _timer = 0f;
    private int _nbOfSpawns = 0;

    private void Start()
    {
        _timer = Random.Range(_waveDelay.x, _waveDelay.y);
        _nbOfSpawns = Random.Range((int)_spawnAmount.x, (int)_spawnAmount.y);
    }


    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            for (int i = 0; i < _nbOfSpawns; ++i)
            {
                float delay = Random.Range(_spawnDelay.x, _spawnDelay.y);
                Invoke("Spawn", delay);
            }
            _timer = Random.Range(_waveDelay.x, _waveDelay.y);
            _nbOfSpawns = Random.Range((int)_spawnAmount.x, (int)_spawnAmount.y);
        }
    }

    private void Spawn()
    {
        int rand = Random.Range(0, _spawners.Count);
        _spawners[rand].SpawnEnemy(2, 4.0f);
    }
}
