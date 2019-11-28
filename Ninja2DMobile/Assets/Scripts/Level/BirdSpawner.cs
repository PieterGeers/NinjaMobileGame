using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _birdPrefab = null;
    [SerializeField]
    private float _heighOffset = 1f;
    [SerializeField]
    private Vector2 _randomTimeInterval = Vector2.zero;
    [SerializeField]
    private Vector2 _randomFlySpeed = Vector2.zero;
    [SerializeField]
    private float _MaxAnimationSpeed = .5f;
    [SerializeField]
    bool _spawnLeftOfScreen = true;
    [SerializeField]
    private float _timeOffset = 2.0f;
    [SerializeField]
    private float _destroyTime = 10.0f;
    private float _timer = 0;


    private void Awake()
    {
        if (_birdPrefab == null)
            throw new System.Exception("_birdPrefab = NULL");
    }

    private void Start()
    {
        _timer = Random.Range(_randomTimeInterval.x, _randomTimeInterval.y) + _timeOffset;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0.0f)
        {
            float speed = Random.Range(_randomFlySpeed.x, _randomFlySpeed.y);
            GameObject newBird = Instantiate(_birdPrefab);
            newBird.transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(-_heighOffset, _heighOffset), 0f);
            MoveObject move = newBird.GetComponent<MoveObject>();
            Animator anim = newBird.GetComponent<Animator>();
            anim.speed = (speed / _randomFlySpeed.y) * _MaxAnimationSpeed;
            if (_spawnLeftOfScreen)
            {
                move.SetSpeed(speed);
                newBird.transform.localScale = new Vector3(-newBird.transform.localScale.x, newBird.transform.localScale.y, newBird.transform.localScale.z);
            }
            else
            {
                move.SetSpeed(-speed);
            }
            _timer = Random.Range(_randomTimeInterval.x, _randomTimeInterval.y);
            Destroy(newBird, _destroyTime);
        }
    }
}
