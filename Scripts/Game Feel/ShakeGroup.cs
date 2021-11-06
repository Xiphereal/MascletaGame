using System;
using System.Threading.Tasks;
using UnityEngine;

public class ShakeGroup : MonoBehaviour
{
    [SerializeField]
    private bool shakeOnWickFired;
    public bool ShakeOnWickFired { get => shakeOnWickFired; set => shakeOnWickFired = value; }

    private ShakeableElement camera;

    private void Awake()
    {
        ExplodeDetector.OnAnyWickFired += Shake;
        camera = Camera.main.GetComponent<ShakeableElement>();

        if (camera == null)
            Debug.LogError("Main camera doesn't have ShakeableElement script", Camera.main);
    }

    [ContextMenu("Shake Group")]
    public void Shake(Wick wick)
    {
        if (ShakeOnWickFired)
        {
            bool isATruenoWick = wick is Trueno;

            if (MustFeedbackBeDelayed(isATruenoWick))
                DelayShake(isATruenoWick);
            else
                ShakeAllElementsInGroup(wick);
        }

        bool MustFeedbackBeDelayed(bool isATruenoWick)
        {
            return isATruenoWick ? (wick.Flyweight as TruenoFlyweight).IsFeedbackDelayed : wick.FeedbackDelayInSeconds > 0f;
        }
        async void DelayShake(bool isATruenoWick)
        {
            await Task.Delay(TimeSpan.FromSeconds(wick.FeedbackDelayInSeconds));

            ShakeAllElementsInGroup(wick);
        }
    }

    public void ShakeAllElementsInGroup(Wick wick)
    {
        float intensity = (wick.DecibelsIncrementPercentage * 60) / 100f;

        foreach (var shakeable in GetComponentsInChildren<ShakeableElement>())
            shakeable.Shake(wick is Trueno, intensity);

        camera?.Shake(wick is Trueno, intensity);
    }

    private void OnDestroy() => ExplodeDetector.OnAnyWickFired -= Shake;
}
