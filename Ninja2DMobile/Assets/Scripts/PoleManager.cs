using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManager : MonoBehaviour
{
    [SerializeField]
    private float _maxTime = 1.0f;
    private float _timer = 0.0f;
    [SerializeField]
    private float _height = 1.0f;
    [SerializeField]
    private GameObject _poleReference = null;


    private void Start()
    {
        if (_poleReference == null)
            throw new System.Exception("_poleReference = NULL");
    }

    private void Update()
    {
        if (_timer >= _maxTime)
        {
            GameObject newPole = Instantiate(_poleReference);
            newPole.transform.position = transform.position + new Vector3(0.0f, Random.Range(_height, -_height), 0.0f);
            Destroy(newPole, 10);

            _timer = 0.0f;
        }

        _timer += Time.deltaTime;
    }
}
