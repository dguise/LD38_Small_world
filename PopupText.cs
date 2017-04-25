using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    Text uiText;
    GameObject popup;

    Color startColor = new Color(255, 255, 255, 0);
    Color endColor = new Color(255, 255, 255, 1);

    void Start()
    {
        uiText = GetComponent<Text>();
        popup = Resources.Load<GameObject>("Prefabs/RandomPopup");

        string text_on_screen = "Unknown Level";
        if (SceneManager.GetActiveScene().name == "level1") text_on_screen = "1. Nature Calls";
        if (SceneManager.GetActiveScene().name == "level2") text_on_screen = "2. First Line of Defence";
        if (SceneManager.GetActiveScene().name == "level3") text_on_screen = "3. The Outer Shell";
        if (SceneManager.GetActiveScene().name == "level4") text_on_screen = "4. 1-UP";
        if (SceneManager.GetActiveScene().name == "level5") text_on_screen = "5. Pay up";

        Popup(text_on_screen);
    }

    public void Popup(string text)
    {
        uiText.text = text;
        StartCoroutine(_Popup());
    }

    private IEnumerator _Popup()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float timestamp = 0;
        do
        {
            timestamp += Time.deltaTime;
            uiText.color = Color.Lerp(startColor, endColor, timestamp);
            yield return new WaitForEndOfFrame();
        } while (uiText.color != endColor);
    }

    private IEnumerator FadeOut()
    {
        float timestamp = 0;
        do
        {
            timestamp += Time.deltaTime;
            uiText.color = Color.Lerp(endColor, startColor, timestamp);
            yield return new WaitForEndOfFrame();
        } while (uiText.color != startColor);
    }

    public void SpawnRandomPopup()
    {
        Instantiate(popup, transform);
    }
}

