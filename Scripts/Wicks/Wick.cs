using UnityEngine;
using UnityEngine.UI;

public class Wick : MonoBehaviour
{
    private WicksFlyweight flyweight;
    public virtual WicksFlyweight Flyweight
    {
        get => flyweight;
        set
        {
            flyweight = value;

            if (flyweight != null)
            {
                flyweight.RemainingWicks++;
                GetComponentInChildren<Image>().sprite = flyweight.Sprite;
            }
        }
    }

    public virtual string FireWorkSoundEventName => Flyweight.EventEmitter.Event;

    [SerializeField]
    [Range(0f, 100f)]
    private float decibelsIncrementPercentage;
    public float DecibelsIncrementPercentage
    {
        get => decibelsIncrementPercentage;
        set => decibelsIncrementPercentage = value;
    }

    private Feedback feedback;
    public float FeedbackDelayInSeconds
    {
        get
        {
            if (feedback == null)
                feedback = GetComponent<Feedback>();

            return feedback.FeedbackDelay;
        }
    }

    protected void Awake() => enabled = false;

    protected void Update() => Flyweight.Move(transform);

    public virtual void Dispatch()
    {
        if (Flyweight == null)
            return;

        Flyweight.OnWickFired();

        ReturnToPool();

        void ReturnToPool()
        {
            ContinuousWickPooling wickGenerator = GetWickGenerator();

            enabled = false;
            Flyweight = null;

            if (wickGenerator != null)
                wickGenerator.WickPool.ReturnToPool(gameObject);
        }
    }


    protected virtual ContinuousWickPooling GetWickGenerator()
    {
        return Flyweight.GetComponentInParent<BoundedContinuousWickPooling>();
    }
}
