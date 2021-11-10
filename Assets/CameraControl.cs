using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 10f;
    public float speedRotation = 10f;
    public float minAngleVertical = -60;
    public float maxAngleVertical = 60;

    float angleHorizontal = 0;
    float angleVertical = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        angleHorizontal = transform.rotation.eulerAngles.y;
        angleVertical = transform.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.Normalize(Input.GetAxis("Vertical") * transform.forward
                                                + Input.GetAxis("Horizontal") * transform.right);

        angleHorizontal += speedRotation * Time.deltaTime * Input.GetAxis("Mouse X");
        angleVertical += speedRotation * Time.deltaTime * -Input.GetAxis("Mouse Y");

        angleVertical = Mathf.Clamp(angleVertical, minAngleVertical, maxAngleVertical);

        Quaternion rotY = Quaternion.AngleAxis(angleHorizontal, Vector3.up);
        Quaternion rotX = Quaternion.AngleAxis(angleVertical, Vector3.right);
        transform.rotation = rotY * rotX; 
    }
}
