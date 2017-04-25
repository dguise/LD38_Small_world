using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RandomPopupText : MonoBehaviour {
    Text uiText;

    Color startColor = new Color(255, 255, 255, 0);
    Color endColor = new Color(255, 255, 255, 1);

    float maxX = Screen.width - 250;
    float maxY = Screen.height - 50;

    float offsetX = 125;
    float offsetY = 50;

    string[] sentences = new string[]
    {
        "Why",
        "I can't believe you've done this",
        "Taxa mica",
        "I can see the bone",
        "My pelvis!",
        "I need iodine!",
        "There goes the neighbourhood...",
        "My car!",
        "Not again...",
        "One day to retirement...",
        "You will pay for this!",
        "Is that an ogre?",
        "Is that a plane?",
        "It's a donkey!!!",
        "Obamacare, here I come!",
        "Oh no.",
        "RUN, RON!!!",
        "Darling, get the kids!!",
        "We have a huge problem...",
        "What's that smell?",
        "Am I on a prank show..?",
        "Huston, we have a problem!",
        "Godzilla!!!!!!",
        "What in OBLIVION is THAT!?",
        "I can see my life flashing before my eyes",
        "Aw man, I LOVE Shingeki no kyojin!",
        "HELP",
        "help",
        "Help",
        "Help!",
        "Aaaaaa",
        "Hurg",
        "No",
        "That was priceless",

    };

    // Use this for initialization
    void Start () {
        uiText = GetComponent<Text>();
        uiText.text = sentences[Random.Range(0, sentences.Length)];
        uiText.fontSize = Random.Range(10, 20);

        var posX = Random.Range(offsetX, maxX);
        var posY = Random.Range(offsetY, maxY);

        uiText.gameObject.transform.position = new Vector2(posX, posY);

        StartCoroutine(_Popup());
    }

    private IEnumerator _Popup()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(2);
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
}
