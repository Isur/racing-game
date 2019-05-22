using UnityEngine;

[RequireComponent(typeof(DistanceMeter))]
[RequireComponent(typeof(Rigidbody))]
public class AICarController : CarController
{
    public NeuralNetwork NeuralNetwork { get; set; }
    DistanceMeter DistanceMeter { get; set; }
    Rigidbody RigidBody { get; set; }

    void Start()
    {
        var layers = new int[] { 4, 3, 3, 2 };
        NeuralNetwork = new NeuralNetwork(layers);

        DistanceMeter = GetComponent<DistanceMeter>();
        RigidBody = GetComponent<Rigidbody>();
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
