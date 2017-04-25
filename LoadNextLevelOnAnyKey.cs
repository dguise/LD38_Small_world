using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevelOnAnyKey : MonoBehaviour {

	void Update () {
		if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
