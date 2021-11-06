using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Falleras : MonoBehaviour
{
    [SerializeField]
    private FireWorksManager fireWorksManager;

    [SerializeField]
    private GameObject fallerasQuote;

    [SerializeField]
    private GameObject tutorial;
    [SerializeField]
    private GameObject mascleta;

    [Header("Timestamps (ms)")]
    [SerializeField]
    private int timeToStartTutorial;
    [SerializeField]
    private int timeToEndTutorial;

    private StudioEventEmitter eventEmitter;

    private void Reset() => eventEmitter = GetComponent<StudioEventEmitter>();

    private void Awake()
    {
        eventEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        StartInitialShout();

        void StartInitialShout()
        {
            eventEmitter.Play();
            fallerasQuote.SetActive(true);
        }
    }

    private void Update()
    {
        int positionInMiliseconds = GetEventPlaybackTimeInMiliseconds();

        CheckForTutorialStart();
        CheckForTutorialEnd();

        int GetEventPlaybackTimeInMiliseconds()
        {
            int position;
            eventEmitter.EventInstance.getTimelinePosition(out position);
            return position;
        }
        void CheckForTutorialStart()
        {
            if (positionInMiliseconds > timeToStartTutorial)
                StartTutorial();
        }
        void CheckForTutorialEnd()
        {
            if (positionInMiliseconds > timeToEndTutorial)
                EndTutorial();
        }
    }

    [ContextMenu("Start Tutorial")]
    public void StartTutorial() => tutorial.SetActive(true);

    [ContextMenu("End Tutorial")]
    public void EndTutorial()
    {
        tutorial.GetComponent<Tutorial>().EndTutorial();
        EndInitialShout();
        StartGame();
        enabled = false;

        void EndInitialShout() => fallerasQuote.SetActive(false);
        void StartGame() => mascleta.SetActive(true);
    }
}
