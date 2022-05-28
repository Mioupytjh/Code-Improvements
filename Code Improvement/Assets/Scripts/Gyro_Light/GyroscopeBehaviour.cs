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
    float MinSpeed = 0.05f;
    float MaxSpeed = 0.2f;
    //private Rigidbody _body;
    private CharacterController _body;
    private Quaternion quatRot;
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
            accelerationTimer += Time.deltaTime;

            float anglePercentage = Vector3.Angle(currentGravity, gyro.gravity) / 180 * (-1);

            accelerationTimer += accelerationTime * anglePercentage;

            if (accelerationTimer > accelerationTime)
                accelerationTimer = accelerationTime;
            else if(accelerationTimer < 0)
                accelerationTimer = 0;

            Debug.Log(accelerationTimer);
                

            rotation = transform.right * gyro.gravity.y
                        +transform.forward * -gyro.gravity.x;

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
