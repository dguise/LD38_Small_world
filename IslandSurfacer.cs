using System.Collections;
using UnityEngine;

public class IslandSurfacer : MonoBehaviour {

    Vector3 origPos;
    Vector3 minHeight;
    public float time_up;
    public float time_down;

    IEnumerator Start () {
        //Debug.Log("start");
        origPos = transform.position;
        minHeight = origPos + new Vector3(0, -2, 0);

        while (true)
        {
            
            //Debug.Log("Up");
            yield return StartCoroutine(Up());
            //yield return new WaitForSeconds(time_down);
            //Debug.Log("Down");
            yield return StartCoroutine(Down());

        }

    }

    private IEnumerator Down()
    {
        float timestamp = 0;
        do
        {
            timestamp += Time.deltaTime *1;
            transform.position = Vector3.Lerp(minHeight, origPos, timestamp);
            //Debug.Log("Moving down" + timestamp);
            yield return new WaitForEndOfFrame();
        } while (transform.position != origPos);

        yield return new WaitForSeconds(time_up);
    }

    private IEnumerator Up()
    {
        float timestamp = 0;
        do
        {
            timestamp += Time.deltaTime*10;
            transform.position = Vector3.Lerp(origPos, minHeight, timestamp);
            //Debug.Log("Moving up");
            yield return new WaitForEndOfFrame();
        } while (transform.position != minHeight);

        yield return new WaitForSeconds(time_down);

    }
}
