using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    //Fader Text 'Tap to start'
    private bool _isFaded;
    private float _alpha;
    [SerializeField] private TMP_Text _text;


    void Start()
    {
        _isFaded = false;
    }


    void Update()
    {
        TextFader();
    }


    void TextFader()
    {
        //fades in/out the text
        if (_isFaded)  { _alpha += Time.deltaTime; }
        else if (!_isFaded) { _alpha -= Time.deltaTime; }

        //Check if text can fade or not
        if (_alpha >= 1.0f) { _isFaded = false; }
        else if (_alpha <= 0.0f) { _isFaded = true; }

        //changes the component
          _text.alpha = _alpha;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Landscape");
    }
}
