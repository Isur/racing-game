using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Klasa ma ustawiać autka na pozycjach startowych.
/// Dać im sobie jeździć przez X czasu.
/// Wybrać najlepszą sieć neuronową z danej iteracji.
/// Usunąć wszystkie autka.
/// Ustawić nowe autka z kopiami (zmutowanymi) najlepszej sieci.
/// </summary>
public class TrainingManager : MonoBehaviour
{
    public GameObject CarPrefab;
    public int NumberOfIterations;
    public int SecondsPerIteration;

    private int Iteration { get; set; } = 0;
    private List<Vector3> StartingPositions { get; set; } = new List<Vector3>();
    private List<GameObject> Cars { get; set; } = new List<GameObject>();
    private List<NeuralNetwork> Networks { get; set; } = new List<NeuralNetwork>();
    private NeuralNetwork BestNetwork { get; set; }
    private bool IsIterating { get; set; } = false;

    private readonly int[] layers = new int[] { 4, 3, 3, 2 };


    void Start()
    {
        StartingPositions = GetStartingPositions();
        Time.timeScale = 20;
    }

    private List<Vector3> GetStartingPositions()
    {
        var result = new List<Vector3>();

        var start = 40f;
        var end = 60f;
        var count = 10f;
        var range = end - start;
        var bit = range / count;

        for (var i = 0; i < count; i++)
            result.Add(new Vector3(start + i * bit, 0f, 41.53f));

        return result;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (BestNetwork != null)
                BestNetwork.Save();
        }

        if (!IsIterating && Iteration < NumberOfIterations)
        {
            IsIterating = true;

            if (Iteration == 0)
                InitiateTraining();
            else
                SetupIteration();

            StartCoroutine(EndIteration());
        }
    }

    private void InitiateTraining()
    {
        foreach (var position in StartingPositions)
        {
            var car = Instantiate(CarPrefab, position, Quaternion.Euler(0, 0, 0));
            Cars.Add(car);

            var network = new NeuralNetwork(layers);
            var carController = car.GetComponent<AICarController>();
            carController.NeuralNetwork = network;
            Networks.Add(network);
        }
    }

    private void SetupIteration()
    {
        foreach (var position in StartingPositions)
        {
            var car = Instantiate(CarPrefab, position, Quaternion.Euler(0, 0, 0));
            Cars.Add(car);

            var network = new NeuralNetwork(BestNetwork);
            network.Mutate();

            var carController = car.GetComponent<AICarController>();
            carController.NeuralNetwork = network;
            Networks.Add(network);
        }
    }

    private IEnumerator EndIteration()
    {
        yield return new WaitForSeconds(SecondsPerIteration);

        Networks.Sort();
        var currentBestNetwork = Networks.Last();
        if (BestNetwork == null)
            BestNetwork = currentBestNetwork;
        else if (BestNetwork.Fitness < currentBestNetwork.Fitness)
            BestNetwork = currentBestNetwork;

        foreach (var car in Cars)
        {
            Destroy(car);
        }

        Cars.Clear();
        Networks.Clear();

        if (BestNetwork != null)
            Debug.Log(BestNetwork.Fitness);

        Iteration++;
        IsIterating = false;

        if (Iteration == NumberOfIterations)
            BestNetwork.Save();
    }
}
