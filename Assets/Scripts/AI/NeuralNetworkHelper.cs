using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

internal class NeuralNetworkHelper
{
    private const string LAYERS_FILE = "layers.txt";
    private const string NEURONS_FILE = "neurons.txt";
    private const string WEIGHTS_FILE = "weights.txt";
    private const string ACTIVATION_FUNCTION_FILE = "activation_function.txt";

    private BinaryFormatter formatter = new BinaryFormatter();

    public void Save(NeuralNetworkModel model)
    {
        save(LAYERS_FILE, model.Layers);
        save(NEURONS_FILE, model.Neurons);
        save(WEIGHTS_FILE, model.Weights);
        save(ACTIVATION_FUNCTION_FILE, model.ActivationFunction);
    }

    private void save(string filename, object obj)
    {
        using (var file = File.OpenWrite(filename))
            formatter.Serialize(file, obj);
    }

    public NeuralNetworkModel Load()
    {
        var layers = load<int[]>(LAYERS_FILE);
        var neurons = load<float[][]>(NEURONS_FILE);
        var weights = load<float[][][]>(WEIGHTS_FILE);
        var activationFunction = load<Func<float, float>>(ACTIVATION_FUNCTION_FILE);
        return new NeuralNetworkModel(layers, neurons, weights, activationFunction);
    }

    private T load<T>(string filename)
        where T : class
    {
        using (var file = File.OpenRead(filename))
            return (T)formatter.Deserialize(file);
    }
}

internal class NeuralNetworkModel
{
    public int[] Layers { get; }
    public float[][] Neurons { get; }
    public float[][][] Weights { get; }
    public Func<float, float> ActivationFunction { get; }

    public NeuralNetworkModel(int[] layers, float[][] neurons, float[][][] weights, Func<float, float> activationFunction)
    {
        Layers = layers;
        Neurons = neurons;
        Weights = weights;
        ActivationFunction = activationFunction;
    }
}
