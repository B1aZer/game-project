using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    private BaseLogic baseCtrl;
    private heavy_trajectory baseTraj;

    private bool ball_released = false;
    private bool start_spin = false;

    private bool sim_path = false;

    private int powerAcc0 = 0;
    private int powerAcc1 = 0;

    void Awake()
    {
        GameObject go = GameObject.Find ("Base");
        baseCtrl = go.GetComponent<BaseLogic>();
        GameObject go1 = GameObject.Find ("TrajectoryRenderer");
        baseTraj = go1.GetComponent<heavy_trajectory>();
        Debug.Log(baseTraj);
    }

    // Update is called once per frame
    void Update()
    {

        // testo
        if (baseCtrl.locked_ball && start_spin) {
            baseCtrl.locked_ball.transform.Rotate(0, 5, 5);   
        }

        // testo
        if (ball_released) {
            // what is this ? Curve ?
            //baseCtrl.catched_ball.attachedRigidbody.AddRelativeForce (Vector3.left*1.088f, ForceMode.Impulse);         
        }

        

        if (baseCtrl.in_catcher_zone) {
           
            if (Input.GetMouseButton(0)) {

                powerAcc0++;
                               
                if (baseCtrl.locked_ball) {
                    start_spin = true;                    
                    //baseTraj.simulatePath(baseCtrl.locked_ball.gameObject.transform, 100f);
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
            }

            if (Input.GetMouseButtonUp(1)) {
                
                    if (baseCtrl.catched_ball) {
                        Debug.Log("Fre");
                        baseCtrl.catched_ball.transform.SetParent(null);
                        baseCtrl.catched_ball.GetComponent<SphereCollider>().isTrigger = false;
                        baseCtrl.catched_ball.attachedRigidbody.isKinematic = false;
                        baseCtrl.DetractFrom(baseCtrl.catcher, baseCtrl.catched_ball, powerAcc1);
                    
                        baseCtrl.locked_ball = null;
                        ball_released = true;
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
