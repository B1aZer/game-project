﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackholetrigger : MonoBehaviour
{
    public int vacuum_mass = 1;
    const float G = 7;
    private BaseLogic baseCtrl;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject go = GameObject.Find ("Base");
        baseCtrl = go.GetComponent <BaseLogic> ();
    }

    void FixedUpdate()
    {
         if (baseCtrl.in_vacuum_zone) {
             //TODO: disable in cather zone
            if (Input.GetMouseButton(0) && !baseCtrl.in_catcher_zone) {
                
                Collider vacuum_catcher = FindObjectOfType<CapsuleCollider>();
                SphereCollider[] balls = FindObjectsOfType<SphereCollider>();
                Debug.Log("starting weeeeeeeeeee!!");
                foreach(SphereCollider ball in balls) {
                    AttractTo(vacuum_catcher, ball);
                }
                
            }

            if (Input.GetMouseButton(1)) {
                Collider vacuum_catcher = FindObjectOfType<CapsuleCollider>();
                SphereCollider[] balls = FindObjectsOfType<SphereCollider>();                
                foreach(SphereCollider ball in balls) {
                    DetractFrom(vacuum_catcher, ball);
                }
            }
         }
            
    }

    private void AttractTo(Collider vacuum, Collider ball) {
        Debug.Log("------");
        Transform rbToAttract = vacuum.transform;
        //Debug.Log(rbToAttract.mass);
        Debug.Log(rbToAttract.position);
        Rigidbody rbBall = ball.attachedRigidbody;
        //Debug.Log(rbBall.mass);
        Debug.Log(rbBall.position);
        // this is a vector pointing from ball to vacuumer
        Vector3 direction = rbToAttract.position - rbBall.position;
        Debug.Log(direction);
        float distance = direction.magnitude;
        //Debug.Log(distance);
        float forcemagintude = G * (vacuum_mass * rbBall.mass) / Mathf.Pow(distance, 2);
        //Debug.Log(forcemagintude);
        Vector3 force = direction.normalized * forcemagintude;
        Debug.Log(force);
        rbBall.AddForce(force);
        //Debug.Break();
        Debug.Log("------");
    }
    
    private void DetractFrom(Collider vacuum, Collider ball) {
        //TODO: change
        Transform rbToAttract = vacuum.transform;
        Rigidbody rbBall = ball.attachedRigidbody;
        Vector3 direction = -(rbToAttract.position - rbBall.position);
        float distance = direction.magnitude;
        float forcemagintude = G * (vacuum_mass * rbBall.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forcemagintude;
        rbBall.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if we enteracting with ball
        if (other.GetType() == typeof(SphereCollider)) {
            baseCtrl.in_vacuum_zone = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
       
    }

    private void OnTriggerExit(Collider other)
    {
         // if we enteracting with ball
        if (other.GetType().IsAssignableFrom(typeof(UnityEngine.SphereCollider))) {
            baseCtrl.in_vacuum_zone = false;
        }
       
    }
}
