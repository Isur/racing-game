using UnityEngine;

public class DistanceMeter : MonoBehaviour
{
    public Vector3 StartPosition;

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
        Vector3 frontSensorStartPosition = transform.position + StartPosition;
        Vector3 leftSensorStartPosition = transform.position + StartPosition;
        Vector3 rightSensorStartPosition = transform.position + StartPosition;

        DistanceFront = GetDistance(frontSensorStartPosition, 0);
        DistanceLeft = GetDistance(leftSensorStartPosition, -SensorAngle);
        DistanceRight = GetDistance(rightSensorStartPosition, SensorAngle);
    }

    private float GetDistance(Vector3 sensorStartPosition, float sensorAngle)
    {
        RaycastHit hit;

        if (Physics.Raycast(sensorStartPosition, Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward, out hit, SensorLength))
        {
            var distance = Vector3.Distance(sensorStartPosition, hit.point);
            var color = Color.red;

            if (hit.collider.gameObject.tag == "Car")
            {
                distance *= -1;
                color = Color.blue;
            }

            Debug.DrawLine(sensorStartPosition, hit.point, color);
            return distance;
        }

        return SensorLength;
    }
}
