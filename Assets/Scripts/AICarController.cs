using UnityEngine;

[RequireComponent(typeof(DistanceMeter))]
[RequireComponent(typeof(Rigidbody))]
public class AICarController : CarController
{
    public NeuralNetwork NeuralNetwork { get; set; }
    DistanceMeter DistanceMeter { get; set; }
    Rigidbody RigidBody { get; set; }

    Transform CheckpointTransform { get; set; }

    void Start()
    {
        NeuralNetwork = NeuralNetwork.Load();

        DistanceMeter = GetComponent<DistanceMeter>();
        RigidBody = GetComponent<Rigidbody>();

        CheckpointTransform = null;
    }

    public override void GetInput()
    {
        var left = DistanceMeter.DistanceLeft;
        var front = DistanceMeter.DistanceFront;
        var right = DistanceMeter.DistanceRight;
        var speed = RigidBody.velocity.magnitude;

        var input = new float[] { left, front, right, speed };
        var output = NeuralNetwork.Compute(input);

        horizontalInput = output[0];
        verticalInput = output[1];
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Checkpoint")
            HandleCheckpoint(col);
    }

    private void HandleCheckpoint(Collider col)
    {
        if (CheckpointTransform == null)
        {
            CheckpointTransform = col.gameObject.transform;
            NeuralNetwork.Fitness++;
        }
        else
        {
            var checkpoint = col.gameObject.GetComponentInParent<Checkpoint>();
            var checkpointTransform = col.gameObject.transform;

            if (checkpoint.GetNext(CheckpointTransform) == checkpointTransform)
            {
                CheckpointTransform = checkpointTransform;
                NeuralNetwork.Fitness++;
            }
        }
    }
}
