using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AverageDecibelsMeter : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 100f)]
    protected float initialValue;

    [SerializeField]
    [Range(0f, 100f)]
    protected float decrementByElapsedTimePercentage;

    [SerializeField]
    protected int minRealDecibels;
    [SerializeField]
    protected int maxRealDecibels;

    protected Slider meter;

    protected void Awake()
    {
        meter = GetComponent<Slider>();
        meter.value = initialValue;
    }

    private void Start()
    {
        gameObject.SetActive(false);

        ExplodeDetector.OnAnyWickFired += IncrementMeterValueByHit;
    }

    private void IncrementMeterValueByHit(Wick wick)
    {
        if (MustFeedbackBeDelayed())
            DelayMeterIncrement();
        else
            meter.value += wick.DecibelsIncrementPercentage;

        bool MustFeedbackBeDelayed()
        {
            return wick is Trueno ? (wick.Flyweight as TruenoFlyweight).IsFeedbackDelayed : wick.FeedbackDelayInSeconds > 0f;
        }
        async void DelayMeterIncrement()
        {
            await Task.Delay(TimeSpan.FromSeconds(wick.FeedbackDelayInSeconds));

            meter.value += wick.DecibelsIncrementPercentage;
        }
    }

    protected void Update()
    {
        DecreaseValueByElapsedTime();
        NotifyDecibelsToScore();

        void DecreaseValueByElapsedTime() => meter.value -= decrementByElapsedTimePercentage * Time.deltaTime;
        void NotifyDecibelsToScore()
        {
            Score.Instance.AverageDecibels = GetRealDecibelsValue();

            int GetRealDecibelsValue()
            {
                float realDecibelsValue = Mathf.Lerp(minRealDecibels, maxRealDecibels, meter.value / 100);
                return Mathf.RoundToInt(realDecibelsValue);
            }
        }
    }

    private void OnDestroy() => ExplodeDetector.OnAnyWickFired -= IncrementMeterValueByHit;
}
