using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catcher : MonoBehaviour
{
    private BaseLogic baseCtrl;
    private heavy_trajectory baseTraj;

    private bool ball_released = false;
    private bool start_spin = false;

    //private bool sim_path = false;

    private int powerAcc0 = 0;
    private int powerAcc1 = 0;
    //TODO: move to base?
    private Canvas crossHair;
    public Text powerText; 

    void Awake()
    {
        GameObject go = GameObject.Find ("Base");
        baseCtrl = go.GetComponent<BaseLogic>();
        GameObject go1 = GameObject.Find ("TrajectoryRenderer");
        baseTraj = go1.GetComponent<heavy_trajectory>();
        
        //GameObject go2 = GameObject.Find ("Image");
        
        //Canvas crossHair = go2.GetComponent<Image>();

        //crossHair.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (start_spin) {
            baseCtrl.locked_ball.transform.Rotate( new Vector3(0, 1, 1) * powerAcc0 * Time.deltaTime);    
        }

        // testo
        if (ball_released) {
            // what is this ? Curve ?
            //baseCtrl.catched_ball.attachedRigidbody.AddRelativeForce (Vector3.left*1.088f, ForceMode.Impulse);         
        }

        if (baseCtrl.in_catcher_zone) {
           
            if (Input.GetMouseButton(0)) {

                powerAcc0++;
                powerText.text = powerAcc0.ToString();
                               
                if (baseCtrl.locked_ball) {
                    start_spin = true;                        
                } else {                    
                    SphereCollider[] balls = FindObjectsOfType<SphereCollider>();
                    baseCtrl.catched_ball.GetComponent<SphereCollider>().isTrigger = true;
                    baseCtrl.catched_ball.attachedRigidbody.isKinematic = true;
                    baseCtrl.catched_ball.attachedRigidbody.velocity = Vector3.zero;
                    // Probably dont' need, check
                    baseCtrl.catched_ball.attachedRigidbody.angularVelocity = Vector3.zero;
                    baseCtrl.catched_ball.transform.SetParent(baseCtrl.catcher.transform);
                    baseCtrl.catched_ball.gameObject.transform.localPosition = new Vector3(0, -0.2f, 0);
                    baseCtrl.locked_ball = baseCtrl.catched_ball;
                }
                
            }
        
            if (Input.GetMouseButton(1)) {
                 powerAcc1++;
                 powerText.text = powerAcc1.ToString();
                if (powerAcc1 > 30 && powerAcc1 % 60 == 0) {
                    //crossHair.enabled = true;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //baseTraj.simulatePath(baseCtrl.locked_ball.attachedRigidbody, ray.direction * powerAcc1);
                    Debug.DrawRay(Camera.main.transform.position, ray.direction, Color.green, 100, true);
                }
            }

            if (Input.GetMouseButtonUp(1)) {
                
                    if (baseCtrl.catched_ball) {
                        baseCtrl.catched_ball.transform.SetParent(null);
                        baseCtrl.catched_ball.GetComponent<SphereCollider>().isTrigger = false;
                        baseCtrl.catched_ball.attachedRigidbody.isKinematic = false;
                        //baseCtrl.DetractFrom(baseCtrl.catcher, baseCtrl.catched_ball, powerAcc1);
                        
                        //Trying to give force manually
                        // TODO: This ray comes fromom camer, wrong direction
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float force = powerAcc1;
                        Debug.Log(ray.origin);
                        
                        baseCtrl.locked_ball.attachedRigidbody.AddForce(ray.direction * force, ForceMode.Impulse);

                        baseCtrl.locked_ball = null;
                        ball_released = true;
                        start_spin = false;    
                        powerAcc0 = 0;
                        powerAcc1 = 0;
                        
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
            baseCtrl.catched_ball = other;
            baseCtrl.in_catcher_zone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         // if we enteracting with ball
        if (other.GetType().IsAssignableFrom(typeof(UnityEngine.SphereCollider))) {
            baseCtrl.in_catcher_zone = false;
            baseCtrl.catched_ball = null;
        }
       
    }

}
