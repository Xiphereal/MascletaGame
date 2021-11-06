using UnityEngine;
using System.Collections;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class BackgroundCrowd : MonoBehaviour
{
    private StudioEventEmitter eventEmitter;

    private void Awake()
    {
        eventEmitter = GetComponent<StudioEventEmitter>();
        DontDestroyOnLoad(gameObject);
    }

    public void OnPause() => eventEmitter.SetParameter("Pause", 1f);

    public void OnResume() => eventEmitter.SetParameter("Pause", 0f);
}
