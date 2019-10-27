using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManager : MonoBehaviour
{
    [SerializeField]
    private uint _poleInterval = 5;
    [SerializeField]
    private float _randomSpawnHeight = 1f;
    [SerializeField]
    private uint _nbOfPoles = 10;
    [SerializeField]
    private Vector2 _obstacleChance = new Vector2(0, 50);
    [SerializeField]
    private Vector2 _grappleChance = new Vector2(80, 100);

    [SerializeField]
    private GameObject _pole = null;
    [SerializeField]
    private GameObject _grapplePole = null;

    private List<GameObject> _poles = new List<GameObject>();
    private GameObject _newPole = null;

    private void Start()
    {
        for (uint i = 0; i < _nbOfPoles; ++i)
        {
            SpawnDefaultPole();
        }
    }

    private void SpawnDefaultPole()
    {
        _newPole = Instantiate(_pole);
        _newPole.transform.position = transform.position + new Vector3(0.0f, Random.Range(_randomSpawnHeight, -_randomSpawnHeight), 0.0f);
        _poles.Add(_newPole);
        transform.position = new Vector3(transform.position.x + _poleInterval, transform.position.y, transform.position.z);
    }

    private void SpawnPoleWithObstacle()
    {
        SpawnDefaultPole();
        _newPole.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void SpawnGrapplePole()
    {
        _newPole = Instantiate(_grapplePole);
        _newPole.transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
        _poles.Add(_newPole);
        transform.position = new Vector3(transform.position.x + _poleInterval, transform.position.y, transform.position.z);
    }

    public void SpawnNewPole()
    {
        //Destroy oldest pole
        GameObject oldPole = _poles[0];
        _poles.Remove(oldPole);
        Destroy(oldPole);

        //Spawn in new pole
        int chance = Random.Range(0, 100);
        if (_newPole.tag == "GrapplingPole")
        {
            SpawnDefaultPole();
        }
        else if (chance > _obstacleChance.x && chance < _obstacleChance.y)
        {
            SpawnPoleWithObstacle();
        }
        else if (chance > _grappleChance.x && chance < _grappleChance.y)
        {
            SpawnGrapplePole();
        }
        else
        {
            SpawnDefaultPole();
        }
    }

    public GameObject GetPole(int i)
    {
        return _poles[i];
    }
}
