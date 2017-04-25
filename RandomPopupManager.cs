using System.Collections;
using UnityEngine;

public class RandomPopupManager : MonoBehaviour
{

    GameObject popup;
    [Range(0, 10)]
    public float maxDelay;
    [Range(0, 10)]
    public float minDelay;

    private IEnumerator SpawnPopups()
    {
        while (false)
        {
            Instantiate(popup, transform);
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
