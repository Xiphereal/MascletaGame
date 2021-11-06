using System;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Wick))]
public class Feedback : MonoBehaviour
{
    private Wick wick;
    private string flareAnimationName;

    [SerializeField]
    private float feedbackDelay;
    public float FeedbackDelay => feedbackDelay;

    private void Awake()
    {
        wick = GetComponent<Wick>();

        flareAnimationName = wick is Trueno ? "TruenoFlare" : "Flare";
    }

    private void Start() => FMODUnity.RuntimeManager.WaitForAllLoads();

    public void Hit()
    {
        FMODUnity.RuntimeManager.PlayOneShot(wick.FireWorkSoundEventName);

        if (wick is Trueno && (wick.Flyweight as TruenoFlyweight).IsFeedbackDelayed)
            DelayedFeedback();
        else
            PlayAnimation();

        async void DelayedFeedback()
        {
            await Task.Delay(TimeSpan.FromSeconds(feedbackDelay));

            PlayAnimation();
        }

        void PlayAnimation()
        {
            wick.Flyweight.Animator.Play(flareAnimationName);
        }
    }
}
