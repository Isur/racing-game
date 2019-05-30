using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<Transform> Checkpoints { get; set; } = new List<Transform>();
    public int CheckpointsCount { get { return Checkpoints.Count; } }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            Checkpoints.Add(t);
            t.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    public Transform GetNext(Transform current)
    {
        var currentIndex = Checkpoints.IndexOf(current);

        if (currentIndex == Checkpoints.Count - 1)
            return Checkpoints[0];

        return Checkpoints[currentIndex + 1];
    }
}
