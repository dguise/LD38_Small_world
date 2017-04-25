using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mycam : MonoBehaviour
{
    Camera cam;
    GameObject leg_left;
    GameObject leg_right;
    public float lean_value = 0;
    public bool have_fallen = false;
    private bool do_once = true;
    private float sound_warning_delay1 = 0.5f;
    private float sound_warning_delay2 = 0.3f;
    private float sound_warning_timer = 0;
    public float pants_hp = 1;

    public AudioClip sound_fallen;
    public AudioClip sound_lean_warning;
    public AudioClip sound_stomp;

    void Start()
    {
        leg_left = GameObject.Find("Leg_left");
        leg_right = GameObject.Find("Leg_right");
        cam = Camera.main;
    }

    void Update()
    {
        if (have_fallen)
        {
            //move cam to floor
            if (cam.transform.position.y > 2)
            {
                Vector3 cam_pos1 = cam.transform.position;
                cam_pos1.y -= 0.5f;
                cam.transform.position = cam_pos1;

                //impact sound
                if (cam.transform.position.y <= 2)
                {
                    GetComponent<AudioSource>().PlayOneShot(sound_stomp);
                }

                //continue leaning
                float lean_sens = 0.01f;
                if (!leg_left.GetComponent<movement>().on_floor)
                {
                    lean_value += lean_sens;
                }
                if (!leg_right.GetComponent<movement>().on_floor)
                {
                    lean_value -= lean_sens;
                }
                //set rotation of cam
                Vector3 rot_ang = cam.transform.localEulerAngles;
                cam.transform.localEulerAngles = new Vector3(90 - 90 * lean_value, 90, 90);

                if (do_once)
                {
                    do_once = !do_once;
                    StartCoroutine(FadeOut(true));
                }
            }
        }


        //update lean value
        if (!have_fallen)
        {
            float leg_dist = Vector3.Distance(leg_left.transform.position, leg_right.transform.position);
            float lean_sens = 0.0001f;
            if (!leg_left.GetComponent<movement>().on_floor)
            {
                lean_value += lean_sens * leg_dist;
            }
            if (!leg_right.GetComponent<movement>().on_floor)
            {
                lean_value -= lean_sens * leg_dist;
            }
            if (leg_left.GetComponent<movement>().on_floor && leg_right.GetComponent<movement>().on_floor)
            {
                //un-lean
                if (lean_value < 0) lean_value += lean_sens * 10;
                if (lean_value > 0) lean_value -= lean_sens * 10;
                if (Mathf.Abs(lean_value) < lean_sens * 20) lean_value = 0;
            }

            //lean warning sound test
            float lean_tol_warning = 0.15f;
            float lean_tol_warning2 = 0.20f;
            if (lean_value > lean_tol_warning || lean_value < -lean_tol_warning)
            {
                //play sound
                if (sound_warning_timer == 0)
                {
                    //set audio speed
                    if (lean_value > lean_tol_warning2 || lean_value < -lean_tol_warning2)
                    {
                        sound_warning_timer = sound_warning_delay2;
                    }
                    else sound_warning_timer = sound_warning_delay1;

                    var aus = GetComponent<AudioSource>();
                    aus.pitch = Random.Range(1.1f, 1.5f);
                    aus.PlayOneShot(sound_lean_warning);
                }
                else//cut long timer if warning2
                {
                    if (lean_value > lean_tol_warning2 || lean_value < -lean_tol_warning2)
                    {
                        if (sound_warning_timer > sound_warning_delay2) sound_warning_timer = sound_warning_delay2;
                    }
                }
            }
            //no sound if both legs on ground
            if (leg_left.GetComponent<movement>().on_floor && leg_right.GetComponent<movement>().on_floor) sound_warning_timer = sound_warning_delay1;

            //fall test
            float lean_tol_max = 0.3f;
            if (lean_value > lean_tol_max)
            {
                lean_value = lean_tol_max;

                if (!have_fallen)
                {
                    have_fallen = true;

                    //play sound
                    var aus = GetComponent<AudioSource>();
                    aus.pitch = Random.Range(0.8f, 1.2f);
                    aus.PlayOneShot(sound_fallen);
                }
            }
            if (lean_value < -lean_tol_max)
            {
                lean_value = -lean_tol_max;

                if (!have_fallen)
                {
                    have_fallen = true;

                    //play sound
                    GetComponent<AudioSource>().PlayOneShot(sound_fallen);
                }
            }
            //Debug.Log(lean_value);

            //set rotation of cam
            Vector3 rot_ang = cam.transform.localEulerAngles;
            cam.transform.localEulerAngles = new Vector3(90 - 90 * lean_value, 90, 90);

            //update hud lean bar...
        }


        Vector3 cam_pos = cam.transform.position;
        Vector3 cam_target_pos = (leg_left.transform.position + leg_right.transform.position) / 2;

        //shift target in front of legs
        cam_target_pos.z += 7;
        Vector3 cam_target_dir = cam_target_pos - cam_pos;
        //ignore height
        cam_target_dir.y = 0;

        //move towards camera target
        float dead_zone = 0.1f;
        float move_sens = 0.02f;
        if (cam.transform.position.x > cam_target_pos.x + dead_zone ||
           cam.transform.position.x < cam_target_pos.x - dead_zone ||
           cam.transform.position.z > cam_target_pos.z + dead_zone ||
           cam.transform.position.z < cam_target_pos.z - dead_zone)
        {
            cam_pos += cam_target_dir * move_sens;
        }

        cam.transform.position = cam_pos;
    }

    private void FixedUpdate()
    {
        if (sound_warning_timer > 0)
        {
            sound_warning_timer -= Time.fixedDeltaTime;
            if (sound_warning_timer < 0) sound_warning_timer = 0;
        }
    }

    private bool points_added = false;
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "is_goal")
        {
            if (SceneManager.GetActiveScene().name == "level4")
            {
                var baby = Resources.Load<GameObject>("prefabs/babby");
                var bab = Instantiate(baby, transform.position, Quaternion.identity);
                bab.GetComponent<Rigidbody>().AddTorque((Vector3.right + Vector3.up) * 1000);
            }

            if (!points_added)
            {
                points_added = true;
                GameObject.Find("point_counter").GetComponent<point_count_script>().points_sum += Camera.main.GetComponent<GameManager>()._points;
                Debug.Log(GameObject.Find("point_counter").GetComponent<point_count_script>().points_sum);

            }

            StartCoroutine(FadeOut(false));
        }
    }

    private bool isNotFading = true;
    public IEnumerator FadeOut(bool restartLevel)
    {
        if (isNotFading)
        {
            isNotFading = false;
            var overlay = GameObject.Find("FadeOverlay").GetComponent<RawImage>();

            Color from = new Color(0, 0, 0, 0);
            Color to = new Color(0, 0, 0, 1);
            float timestamp = 0;
            do
            {
                timestamp += Time.deltaTime * 0.3f;
                overlay.color = Color.Lerp(from, to, timestamp);
                yield return new WaitForEndOfFrame();
            } while (overlay.color != to);
            yield return new WaitForSeconds(2);

            if (restartLevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
