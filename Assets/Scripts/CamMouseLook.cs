using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {
    /*
        public enum RotationAxis
        {
            MouseX = 1,
            MouseY = 2
        }
        public RotationAxis axes = RotationAxis.MouseX;

        public float minimumVert = -45.0f;
        public float maximumVert = 45.0f;

        public float X_Dir = 5.0f;
        public float Y_Dir = 5.0f;

        public float _rotationX = 0;

        // Update is called once per frame
        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            if (axes == RotationAxis.MouseX)
            {
                //rorate X direction
                transform.Rotate(0, mouseX * X_Dir, 0);
            }
            else if (axes == RotationAxis.MouseY)
            {
                //change in the Y direction
                _rotationX += mouseY * Y_Dir;
                //Clamps the vertical angle within the min and max limits (45 degrees)
                _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert); 
                float rotationY = transform.localEulerAngles.y;

                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
            }
        }*/
    public enum RotationAxis
    {
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxis axes = RotationAxis.MouseX;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    public float sensHorizontal = 10.0f;
    public float sensVertical = 10.0f;

    public float _rotationX = 0;

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxis.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensHorizontal, 0);
        }
        else if (axes == RotationAxis.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensVertical;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert); //Clamps the vertical angle within the min and max limits (45 degrees)

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
    }
}
