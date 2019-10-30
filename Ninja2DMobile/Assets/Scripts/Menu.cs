using UnityEngine;
using TMPro;


public class Menu : MonoBehaviour
{
    public Color newColor;
    private float _timer;
    private float _counterSpeed;



    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer < 1.5f) 
        {
            GetComponentInChildren<TMP_Text>().alpha -= Time.deltaTime;
        }
        else if (_timer > 2.5f && _timer < 4.5f)
        {
            GetComponentInChildren<TMP_Text>().alpha += Time.deltaTime;
        }
        else if (_timer > 4.5f)
        {
            _timer = 0.0f;
        }
    }
}
