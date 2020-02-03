using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLogic : MonoBehaviour
{
    public bool in_catcher_zone = false;
    public bool in_vacuum_zone = false;
    //public Collider 
    public Collider catched_ball;
    public Collider locked_ball;
    public Collider catcher;
    public GameObject FirstCam;
    public GameObject ThirdCam;

    const float G = 7;

    public int vacuum_mass = 1;

    private int active_camera = 3;

    void Awake()
    {
        GameObject go = GameObject.Find ("Catcher");
        catcher = go.GetComponent <CapsuleCollider> ();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void ChangeCams(int cam_num) {
        if (cam_num != active_camera) {
     
            StartCoroutine(setCamera());

        }
       
    }

    private IEnumerator setCamera() {
            yield return new WaitForSeconds(.1f);
            if (active_camera == 1) {
                FirstCam.SetActive(false);  
                ThirdCam.SetActive(true);
                active_camera = 3;
            } else if (active_camera == 3) {
                FirstCam.SetActive(true);
                ThirdCam.SetActive(false);  
                active_camera = 1;        
            }
    }

    public void DetractFrom(Collider vacuum, Collider ball, int power = 1) {
        //TODO: change
        Transform rbToAttract = vacuum.transform;
        Rigidbody rbBall = ball.attachedRigidbody;
        Vector3 direction = -(rbToAttract.position - rbBall.position);
        float distance = direction.magnitude;
        float forcemagintude = power/10 * G * (vacuum_mass * rbBall.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forcemagintude;
        rbBall.AddForce(force);
    }

    public void drawProjectile(Rigidbody rigidbody, Vector3 force, float drag, float timeToPredict, float frameTime) {

        float maxIterations = timeToPredict / frameTime;
        Vector3 point1 = rigidbody.position;
        Vector3 point2;
        Vector3 velocity = force / rigidbody.mass;
        Vector3 gravity_coef = Physics.gravity * frameTime;

        for(float i = 0; i < maxIterations; i++) {
           
            //velocity *= Mathf.Clamp01(1.0f - (drag * frameTime));
            velocity *= 1.0f - (drag * frameTime);

            velocity += Physics.gravity * frameTime;
    
            point2 = point1 + velocity * frameTime;
         
            Debug.DrawLine(point1, point2, i % 2 == 0 ? Color.yellow : Color.green, frameTime);

            point1 = point2;
          
        }

    }

}
