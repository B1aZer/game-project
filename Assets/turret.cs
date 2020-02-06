using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject pc = GameObject.Find("PlayerController");
        Vector3 toPlayer = pc.transform.position - this.GetComponent<Rigidbody>().position;
        toPlayer /= 2;
        toPlayer.y += 10;
        this.GetComponent<Rigidbody>().AddForce(toPlayer, ForceMode.Impulse);
        
    }

}
