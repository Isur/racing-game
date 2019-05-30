using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    [Range(1, 10)]
    public int LapsCount;
    public Checkpoint Checkpoint;
    public SoundController SoundController;
    public TextMeshProUGUI CountdownTextMesh;
    public TextMeshProUGUI LapTextMesh;
    public TextMeshProUGUI FinishMesh;

    private int RaceCheckpointCount { get; set; }
    private CarController[] Cars { get; set; }
    private int FinishedCarsCount { get; set; }

    private NumberEnding NumberEnding { get; set; } = new NumberEnding();

    void Start()
    {
        SetupCars();
        StartCoroutine(LateStart(0.5f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetupCheckpoints();
        StartCoroutine(Countdown());
    }

    private void SetupCheckpoints()
    {
        var trackCheckpointCount = Checkpoint.CheckpointsCount;
        RaceCheckpointCount = trackCheckpointCount * LapsCount;
    }

    private IEnumerator Countdown()
    {
        for (var i = 3; i > 0; i--)
        {
            CountdownTextMesh.text = $"{i}";
            yield return new WaitForSeconds(1);
        }
        CountdownTextMesh.text = string.Empty;
        StartRace();
    }

    private void StartRace()
    {
        LapTextMesh.text = GetLapsInTotal(1);
        foreach (var car in Cars)
            car.enabled = true;
    }

    private void SetupCars()
    {
        Cars = GameObject.FindGameObjectsWithTag("Car")
                .Select(x => x.GetComponent<CarController>())
                .ToArray();

        foreach (var car in Cars)
        {
            car.OnCheckpointEnter += delegate { HandleCarOnCheckpointEnter(car); };
            car.enabled = false;
        }
    }

    private void HandleCarOnCheckpointEnter(CarController car)
    {
        if (car.CheckpointEnteredCount == RaceCheckpointCount)
        {
            // Autko skończyło wyścig
            FinishedCarsCount++;
            car.gameObject.SetActive(false);

            if (!(car is AICarController))
            {
                if (FinishedCarsCount == 1)
                    SoundController.PlayWin();
                else
                    SoundController.PlayLoose();

                FinishMesh.text = $"Congratulations you took {FinishedCarsCount}{NumberEnding.Calculate(FinishedCarsCount)} place!";
                StartCoroutine(GoBackToMenu());
            }
        }
        else
        {
            if (!(car is AICarController))
            {
                // Autko jest graczem i można odświeżyć numer okrążenia
                var trackCheckpointCount = Checkpoint.CheckpointsCount;
                var lap = car.CheckpointEnteredCount / trackCheckpointCount + 1;
                LapTextMesh.text = GetLapsInTotal(lap);
            }
        }
    }

    private IEnumerator GoBackToMenu()
    {
        yield return new WaitForSeconds(5);
        Initiate.Fade("MainMenu", Color.black, 1.0f);
    }

    private string GetLapsInTotal(int lap) { return $"Laps: {lap}/{LapsCount}"; }
}
