using System.Collections;
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
