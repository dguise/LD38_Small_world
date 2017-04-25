using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour {

    private BoxCollider bc;
    private Rigidbody rb;
    private bool leg_left_grab = false;
    Camera cam;
    private Vector3 mousepos_onfloor;
    public float feet_hight_max = 5;
    public bool isleftleg = true;
    GameObject other_leg;
    public bool on_floor = true;
    GameObject pref_footprint;
    GameObject pref_footprint_shoe;
    GameObject pref_footprint_sock;
    public AudioClip audio_lift;
    public AudioClip audio_stomp;
    float lift_delay = 0.1f;
    float lift_timer = 0;
    WaterBar waterBar;
    public bool is_slippery = false;
    GameObject dp;
    GameObject bar_pants;

    // Use this for initialization
    void Start ()
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        pref_footprint = (GameObject)Resources.Load("prefabs/Footprint", typeof(GameObject));
        pref_footprint_sock = (GameObject)Resources.Load("prefabs/Footprint_sock", typeof(GameObject));
        pref_footprint_shoe = (GameObject)Resources.Load("prefabs/Footprint_shoe", typeof(GameObject));
        dp = Resources.Load<GameObject>("prefabs/DustParticle");
        bar_pants=GameObject.Find("TrouserStatus");

        var ws = GameObject.Find("WaterStatus");
        if(ws != null)
        {
            waterBar = ws.GetComponent<WaterBar>();
        }

        if (this.name == "Leg_left")
        {
            other_leg = GameObject.Find("Leg_right");
        } else
        {
            other_leg = GameObject.Find("Leg_left");
        }
        Debug.Log(other_leg.name);

        cam = Camera.main;


        //hide and lock mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //cam test
        //cam.transform.Rotate(new Vector3(1,0,0),0.1f);
        //cam.transform.localEulerAngles = new Vector3(90, 90, 90);//change x-val for tilt

        int active_mouse_button = 1;
        if (isleftleg) active_mouse_button = 0;

        //no click requirement for foot selection
        /*if (Input.GetMouseButton(active_mouse_button))
        {
            leg_left_grab = true;
        }*/
        
        /*//check if moving leg
        if (leg_left_grab)
        {
            if (Input.GetMouseButtonUp(0))
            {
                leg_left_grab = false;
                //stop movement
                Vector3 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;
                rb.velocity = new Vector3(0,0,0);

            }
        }*/

        //rel mouse move leg
        Vector3 pos = transform.position;
        if (Input.GetMouseButton(active_mouse_button) && other_leg.GetComponent<movement>().on_floor && lift_timer==0)
        {
            on_floor = false;

            float move_sens = 0.2f;
            float float_sens = 0.1f;

            pos.x += move_sens * Input.GetAxis("Mouse X");
            pos.z += move_sens * Input.GetAxis("Mouse Y");
            if(pos.y < feet_hight_max) pos.y += float_sens;

            //cap leg pos
            Vector3 cam_pos = cam.transform.position;
            float walk_area_side = 30;
            float walk_area_front = 15;
            float leg_separation_min = 2;
            if (isleftleg)
            {
                if (pos.x > cam_pos.x - leg_separation_min) pos.x = cam_pos.x - leg_separation_min;

                if (other_leg.transform.position.x - pos.x > walk_area_side)
                {
                    pos.x = other_leg.transform.position.x - walk_area_side;

                    //if level 5 update pants bar
                    
                    Camera.main.GetComponent<mycam>().pants_hp -= Time.deltaTime;
                }

            }
            else
            {
                if (pos.x < cam_pos.x + leg_separation_min) pos.x = cam_pos.x + leg_separation_min;

                if (other_leg.transform.position.x - pos.x < -walk_area_side)
                {
                    pos.x = other_leg.transform.position.x + walk_area_side;

                    //if level 5 update pants bar
                    Camera.main.GetComponent<mycam>().pants_hp -= Time.deltaTime;
                }

            }
            if (pos.z > other_leg.transform.position.z + walk_area_front)
            {
                pos.z = other_leg.transform.position.z + walk_area_front;

                //if level 5 update pants bar
                Camera.main.GetComponent<mycam>().pants_hp -= Time.deltaTime;
            }
            if (pos.z < other_leg.transform.position.z - walk_area_front)
            {
                pos.z = other_leg.transform.position.z - walk_area_front;

                //if level 5 update pants bar
                Camera.main.GetComponent<mycam>().pants_hp -= Time.deltaTime;
            }

            //reset gravity
            Vector3 vel = rb.velocity;
            vel.y = 0;
            rb.velocity = vel;
        }
        //move feet down
        float feet_down_sens = 0.5f;
        if(!Input.GetMouseButton(active_mouse_button) && !on_floor)
        {
            pos.y -= feet_down_sens;
        }


        //slippery effect
        if(is_slippery)
        {
            //increase distance of feets on ground
            if(on_floor)
            {
                float slip_sens = 0.002f;
                Vector3 move_dir = transform.position - other_leg.transform.position;
                pos += move_dir * slip_sens;
            }
        }

        transform.position = pos;

        //find mouse pos on plane

        /*Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        //find floor
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.tag=="is_floor")
            {
                
                mousepos_onfloor = hit.point;
                //Debug.Log(mousepos_onfloor);
                //Debug.DrawLine(new Vector3(-5, 0.5f, 0), new Vector3(5, 0.5f, 0),Color.red);
            }
        }*/
    }

    private void FixedUpdate()
    {
        if(lift_timer>0)
        {
            lift_timer -= Time.fixedDeltaTime;
            if (lift_timer < 0) lift_timer = 0;
        }

        /*//leg movement
        if (leg_left_grab)
        {
            rb.velocity = new Vector3(0, 0, 0);

            Vector3 pos = transform.position;
            //movement leg up
            //if (transform.position.y < feet_hight_max)
            {
                //rb.AddForce(0, 15, 0);
                pos.y += 0.1f;
            }*/

            //move leg rel to mouse movement



            /*
            //move leg to mouse
            float dead_zone = 0.01f;
            float leg_move_speed = 0.1f;
            if (transform.position.x > mousepos_onfloor.x + dead_zone)
            {
                //rb.AddForce(-leg_move_speed, 0, 0);
                pos.x -= leg_move_speed;
            }
            if (transform.position.x < mousepos_onfloor.x - dead_zone)
            {
                //rb.AddForce(leg_move_speed, 0, 0);
                pos.x += leg_move_speed;
            }
            if (transform.position.z > mousepos_onfloor.z + dead_zone)
            {
                //rb.AddForce(0, 0, -leg_move_speed);
                pos.z -= leg_move_speed;
            }
            if (transform.position.z < mousepos_onfloor.z - dead_zone)
            {
                //rb.AddForce(0, 0, leg_move_speed);
                pos.z += leg_move_speed;
            }
            //update pos
            transform.position = pos;
            
        }*/
    }

    void OnMouseDown()
    {
        //leg_left_grab = true;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check if floor
        if(collision.collider.tag=="is_floor")
        {
            on_floor = true;

            //make footprint
            if (!is_slippery)
            {
                GameObject temp;
                if(SceneManager.GetActiveScene().name == "level3")
                    temp=Instantiate(pref_footprint_sock, transform.position, Quaternion.identity);
                else if(SceneManager.GetActiveScene().name == "level4" || SceneManager.GetActiveScene().name == "level5")
                    temp=Instantiate(pref_footprint_shoe, transform.position, Quaternion.identity);
                else temp= Instantiate(pref_footprint, transform.position, Quaternion.identity);

                //rotate
                if (SceneManager.GetActiveScene().name == "level3")
                {
                    Vector3 rot = temp.transform.eulerAngles;
                    rot.y += 180;
                    temp.transform.eulerAngles = rot;
                }

                Vector3 tmpvec = temp.transform.position;
                tmpvec.y = 0.1f;
                tmpvec.z += 2f;
                temp.transform.position = tmpvec;
                Vector3 foot_scale = temp.transform.localScale;
                if (isleftleg) foot_scale.x = -0.4f;
                temp.transform.localScale = foot_scale;

                
            }

            //footprint damage cost
            Camera.main.GetComponent<GameManager>().points -= 1;

            //play sound
            var aus = GetComponent<AudioSource>();
            aus.pitch = Random.Range(0.8f, 1.2f);
            aus.PlayOneShot(audio_stomp);

            //set lift delay
            //lift_timer = lift_delay;

        }


        if (collision.collider.tag == "is_tree")
        {
            //score
            Camera.main.GetComponent<GameManager>().points -= 10;

            //make particles
            Instantiate(dp, collision.transform.position + new Vector3(0, 0.2f, 0), dp.transform.rotation);

            //play sound
            var res = Resources.Load<GameObject>("prefabs/SoundPlayer");
            var obj = Instantiate(res, transform.position, Quaternion.identity);
            var sound = obj.GetComponent<SoundPlayer>();
            sound.Tree();

            //remove
            Destroy(collision.gameObject, 0.1f);
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        //check if floor
        if (collision.collider.tag == "is_floor")
        {
            on_floor = false;

            //play sound
            GetComponent<AudioSource>().PlayOneShot(audio_lift);
        }
    }

    bool restart_once = true;
    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "is_water")
        {
            waterBar.waterLevel += 0.1f;
            
            if (waterBar.waterLevel >= waterBar.maxWater && restart_once)
            {
                restart_once = false;
                StartCoroutine(Camera.main.GetComponent<mycam>().FadeOut(true));
                GameObject.Find("LevelPopup").GetComponent<PopupText>().Popup("No one can achieve anything with wet socks");
            }
        }
    }
}
