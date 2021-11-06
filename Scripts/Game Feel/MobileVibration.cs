using System;
using System.Threading.Tasks;
using UnityEngine;

public class MobileVibration : MonoBehaviour
{
    public static MobileVibration Instance { get; private set; }

    private void Awake()
    {
        EnsureSingleton();

        void EnsureSingleton()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
    }

    private void Start() => ExplodeDetector.OnAnyWickFired += Vibrate;

    private void Vibrate(Wick wick)
    {
        if (wick.Flyweight.IsMobileVibrationActive)
            if (MustFeedbackBeDelayed())
                DelayVibration();
            else
                Handheld.Vibrate();

        async void DelayVibration()
        {
            await Task.Delay(TimeSpan.FromSeconds(wick.FeedbackDelayInSeconds));

            Handheld.Vibrate();
        }
        bool MustFeedbackBeDelayed()
        {
            return wick is Trueno ? (wick.Flyweight as TruenoFlyweight).IsFeedbackDelayed : wick.FeedbackDelayInSeconds > 0f;
        }
    }

    private void OnDestroy()
    {
        CleanStaticReferenceAtSelfDispose();

        void CleanStaticReferenceAtSelfDispose()
        {
            if (Instance == this)
            {
                ExplodeDetector.OnAnyWickFired -= Vibrate;
                Instance = null;
            }
        }
    }
}
