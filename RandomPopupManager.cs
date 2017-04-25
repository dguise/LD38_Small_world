using System.Collections;
using UnityEngine;

public class RandomPopupManager : MonoBehaviour {

    GameObject popup;
    [Range(0, 10)]
    public float maxDelay;
    [Range(0, 10)]
    public float minDelay;

    // Use this for initialization
    void Start () {
        
        //Instantiate(popup, transform);
        //StartCoroutine(SpawnPopups());
	}

    private IEnumerator SpawnPopups()
    {
        while (false)
        {
            Instantiate(popup, transform);
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
