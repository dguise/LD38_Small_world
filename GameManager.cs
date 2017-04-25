using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    bool restart_once = true;
    bool music_started = false;
    float music_delay_timer = 0;
    float music_delay = 2;

    public int points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            if (_points < 0) pointsUi.text = "€ 0";
            else pointsUi.text = "€ " + _points.ToString();

            if (_points < 0 && restart_once)
            {
                restart_once = false;
                StartCoroutine(Camera.main.GetComponent<mycam>().FadeOut(true));
                GameObject.Find("LevelPopup").GetComponent<PopupText>().Popup("Your reckless actions has put you in dept, you must forfeit your career");
            }
        }
    }
    [SerializeField]
    public int _points = 100;
    Text pointsUi;

    void Start()
    {
        pointsUi = GameObject.Find("Points").GetComponent<Text>();
        pointsUi.text = "€ " + _points;
        pointsUi.color = Color.white;

        if (SceneManager.GetActiveScene().name == "level1" || SceneManager.GetActiveScene().name == "level3")
        {
            music_delay_timer = music_delay;
        }
    }

    private void FixedUpdate()
    {
        if (music_delay_timer > 0)
        {
            music_delay_timer -= Time.fixedDeltaTime;
            if (music_delay_timer < 0) music_delay_timer = 0;
            //reduce volume
            GameObject music_go = GameObject.Find("music_sak");
            music_go.GetComponent<AudioSource>().volume = music_delay_timer / music_delay;

            if (music_delay_timer == 0)
            {
                //start music
                music_go.GetComponent<AudioSource>().Stop();
                music_go.GetComponent<AudioSource>().volume = 1;

                if (SceneManager.GetActiveScene().name == "level1")
                    music_go.GetComponent<AudioSource>().clip = music_go.GetComponent<music_script>().sound_music1;
                if (SceneManager.GetActiveScene().name == "level3")
                    music_go.GetComponent<AudioSource>().clip = music_go.GetComponent<music_script>().sound_music2;
                music_go.GetComponent<AudioSource>().Play();
            }
        }
    }

}
