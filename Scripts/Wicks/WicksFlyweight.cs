using FMODUnity;
using UnityEngine;

public class WicksFlyweight : MonoBehaviour
{
    public FireWorksManager Manager { protected get; set; }

    [SerializeField, Range(1f, 3f)]
    protected float maxAllowedDiffBetweenSpeedAndGenerationDelay = 1.9f;

    [SerializeField, Range(0.01f, 40f)]
    protected float speed = 1;
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;

            ReduceDelayBetweenGenerationsIfNeeded();

            void ReduceDelayBetweenGenerationsIfNeeded()
            {
                var generator = transform.parent.GetComponent<BoundedContinuousWickPooling>();

                if (generator == null)
                    return;

                float speedPerGenerationRate = speed / generator.DelayBetweenGenerations;

                if (speedPerGenerationRate > maxAllowedDiffBetweenSpeedAndGenerationDelay)
                    generator.DelayBetweenGenerations *= 0.5f;
            }
        }
    }

    public float SizeRatio { get; set; } = 1f;

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }

    [SerializeField]
    private StudioEventEmitter eventEmitter;
    public StudioEventEmitter EventEmitter { get => eventEmitter; set => eventEmitter = value; }

    [SerializeField]
    protected int remainingWicks;
    public virtual int RemainingWicks { get => remainingWicks; set => remainingWicks = value; }

    private Animator animator;
    public Animator Animator
    {
        get
        {
            if (animator == null)
                animator = transform.parent.GetComponentInChildren<Animator>();

            return animator;
        }
    }

    public virtual bool IsMobileVibrationActive { get; set; }

    public virtual void OnWickFired() => RemainingWicks--;

    void Update() => RemainingWicks = GetComponentsInChildren<Wick>().Length;

    public void Move(Transform wick) => wick.Translate(Vector3.down * speed * Time.deltaTime);

    public void EnableWicksMovement()
    {
        if (RemainingWicks == 0)
            return;

        ReferenceWicks();

        foreach (Wick wick in GetComponentsInChildren<Wick>())
            wick.enabled = true;

        void ReferenceWicks()
        {
            Wick[] wicks = GetComponentsInChildren<Wick>();

            foreach (Wick wick in wicks)
                wick.Flyweight = this;

            RemainingWicks = wicks.Length;
        }
    }
}
