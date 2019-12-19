using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private PoleManagerTutorial _manager = null;
    [SerializeField]
    private GameObject _tutGrap = null;
    [SerializeField]
    private GameObject _tutThrow = null;
    [SerializeField]
    private GameObject _endTutorialScreen = null;

    private uint[] _poles = new uint[10] { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1 };
    private uint[] _poles1 = new uint[8] { 0, 0, 0, 0, 2, 0, 0, 1 };
    private uint[] _poles2 = new uint[5] { 0, 0, 0, 0, 1 };

    public static bool FailedToGrapple = false;
    public static bool FailedToThrow = false;

    private void Start()
    {
        if (FailedToThrow)
        {
            for (int i = 0; i < _poles2.Length; i++)
            {
                _manager.SpawnNewPole(_poles2[i], false);
            }
        }
        else if (FailedToGrapple)
        {
            for (int i = 0; i < _poles1.Length; i++)
            {
                _manager.SpawnNewPole(_poles1[i], false);
            }
        }
        else
        {
            for (int i = 0; i < _poles.Length; i++)
            {
                _manager.SpawnNewPole(_poles[i], false);
            }
        }
    }

    public void CloseTutorial(bool closeThrow)
    {
        Time.timeScale = 1.0f;
        if (closeThrow)
            Invoke("EndTutorial", 5.0f);
    }

    public void ShowTutorial(int idx)
    {
        Time.timeScale = 0.0f;
        if (idx == 0)
        {
            _tutGrap.SetActive(true);
        }
        else if (idx == 1)
        {
            _tutThrow.SetActive(true);
        }
    }

    private void EndTutorial()
    {
        SaveSystem.SaveTutorial(true);
        _endTutorialScreen.SetActive(true);
    }

    public void ReplayTutorial()
    {
        FailedToGrapple = false;
        FailedToThrow = false;
        SceneManager.LoadScene(2);
    }

    public void ReturnHome()
    {
        FailedToGrapple = false;
        FailedToThrow = false;
        SceneManager.LoadScene(0);
    }
}
