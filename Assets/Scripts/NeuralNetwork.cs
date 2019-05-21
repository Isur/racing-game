using System;
using System.Collections.Generic;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private static NeuralNetworkHelper Helper { get; } = new NeuralNetworkHelper();

    private int[] Layers { get; set; } //layers
    private float[][] Neurons { get; set; } //neuron matix
    private float[][][] Weights { get; set; } //weight matrix

    public float Fitness { get; set; } = 0; //fitness of the network
    public Func<float, float> ActivationFunction { get; set; } = (x) => (float)Math.Tanh(x); //activation function


    /// <summary>
    /// Initilizes and neural network with random weights
    /// </summary>
    /// <param name="layers">layers to the neural network</param>
    public NeuralNetwork(int[] layers)
    {
        //deep copy of layers of this network 
        this.Layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.Layers[i] = layers[i];
        }


        //generate matrix
        InitNeurons();
        InitWeights();
    }

    /// <summary>
    /// Deep copy constructor 
    /// </summary>
    /// <param name="copyNetwork">Network to deep copy</param>
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.Layers = new int[copyNetwork.Layers.Length];
        for (int i = 0; i < copyNetwork.Layers.Length; i++)
        {
            this.Layers[i] = copyNetwork.Layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.Weights);

        this.ActivationFunction = copyNetwork.ActivationFunction;
    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < Weights.Length; i++)
        {
            for (int j = 0; j < Weights[i].Length; j++)
            {
                for (int k = 0; k < Weights[i][j].Length; k++)
                {
                    Weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    /// <summary>
    /// Create neuron matrix
    /// </summary>
    private void InitNeurons()
    {
        //Neuron Initilization
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < Layers.Length; i++) //run through all layers
        {
            neuronsList.Add(new float[Layers[i]]); //add layer to neuron list
        }

        Neurons = neuronsList.ToArray(); //convert list to array
    }

    /// <summary>
    /// Create weights matrix.
    /// </summary>
    private void InitWeights()
    {

        List<float[][]> weightsList = new List<float[][]>(); //weights list which will later will converted into a weights 3D array

        //itterate over all neurons that have a weight connection
        for (int i = 1; i < Layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>(); //layer weight list for this current layer (will be converted to 2D array)

            int neuronsInPreviousLayer = Layers[i - 1];

            //itterate over all neurons in this current layer
            for (int j = 0; j < Neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //neruons weights

                //itterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //give random weights to neuron weights
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights); //add neuron weights of this current layer to layer weights
            }

            weightsList.Add(layerWeightsList.ToArray()); //add this layers weights converted into 2D array into weights list
        }

        Weights = weightsList.ToArray(); //convert to 3D array
    }

    /// <summary>
    /// Feed forward this neural network with a given input array
    /// </summary>
    /// <param name="inputs">Inputs to network</param>
    /// <returns></returns>
    public float[] Compute(float[] inputs)
    {
        //Add inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            Neurons[0][i] = inputs[i];
        }

        //itterate over all neurons and compute feedforward values 
        for (int i = 1; i < Layers.Length; i++)
        {
            for (int j = 0; j < Neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < Neurons[i - 1].Length; k++)
                {
                    value += Weights[i - 1][j][k] * Neurons[i - 1][k]; //sum off all weights connections of this neuron weight their values in previous layer
                }

                Neurons[i][j] = ActivationFunction.Invoke(value); // Activation
            }
        }

        return Neurons[Neurons.Length - 1]; //return output layer
    }

    /// <summary>
    /// Mutate neural network weights
    /// </summary>
    public void Mutate()
    {
        for (int i = 0; i < Weights.Length; i++)
        {
            for (int j = 0; j < Weights[i].Length; j++)
            {
                for (int k = 0; k < Weights[i][j].Length; k++)
                {
                    float weight = Weights[i][j][k];

                    //mutate weight value 
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumber <= 2f)
                    { //if 1
                      //flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    { //if 2
                      //pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { //if 3
                      //randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { //if 4
                      //randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    Weights[i][j][k] = weight;
                }
            }
        }
    }

    /// <summary>
    /// Saves the network to a file to recreate it later
    /// </summary>
    public void Save()
    {
        var model = ToModel();
        Helper.Save(model);
    }

    /// <summary>
    /// Loades the network from existing save file
    /// </summary>
    public void Load()
    {
        var model = Helper.Load();
        Layers = model.Layers;
        Neurons = model.Neurons;
        Weights = model.Weights;
        ActivationFunction = model.ActivationFunction;
    }

    /// <summary>
    /// Returns pure DTO model of this neural network
    /// </summary>
    /// <returns></returns>
    private NeuralNetworkModel ToModel()
    {
        return new NeuralNetworkModel(Layers, Neurons, Weights, ActivationFunction);
    }

    /// <summary>
    /// Compare two neural networks and sort based on fitness
    /// </summary>
    /// <param name="other">Network to be compared to</param>
    /// <returns></returns>
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (Fitness > other.Fitness)
            return 1;
        else if (Fitness < other.Fitness)
            return -1;
        else
            return 0;
    }
}
