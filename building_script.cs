using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building_script : MonoBehaviour
{
    GameObject pref_destroyed;
    GameObject ps;
    AudioClip[] crashSounds;

    void Start()
    {
        pref_destroyed = (GameObject)Resources.Load("prefabs/Building_destroyed", typeof(GameObject));
        ps = Resources.Load<GameObject>("prefabs/DustParticle");
        crashSounds = Resources.LoadAll<AudioClip>("Sounds/crash");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "is_foot")
        {
            Camera.main.GetComponent<GameManager>().points -= 10;

            Instantiate(pref_destroyed, transform.position, Quaternion.identity);

            Instantiate(ps, transform.position + new Vector3(0, 0.2f, 0), ps.transform.rotation);

            var aus = transform.parent.GetComponent<AudioSource>();
            aus.pitch = Random.Range(0.8f, 1.2f);
            aus.PlayOneShot(crashSounds[Random.Range(0, crashSounds.Length)]);
            GameObject.Find("LevelPopup").GetComponent<PopupText>().SpawnRandomPopup();

            Destroy(gameObject, 0.1f);
        }
    }
}
