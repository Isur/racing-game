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
    [Header("General")]
    public int NumberOfIterations;
    public int SecondsPerIteration;
    public KeyCode SaveButton = KeyCode.S;
    public float TimeScale = 1;
    public bool UseCurrentBestNetwork;

    [Header("Spawn")]
    public GameObject CarPrefab;
    public Vector3 Center;
    public Vector3 Size;
    public Vector3 StartRotation;
    public int AgentsCount;
    //public int Rows;

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
    }

    private List<Vector3> GetStartingPositions()
    {
        var result = new List<Vector3>();

        var start = Center.x - Size.x / 2;
        var end = Center.x + Size.x / 2;
        var range = end - start;
        var bit = range / AgentsCount;

        for (var i = 0; i < AgentsCount; i++)
            result.Add(new Vector3(start + i * bit, Center.y, Center.z));

        return result;
    }

    void Update()
    {
        Time.timeScale = TimeScale;

        if (Input.GetKeyDown(SaveButton))
        {
            if (BestNetwork != null)
                BestNetwork.Save();
        }

        if (!IsIterating && Iteration < NumberOfIterations)
        {
            IsIterating = true;

            var isFirstIteration = Iteration == 0;
            SetupIteration(isFirstIteration);

            StartCoroutine(EndIteration());
        }
    }

    private void SetupIteration(bool first)
    {
        StartingPositions = GetStartingPositions();
        foreach (var position in StartingPositions)
        {
            var car = Instantiate(CarPrefab, position, Quaternion.Euler(StartRotation));
            Cars.Add(car);

            NeuralNetwork network;
            if (first)
            {
                if (UseCurrentBestNetwork)
                    network = NeuralNetwork.Load();
                else
                    network = new NeuralNetwork(layers);
            }
            else
            {
                network = new NeuralNetwork(BestNetwork);
                network.Mutate();
            }

            var carController = car.GetComponent<AICarController>();
            carController.NeuralNetwork = network;
            Networks.Add(network);
        }
    }

    private IEnumerator EndIteration()
    {
        yield return new WaitForSeconds(SecondsPerIteration);

        GetBestNetwork();
        Clear();


        if (BestNetwork != null)
            Debug.Log(BestNetwork.Fitness);

        Iteration++;
        IsIterating = false;

        if (Iteration == NumberOfIterations)
        {
            BestNetwork.Save();
            Time.timeScale = 1;
        }
    }

    private void GetBestNetwork()
    {
        Networks.Sort();
        var currentBestNetwork = Networks.Last();
        if (BestNetwork == null)
            BestNetwork = currentBestNetwork;
        else if (BestNetwork.Fitness < currentBestNetwork.Fitness)
            BestNetwork = currentBestNetwork;
    }

    private void Clear()
    {
        foreach (var car in Cars)
        {
            Destroy(car);
        }
        Cars.Clear();
        Networks.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Center, Size);
    }
}
