using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    public Transform orientation;

    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible= false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Mouse Input.
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
