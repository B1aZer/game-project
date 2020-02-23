using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePhysics : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public static void startSpinForCoordinates(Vector3 mp) {

            //baseCtrl.locked_ball.attachedRigidbody.AddTorque(mp, ForceMode.Impulse);
            //locked_ball.transform.Rotate( new Vector3(0, 1, 1) * powerAcc0 * Time.deltaTime); 
    }

    public static void addVelocity(Rigidbody rb, Vector3 velocity) {
        rb.velocity = velocity;
    }

    // testo
    //if (ball_released) {
        // Curve
        //baseCtrl.catched_ball.attachedRigidbody.AddRelativeForce (Vector3.left*1.088f, ForceMode.Impulse);         
    //}

}
