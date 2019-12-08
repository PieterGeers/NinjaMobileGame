using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    private Sprite _SlowMotionImage = null; //0
    [SerializeField]
    private Sprite _SuperBreakerImage = null; //1
    [SerializeField]
    private Sprite _InstaKillImage = null; //2
    [SerializeField]
    private Sprite _TripleShotImage = null; //3

    [SerializeField]
    private GameObject _genericPowerUP = null;

    [SerializeField]
    private Image _SlowMotionImageUI = null;
    [SerializeField]
    private Image _SuperBreakerImageUI = null;
    [SerializeField]
    private Image _InstaKillImageUI = null;
    [SerializeField]
    private Image _TripleShotImageUI = null;

    private Color _inactive = new Color(0.3f, 0.3f, 0.3f);
    private Color _active = new Color(1f, 1f, 1f);


    public void SpawnPowerUp(Vector3 position)
    {
        int random = Random.Range(0, 4);
        GameObject temp = Instantiate(_genericPowerUP);
        switch (random)
        {
            case 0:
                temp.GetComponent<SpriteRenderer>().sprite = _SlowMotionImage;
                SlowmotionPU comp1 = temp.AddComponent<SlowmotionPU>();
                comp1.PM = this;
                temp.transform.position = position;
                break;
            case 1:
                temp.GetComponent<SpriteRenderer>().sprite = _SuperBreakerImage;
                SuperBreakerPU comp2 = temp.AddComponent<SuperBreakerPU>();
                comp2.PM = this;
                temp.transform.position = position;
                break;
            case 2:
                temp.GetComponent<SpriteRenderer>().sprite = _InstaKillImage;
                InstaKillPU comp3 = temp.AddComponent<InstaKillPU>();
                comp3.PM = this;
                temp.transform.position = position;
                break;
            case 3:
                temp.GetComponent<SpriteRenderer>().sprite = _TripleShotImage;
                TripleShotPU comp4 = temp.AddComponent<TripleShotPU>();
                comp4.PM = this;
                temp.transform.position = position;
                break;
        }
    }

    public void InactiveSuperBreaker()
    {
        _SuperBreakerImageUI.color = _inactive;
    }

    public void InactiveInstaKill()
    {
        _InstaKillImageUI.color = _inactive;
    }

    public void InactiveSlowMotion()
    {
        _SlowMotionImageUI.color = _inactive;
    }

    public void InactiveTripleShot()
    {
        _TripleShotImageUI.color = _inactive;
    }

    public void ActiveSuperBreaker()
    {
        _SuperBreakerImageUI.color = _active;
    }

    public void ActiveInstaKill()
    {
        _InstaKillImageUI.color = _active;
    }

    public void ActiveSlowMotion()
    {
        _SlowMotionImageUI.color = _active;
    }

    public void ActiveTripleShot()
    {
        _TripleShotImageUI.color = _active;
    }
}
