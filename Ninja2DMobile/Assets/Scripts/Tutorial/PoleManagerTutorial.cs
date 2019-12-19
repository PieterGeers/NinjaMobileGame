using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManagerTutorial : MonoBehaviour
{
    [SerializeField]
    private uint _poleInterval = 5;
    [SerializeField]
    private float _randomSpawnHeight = 1f;

    [SerializeField]
    private GameObject _pole = null;
    [SerializeField]
    private GameObject _grapplePole = null;

    private List<GameObject> _poles = new List<GameObject>();
    private GameObject _newPole = null;

    private Transform GetJumpToPosition(GameObject pole)
    {
        for (int i = 0; i < pole.transform.childCount; ++i)
        {
            if (pole.transform.GetChild(i).tag == "Position")
            {
                return pole.transform.GetChild(i);
            }
        }
        return null;
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

    public void SpawnNewPole(uint idx, bool destroy)
    {
        //Destroy oldest pole
        if (destroy)
        {
            GameObject oldPole = _poles[0];
            _poles.Remove(oldPole);
            Destroy(oldPole);
        }

        //Spawn in new pole
        switch (idx)
        {
            case 0:
                SpawnDefaultPole();
                break;
            case 1:
                SpawnPoleWithObstacle();
                break;
            case 2:
                SpawnGrapplePole();
                break;
        }
    }

    public GameObject GetPole(int i)
    {
        return _poles[i];
    }
}
