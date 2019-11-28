using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    //Fader Text 'Tap to start'
    [SerializeField] 
    private TMP_Text _text;

    private bool _isFaded;
    private float _alpha;

    void Start()
    {
        if (!AudioManager.instance.IsSongPlaying("MainMenu"))
        {
            AudioManager.instance.StopSong("GamePlay");
            AudioManager.instance.PlaySong("MainMenu");
        }

        _isFaded = false;
        _text = GetComponentInChildren<TMP_Text>();
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
        AudioManager.instance.PlaySoundEffect("PlayButton");
        SceneManager.LoadScene("Landscape");
    }

    public void TestClick()
    {
        Debug.Log("click");
    }
}
