using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour {
    GameObject waterBar;
    Image warningSign;
    public float maxWater = 2;
    

    private float _waterLevel;
    public float waterLevel {
        get
        {
            return _waterLevel;
        } set
        {
            _waterLevel = value;

            var scale = waterBar.transform.localScale;
            scale.x = Mathf.Clamp(_waterLevel / maxWater, 0, 1);

            if(scale.x > 0.7f)
            {
                warningSign.enabled = true;
            } else
            {
                warningSign.enabled = false;
            }

            waterBar.transform.localScale = scale;
        }
    }

	void Start () {
        waterBar = transform.FindChild("WaterBar").gameObject;
        warningSign = transform.FindChild("WarningSign").GetComponent<Image>();
	}

    private void Update()
    {
        if(waterLevel>0)
         waterLevel -= 0.05f;
        if (waterLevel < 0) waterLevel = 0;
    }
}
