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

    const float G = 7;

    public int vacuum_mass = 1;

    void Awake()
    {
        GameObject go = GameObject.Find ("Catcher");
        catcher = go.GetComponent <CapsuleCollider> ();
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

}
