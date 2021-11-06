using FMODUnity;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class CrowdReactions : MonoBehaviour
{
    private StudioEventEmitter eventEmitter;

    [SerializeField]
    [Range(1f, 10f)]
    private float secondsUntilCrowdComplains;
    [SerializeField]
    [Range(0.01f, 4f)]
    private float secondsUntilCrowdReactsToTrueno;

    private Coroutine activeCoroutine;

    private void Awake() => eventEmitter = GetComponent<StudioEventEmitter>();

    private void Start() => StartBadReactionCoroutine();

    private void StartBadReactionCoroutine()
    {
        activeCoroutine = StartCoroutine(PlayBadReaction(new WaitForSeconds(secondsUntilCrowdComplains)));

        IEnumerator PlayBadReaction(WaitForSeconds waitForSeconds)
        {
            yield return waitForSeconds;

            PlayCrowdReaction(0);
        }
    }

    public void StartEndOfStageReaction(float hitPercentage)
    {
        StartCoroutine(PlayEndOfStageReaction(new WaitForSeconds(secondsUntilCrowdReactsToTrueno)));

        IEnumerator PlayEndOfStageReaction(WaitForSeconds waitForSeconds)
        {
            yield return waitForSeconds;

            PlayCrowdReaction(hitPercentage);
        }
    }
    
    public void PlayCrowdReaction(float hitPercentage)
    {
        eventEmitter.Play();
        eventEmitter.SetParameter("WinLose", hitPercentage);
    }

    public void RestartCrowdReactionCoroutine()
    {
        StopCoroutine(activeCoroutine);
        StartBadReactionCoroutine();
    }
}
