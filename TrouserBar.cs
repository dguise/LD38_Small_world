using UnityEngine;
using UnityEngine.UI;

public class TrouserBar : MonoBehaviour
{
    GameObject trouserBar;
    Image warningSign;
    float maxDistance = 30;

    GameObject lFoot;
    GameObject rFoot;

    private float _trouserLevel;
    public float trouserLevel
    {
        get
        {
            return _trouserLevel;
        }
        set
        {
            _trouserLevel = value;

            var scale = trouserBar.transform.localScale;
            scale.x = Mathf.Clamp(_trouserLevel / maxDistance, 0, 1);

            if (scale.x > 0.7f)
            {
                warningSign.enabled = true;
            }
            else
            {
                warningSign.enabled = false;
            }

            trouserBar.transform.localScale = scale;
        }
    }

    void Start()
    {
        trouserBar = transform.FindChild("TrouserBar").gameObject;
        warningSign = transform.FindChild("WarningSign").GetComponent<Image>();

        lFoot = GameObject.Find("Leg_left");
        rFoot = GameObject.Find("Leg_right");

    }

    bool restart_once = true;
    private void Update()
    {
        trouserLevel = Vector3.Distance(rFoot.transform.position, lFoot.transform.position);

        //break check
        if (Vector3.Distance(rFoot.transform.position, lFoot.transform.position) > maxDistance && restart_once)
        {
            restart_once = false;
            StartCoroutine(Camera.main.GetComponent<mycam>().FadeOut(true));
            GameObject.Find("LevelPopup").GetComponent<PopupText>().Popup("Life can not be sustained with cracked pants");
        }
    }
}
