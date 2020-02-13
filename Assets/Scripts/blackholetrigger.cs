using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackholetrigger : MonoBehaviour
{
    public BaseLogic baseCtrl;
    public int vacuum_mass = 1;
    const float G = 7;
    private Color oldColor;
    // Start is called before the first frame update
    void Awake()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // if we enteracting with ball
        if (other.GetType() == typeof(SphereCollider)) {
            baseCtrl.in_vacuum_zone = true;
            oldColor = other.gameObject.GetComponent<Renderer>().material.color;
            other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetType() == typeof(SphereCollider)) {
            if (!baseCtrl.balls_in_vacuum_zone.Contains(other)) {
                baseCtrl.balls_in_vacuum_zone.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
         // if we enteracting with ball
        if (other.GetType().IsAssignableFrom(typeof(UnityEngine.SphereCollider))) {
            baseCtrl.in_vacuum_zone = false;
            other.gameObject.GetComponent<Renderer>().material.color = oldColor;
            baseCtrl.balls_in_vacuum_zone.Remove(other);
        }
       
    }
}
