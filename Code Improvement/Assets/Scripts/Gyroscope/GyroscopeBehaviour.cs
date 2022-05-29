using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GyroscopeBehaviour : MonoBehaviour
{

    private bool gyroEnabled;
    private UnityEngine.Gyroscope gyro;
    //private UnityEngine.InputSystem.Gyroscope gyro;

    float MaxAngle = 35;

    // Minimum and Maximum speed defined
    float MinSpeed = 0.05f;
    float MaxSpeed = 0.2f;

    //private Rigidbody _body;
    private CharacterController _body;
    private Quaternion quatRot;

    //acceleration time, timer and current gravity (which works as an angle) defined
    private float accelerationTime = 3;
    private float accelerationTimer = 0;
    private Vector3 currentGravity = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        //_body = GetComponent<Rigidbody>();
        _body = GetComponent<CharacterController>();
        gyroEnabled = EnabledGryo();
    }

    private bool EnabledGryo()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Portrait;
        Debug.Log(LightSensor.current);

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            //gyro = UnityEngine.InputSystem.Gyroscope.current;
            gyro.enabled = true;
            // Debug.LogError("Enabled");

            //If gyroscope is supported, currentGravity = gyro.gravity
            currentGravity = gyro.gravity;
        }


        return false;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 rotation = new Vector3();
        if (gyro != null)
        {
            //adds the current accelerationTimer with the difference in time, and makes it the new accelerationTimer
            accelerationTimer += Time.deltaTime;

            //Calculates the angle differences between the currentGravity and gyro.gravity, returning a negative percentage difference in the angles.
            float anglePercentage = Vector3.Angle(currentGravity, gyro.gravity) / 180 * (-1);

            //ccelerationTimer = accelerationTimer + accelerationTime * anglePercentage
            accelerationTimer += accelerationTime * anglePercentage;

            //Timer for acceleration from 0-3 (defined earlier) 
            if (accelerationTimer > accelerationTime)
                accelerationTimer = accelerationTime;
            else if(accelerationTimer < 0)
                accelerationTimer = 0;

            //Prints the current value of accelerationTimer (if it prints 3, the player moves at max speed)
            Debug.Log(accelerationTimer);
                
            rotation = transform.right * gyro.gravity.y
                        +transform.forward * -gyro.gravity.x;

            //moves the player at minimum speed, and adds an acceleration to the minimum speed.
            rotation = rotation.normalized * (MinSpeed + (MaxSpeed - MinSpeed) * accelerationTimer/accelerationTime );

            // Debug.Log("X: " + rotation.x + ", Y" + rotation.y);
            // Debug.Log(rotation);
            currentGravity = gyro.gravity;
        }

        //if (Mathf.Abs(_body.velocity.magnitude) < speed)
            //_body.velocity += rotation * Time.deltaTime;
            _body.Move(rotation);
        //Debug.Log(_body.velocity.magnitude);
        

    }
}
