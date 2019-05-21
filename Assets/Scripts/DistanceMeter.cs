using UnityEngine;

public class DistanceMeter : MonoBehaviour
{
    public float SensorAngle = 45f;
    public float SensorLength = 1000f;

    public float DistanceFront { get; private set; }
    public float DistanceLeft { get; private set; }
    public float DistanceRight { get; private set; }


    void Update()
    {
        GetDistances();
    }

    private void GetDistances()
    {
        RaycastHit frontHit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 frontSensorStartPosition = transform.position;
        Vector3 leftSensorStartPosition = transform.position;
        Vector3 rightSensorStartPosition = transform.position;

        if (Physics.Raycast(frontSensorStartPosition, Quaternion.AngleAxis(0, transform.up) * transform.forward, out frontHit, SensorLength))
        {
            DistanceFront = Vector3.Distance(frontSensorStartPosition, frontHit.point);
            Debug.DrawLine(frontSensorStartPosition, frontHit.point);
        }

        if (Physics.Raycast(leftSensorStartPosition, Quaternion.AngleAxis(-SensorAngle, transform.up) * transform.forward, out leftHit, SensorLength))
        {
            DistanceLeft = Vector3.Distance(leftSensorStartPosition, leftHit.point);
            Debug.DrawLine(leftSensorStartPosition, leftHit.point);
        }

        if (Physics.Raycast(rightSensorStartPosition, Quaternion.AngleAxis(SensorAngle, transform.up) * transform.forward, out rightHit, SensorLength))
        {
            DistanceRight = Vector3.Distance(rightSensorStartPosition, rightHit.point);
            Debug.DrawLine(rightSensorStartPosition, rightHit.point);
        }
    }
}
