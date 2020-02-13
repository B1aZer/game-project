using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public GameObject dotIm;
    public GameObject circle;
    // Start is called before the first frame update
    void Start()
    {
        
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
         
    }

    // Update is called once per frame
    void Update()
    {
         Camera mycam = this.GetComponent<Camera>();
 
            float sensitivity = 0.05f;
            Vector3 vp = mycam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane));
            vp.x -= 0.5f;
            vp.y -= 0.5f;
            vp.x *= sensitivity;
            vp.y *= sensitivity;
            vp.x += 0.5f;
            vp.y += 0.5f;
            Vector3 sp = mycam.ViewportToScreenPoint(vp);
            
            Vector3 v = mycam.ScreenToWorldPoint(sp);
            //transform.LookAt(v, Vector3.up);
        
        float dist = Mathf.Sqrt(
            Mathf.Pow((Input.mousePosition.x - circle.transform.position.x), 2) 
            + Mathf.Pow((Input.mousePosition.y - circle.transform.position.y), 2) 
            + Mathf.Pow((Input.mousePosition.z - circle.transform.position.z), 2)
        );
        // contain within cycle
        if (dist <= 250) {
            dotIm.transform.position = Input.mousePosition;
        }
        
        
    }
}
