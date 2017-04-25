using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_unit_script : MonoBehaviour
{
    GameObject target1;
    GameObject target2;
    GameObject target_curr;
    GameObject bp;

    public float speed_move = 0.01f;
    public float speed_rotate = 3f;
    public float idle_delay = 3;
    public float idle_timer;
    Animator anim;

    AudioClip[] crashSounds;

    // Use this for initialization
    void Start ()
    {
        target1 = transform.parent.FindChild("target1").gameObject;
        target2 = transform.parent.FindChild("target2").gameObject;
        target_curr = target1;
        anim = GetComponent<Animator>();
        bp = Resources.Load<GameObject>("prefabs/BloodParticle");
        crashSounds = Resources.LoadAll<AudioClip>("Sounds/die");

        idle_timer = idle_delay;
    }
	
    private void FixedUpdate()
    {
        //movement towards target
        if (idle_timer > 0)
        {
            idle_timer -= Time.fixedDeltaTime;
            if (idle_timer < 0)
            {
                idle_timer = 0;
                anim.SetBool("is_walking", true);
            }

            //rotate
            Vector3 targetDir = transform.position - target_curr.transform.position;
            float step = speed_rotate * Time.fixedDeltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            //Debug.Log(targetDir);
        }
        else
        {
            Vector3 target_dir = target_curr.transform.position - transform.position;
            target_dir.Normalize();
            Vector3 new_pos = transform.position;
            new_pos.x += target_dir.x * speed_move;
            new_pos.z += target_dir.z * speed_move;
            transform.position = new_pos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if foot, destroy
        if (collision.collider.tag == "is_foot")
        {
            //make particles
            Instantiate(bp, transform.position, bp.transform.rotation);
            Camera.main.GetComponent<GameManager>().points -= 5;

            //play sound
            var res = Resources.Load<GameObject>("prefabs/SoundPlayer");
            var obj = Instantiate(res, transform.position, Quaternion.identity);
            var sound = obj.GetComponent<SoundPlayer>();

            if (gameObject.tag == "is_bear")
            {
                sound.Bear();
            }
            else if (gameObject.tag == "is_car")
            {
                sound.Crash();
                GameObject.Find("LevelPopup").GetComponent<PopupText>().SpawnRandomPopup();
            }
            else if (gameObject.tag == "is_gobbe")
            {
                sound.Unit();
                GameObject.Find("LevelPopup").GetComponent<PopupText>().SpawnRandomPopup();
            }

            //remove
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == target_curr)
        {
            //change target
            if (target_curr == target1)
                target_curr = target2;
            else
                target_curr = target1;

            idle_timer = idle_delay;

            anim.SetBool("is_walking", false);
        }
    }
}
