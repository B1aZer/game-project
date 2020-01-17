using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    private BaseLogic baseCtrl;

    void Awake()
    {
        GameObject go = GameObject.Find ("Base");
        baseCtrl = go.GetComponent<BaseLogic>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         if (baseCtrl.in_catcher_zone) {
            if (Input.GetMouseButton(0)) {
                SphereCollider[] balls = FindObjectsOfType<SphereCollider>();
                CapsuleCollider catcher = FindObjectOfType<CapsuleCollider>();
                foreach(SphereCollider ball in balls) {
                    ball.attachedRigidbody.velocity = Vector3.zero;
                    // Probably dont' need, check
                    ball.attachedRigidbody.angularVelocity = Vector3.zero;
                    ball.transform.SetParent(catcher.transform);
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
            Debug.Log("Cathinggg!!");
            baseCtrl.in_catcher_zone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         // if we enteracting with ball
        if (other.GetType().IsAssignableFrom(typeof(UnityEngine.SphereCollider))) {
            baseCtrl.in_catcher_zone = false;
        }
       
    }

    public void Cathed() {

    }
}
