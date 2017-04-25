using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour
{
    public void Crash(){StartCoroutine(_Crash());}
    private IEnumerator _Crash()
    {
        var sounds = Resources.LoadAll<AudioClip>("Sounds/crash");
        var aus = gameObject.AddComponent<AudioSource>();
        AudioClip audio = sounds[Random.Range(0, sounds.Length)];
        aus.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
        Destroy(gameObject);
    }

    public void Unit() { StartCoroutine(_Unit()); }
    private IEnumerator _Unit()
    {
        var sounds = Resources.LoadAll<AudioClip>("Sounds/die");
        var aus = gameObject.AddComponent<AudioSource>();
        AudioClip audio = sounds[Random.Range(0, sounds.Length)];
        aus.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
        Destroy(gameObject);
    }

    public void Bear() { StartCoroutine(_Bear()); }
    private IEnumerator _Bear()
    {
        var sounds = Resources.LoadAll<AudioClip>("Sounds/bear");
        var aus = gameObject.AddComponent<AudioSource>();
        AudioClip audio = sounds[Random.Range(0, sounds.Length)];
        aus.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
        Destroy(gameObject);
    }

    public void Tree() { StartCoroutine(_Tree()); }
    private IEnumerator _Tree()
    {
        var sounds = Resources.LoadAll<AudioClip>("Sounds/tree");
        var aus = gameObject.AddComponent<AudioSource>();
        AudioClip audio = sounds[Random.Range(0, sounds.Length)];
        aus.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
        Destroy(gameObject);
    }
}
