using UnityEngine;

public class CarController : MonoBehaviour
{
    protected float horizontalInput;
    protected float verticalInput;
    private float steeringAngle;
    private bool breaking = false;

    public WheelCollider frontRightCollider, frontLeftCollider, rearRightCollider, rearLeftCollider;
    public Transform frontRightTransform, frontLeftTransform, rearRightTransform, rearLeftTransform;
    public float maxSteerAngle = 30;
    public float motorForce = 80;
    public float breakingForce = 40;

    public virtual void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        breaking = Input.GetKey(KeyCode.Space);
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        frontRightCollider.steerAngle = steeringAngle;
        frontLeftCollider.steerAngle = steeringAngle;
    }

    private void Brake()
    {
        if (breaking)
        {
            breakingForce = 40;
        }
        else
        {
            breakingForce = 0;
        }
        frontLeftCollider.brakeTorque = breakingForce;
        frontRightCollider.brakeTorque = breakingForce;
        rearLeftCollider.brakeTorque = breakingForce;
        rearRightCollider.brakeTorque = breakingForce;
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
        //Debug.Log(breaking);
        GetInput();
        Steer();
        Accelerate();
        Brake();
        UpdateWheelPoses();
    }


}
