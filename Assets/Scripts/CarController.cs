using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;

    public WheelCollider frontRightCollider, frontLeftCollider, rearRightCollider, rearLeftCollider;
    public Transform frontRightTransform, frontLeftTransform, rearRightTransform, rearLeftTransform;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        frontRightCollider.steerAngle = steeringAngle;
        frontLeftCollider.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        frontLeftCollider.motorTorque = verticalInput * motorForce;
        frontRightCollider.motorTorque = verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPoses(frontRightCollider, frontRightTransform);
        UpdateWheelPoses(frontLeftCollider, frontLeftTransform);

        UpdateWheelPoses(rearRightCollider, rearRightTransform);
        UpdateWheelPoses(rearLeftCollider, rearLeftTransform);
    }

    private void UpdateWheelPoses(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;
        _collider.GetWorldPose(out _pos, out _quat);
        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }


}
