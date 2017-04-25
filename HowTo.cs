using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowTo : MonoBehaviour
{

    Text uiText;
    GameObject loadedObject;
    
    void Start()
    {
        // Runs when this object is created
        uiText = GameObject.Find("Canvas/HelpText").GetComponent<Text>();

        loadedObject = Resources.Load("Prefabs/Cube") as GameObject;

        StartCoroutine(MyCoroutine("variable"));
        Debug.Log("This will still run but the MyCoroutine will run at the same time.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If the button Space was clicked this frame we change the text of the UI text
            uiText.text = "I changed this text through code!";
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            // Spawning a cube every frame you hold down a horizontal key (joystick or A/D)
            Instantiate(loadedObject);
            Debug.Log("Spawned a cube!");
        }
    }

    // En Coroutine (IEnumerator) är ett tråd-liknande beteende
    IEnumerator MyCoroutine(string someText)
    {
        // wait for 2 seconds
        yield return new WaitForSeconds(2);
        Debug.Log("2 seconds has passed, let's run MySecondCoroutine");
        // wait for MySecondCoroutine to finish before continuing
        yield return StartCoroutine(MySecondCoroutine());
        Debug.Log("Ok, we're back!");
    }

    IEnumerator MySecondCoroutine()
    {
        // wait for 2 seconds
        yield return new WaitForSeconds(2);
        Debug.Log("Back to MyCoroutine!");
    }
















    private void FixedUpdate()
    {
        // Runs 60 times per seconds
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Another object has collided with this object
    }

    private void OnTriggerEnter(Collider other)
    {
        // You have entered a collision with the box "trigger" set true
    }

    private void OnBecameInvisible()
    {
        // This object is outside the camera (not rendered)
    }

    private void OnBecameVisible()
    {
        // This object was outside camera but is now inside (rendered)
    }

    private void OnCollisionStay(Collision collision)
    {
        // Run every frame when touching another collider
    }

}
