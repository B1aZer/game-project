using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catcher : MonoBehaviour
{
    public Slider powerBar;
    public Text powerText; 
    public GameObject aim_panel;

    private BaseLogic baseCtrl;
    private heavy_trajectory baseTraj;

    private bool ball_released = false;
    private bool start_spin = false;

    //private bool sim_path = false;

    private int powerAcc0 = 0;
    private int powerAcc1 = 0;
    //TODO: move to base?
    private Canvas crossHair;
    
    public AudioSource glu_sound;
    public AudioSource chrg_sound;

    private List<Collider> balls_to_be_catched = new List<Collider>();
    private Collider locked_ball;
    void Awake()
    {
        GameObject go = GameObject.Find ("Base");
        baseCtrl = go.GetComponent<BaseLogic>();
        GameObject go1 = GameObject.Find ("TrajectoryRenderer");
        baseTraj = go1.GetComponent<heavy_trajectory>();
        //glu_sound = this.GetComponent<AudioSource>();
        glu_sound.Stop();
        //chrg_sound = this.GetComponent<AudioSource>();
        chrg_sound.Stop();
        powerBar.gameObject.SetActive(false);
        aim_panel.SetActive(false);
        
        //GameObject go2 = GameObject.Find ("Image");
        
        //Canvas crossHair = go2.GetComponent<Image>();

        //crossHair.enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (start_spin) {
            locked_ball.transform.Rotate( new Vector3(0, 1, 1) * powerAcc0 * Time.deltaTime); 
           
        }

        // testo
        if (ball_released) {
            // Curve
            //baseCtrl.catched_ball.attachedRigidbody.AddRelativeForce (Vector3.left*1.088f, ForceMode.Impulse);         
        }
        // flag indicates that there are some balls in catcher zone
        if (balls_to_be_catched.Count > 0) {
           
            // trying to catch
            if (Input.GetMouseButton(0)) {

                powerAcc0++;
                // Max power is 100
                powerAcc0 = Mathf.Min(powerAcc0, 100);
                powerText.text = powerAcc0.ToString();
                // power UI update 
                powerBar.gameObject.SetActive(true); 
                powerBar.value = powerAcc0 / 100f;
                               
                // if ball catched
                if (locked_ball) {
                    
                    //start_spin = true;    
                    if (powerAcc0 > 30 && powerAcc0 % 10 == 0) {
                        //crossHair.enabled = true;                   
                        baseCtrl.ChangeCams(1);
                        if (!chrg_sound.isPlaying) {
                            chrg_sound.Play();
                        }
                        aim_panel.SetActive(true);                    
                    }

                // if ball is not catched
                } else {                    
                    // just take the first ball
                    Collider catched_ball = balls_to_be_catched[0];
                    catched_ball.GetComponent<SphereCollider>().isTrigger = true;
                    catched_ball.attachedRigidbody.isKinematic = true;
                    catched_ball.attachedRigidbody.velocity = Vector3.zero;
                    // Probably dont' need, check
                    catched_ball.attachedRigidbody.angularVelocity = Vector3.zero;
                    catched_ball.transform.SetParent(baseCtrl.catcher.transform);
                    catched_ball.gameObject.transform.localPosition = new Vector3(0, -0.2f, 0);
                    locked_ball = catched_ball;
                }
                
            }

            // releasing catch button
            if (Input.GetMouseButtonUp(0)) {

                // and we actually have catched ball
                if (locked_ball) {
                    
                    Vector3 mp = Input.mousePosition;
                    // reset to 0,0 center coordinates
                    mp.x -= Screen.width/2;
                    mp.y -= Screen.height/2;
                    
                    //baseCtrl.locked_ball.attachedRigidbody.AddTorque(mp, ForceMode.Impulse);
                    start_spin = true;
                    
                    
                }

                // reset power and UI
                chrg_sound.Stop();
                baseCtrl.ChangeCams(3);
                aim_panel.SetActive(false);
                powerAcc0 = 0;
                powerBar.gameObject.SetActive(false); 
                powerBar.value = 0; 
            }
        
            // trying to shoot the ball
            if (Input.GetMouseButton(1)) {
                powerAcc1++;
                // Max power is 100
                powerAcc1 = Mathf.Min(powerAcc1, 100);
                powerText.text = powerAcc1.ToString();
                // power UI update 
                powerBar.gameObject.SetActive(true); 
                powerBar.value = powerAcc1 / 100f;
                // throttle every 10 * 0.02 s
                if (powerAcc1 > 30 && powerAcc1 % 10 == 0) {
                    //crossHair.enabled = true;                   
                    baseCtrl.ChangeCams(1);
                    if (!glu_sound.isPlaying) {
                        glu_sound.Play();
                    }
                    aim_panel.SetActive(true);
                          
                }
                float force = powerAcc1 / 5;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // debug ray
                baseCtrl.drawProjectile(locked_ball.attachedRigidbody, ray.direction * force, locked_ball.attachedRigidbody.drag, 1f, Time.fixedDeltaTime);
            }

            // when shoot button is released
            if (Input.GetMouseButtonUp(1)) {
            
                // if we have the ball in cather zone
                if (locked_ball) {
                    locked_ball.transform.SetParent(null);
                    locked_ball.GetComponent<SphereCollider>().isTrigger = false;
                    locked_ball.attachedRigidbody.isKinematic = false;
                    //baseCtrl.DetractFrom(baseCtrl.catcher, baseCtrl.catched_ball, powerAcc1);
                    
                    //Trying to give force manually
                    // TODO: This ray comes fromom camera, wrong direction
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float force = powerAcc1 / 5;
                    
                    // shoot the ball with the force
                    //baseCtrl.locked_ball.attachedRigidbody.AddForce(ray.direction * force, ForceMode.Impulse);
                    locked_ball.attachedRigidbody.velocity = ray.direction * force /  locked_ball.attachedRigidbody.mass;
                    
                    baseCtrl.ChangeCams(3);
                    locked_ball = null;
                    ball_released = true;
                    start_spin = false;    
                    powerAcc0 = 0;
                    powerAcc1 = 0;
                    powerBar.gameObject.SetActive(false); 
                    powerBar.value = 0; 
                    aim_panel.SetActive(false);
                    
                }     

            }     

        } else if (baseCtrl.in_vacuum_zone) {

            if (Input.GetMouseButton(0)) {               
                SphereCollider[] balls = FindObjectsOfType<SphereCollider>();
                foreach(SphereCollider ball in balls) {
                    baseCtrl.AttractTo(baseCtrl.catcher, ball);
                }
                
            }

            if (Input.GetMouseButton(1)) {
                SphereCollider[] balls = FindObjectsOfType<SphereCollider>();                
                foreach(SphereCollider ball in balls) {
                    baseCtrl.DetractFrom(baseCtrl.catcher, ball);
                }
            }
         }
    }
    
    private void OnTriggerEnter(Collider other)
    {
       
    }
    
    private void OnTriggerStay(Collider other)
    {
        // if we enteracting with ball
        if (other.GetType() == typeof(SphereCollider)) {
            balls_to_be_catched.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
         // if we enteracting with ball
        if (other.GetType().IsAssignableFrom(typeof(UnityEngine.SphereCollider))) {
            balls_to_be_catched.Remove(other);
        }
       
    }

}
