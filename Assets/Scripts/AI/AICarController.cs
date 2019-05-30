using UnityEngine;

[RequireComponent(typeof(DistanceMeter))]
public class AICarController : CarController
{
    public NeuralNetwork NeuralNetwork { get; set; }
    DistanceMeter DistanceMeter { get; set; }

    protected override void Start()
    {
        base.Start();

        if (NeuralNetwork == null)
            NeuralNetwork = NeuralNetwork.Load();

        DistanceMeter = GetComponent<DistanceMeter>();

        OnCheckpointEnter += delegate { NeuralNetwork.Fitness++; };
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
}
