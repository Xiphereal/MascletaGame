using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/New Stage", order = 1)]
public class Stage : ScriptableObject, IState
{
    public Mascleta Context { get; set; }

    [SerializeField]
    private int stageID = 0;
    public int StateID => stageID;
    public string Name => $"Stage{stageID}";

    [SerializeField]
    private ScriptableObject nextState;
    public ScriptableObject NextState => nextState;

    [SerializeField]
    [Range(0.1f, 5f)]
    public float wickSpeed;
    public float WicksSpeed => wickSpeed;

    [SerializeField]
    [Range(0f, 100f)]
    private float decibelsIncrementPercentage;

    [SerializeField]
    [Range(0f, 100f)]
    private float fogExtension;
    public float FogExtension { get => fogExtension; protected set => fogExtension = value; }

    [Header("Game Feel")]
    [SerializeField]
    private bool vibrateOnWickFired;

    [Header("Ultra Low")]
    [SerializeField]
    [Range(1f, 15f)]
    private float maxDistanceBetweenUltraLows = 8;
    [SerializeField]
    [Range(1, 30)]
    private int quantityOfUltraLow = 8;

    [Header("Low")]
    [SerializeField]
    [Range(1f, 15f)]
    private float maxDistanceBetweenLows = 8;
    [SerializeField]
    [Range(1, 30)]
    private int quantityOfLow = 8;

    [Header("Mid")]
    [SerializeField]
    [Range(1f, 15f)]
    private float maxDistanceBetweenMids = 8;
    [SerializeField]
    [Range(1, 30)]
    private int quantityOfMid = 8;

    [Header("Whistle")]
    [SerializeField]
    [Range(1f, 15f)]
    private float maxDistanceBetweenWhistles = 8;
    [SerializeField]
    [Range(1, 30)]
    private int quantityOfWhistle = 8;

    public void ApplyState()
    {
        TryGetContextReference();

        ApplyFireworksChanges();
        ApplyVisibility();

        Context.EnableWicksMovement();
        Context.UpdateProgressBar();

        void TryGetContextReference()
        {
            Context = Mascleta.Instance;

            if (Context == null)
                throw new System.NullReferenceException($"The context in {Name} is null. Check if Mascleta script is present.");
        }
        void ApplyFireworksChanges()
        {
            foreach (WicksFlyweight type in Context.FireWorksManager.Types)
            {
                ChangeSound(type);
                ChangeSpeed(type);
            }

            SetNumberOfWicks();
            ChangeWickDecibelsOnFire();
            SetMobileVibration();

            void ChangeSound(WicksFlyweight type)
            {
                type.EventEmitter.Event = ReplaceLastCharForStageID();

                string ReplaceLastCharForStageID()
                {
                    string previousEventName = type.EventEmitter.Event;
                    return previousEventName.Substring(0, previousEventName.Length - 1) + stageID.ToString();
                }
            }
            void ChangeSpeed(WicksFlyweight type) => type.Speed = WicksSpeed;
            void SetNumberOfWicks()
            {
                List<WickGenerator> generators = Context.FireWorksManager.WickGenerators;
                var continuousGenerators = generators.OfType<BoundedContinuousWickPooling>().ToList();

                continuousGenerators.Find(gen => gen.name == "Ultra Low").QuantityToGenerate = quantityOfUltraLow;
                continuousGenerators.Find(gen => gen.name == "Ultra Low").MaxDistanceBetweenWicks = maxDistanceBetweenUltraLows;

                continuousGenerators.Find(gen => gen.name == "Low").QuantityToGenerate = quantityOfLow;
                continuousGenerators.Find(gen => gen.name == "Low").MaxDistanceBetweenWicks = maxDistanceBetweenLows;

                continuousGenerators.Find(gen => gen.name == "Mid").QuantityToGenerate = quantityOfMid;
                continuousGenerators.Find(gen => gen.name == "Mid").MaxDistanceBetweenWicks = maxDistanceBetweenMids;

                continuousGenerators.Find(gen => gen.name == "Whistle").QuantityToGenerate = quantityOfWhistle;
                continuousGenerators.Find(gen => gen.name == "Whistle").MaxDistanceBetweenWicks = maxDistanceBetweenWhistles;

                generators.OfType<BatchWickGenerator>().ToList().Find(gen => gen.name == "Trueno").QuantityToGenerate = 0;
            }
            void ChangeWickDecibelsOnFire()
            {
                var firstWickGenerator = Context.FireWorksManager.WickGenerators[0];
                firstWickGenerator.WickType.GetComponent<Wick>().DecibelsIncrementPercentage = decibelsIncrementPercentage;
            }
            void SetMobileVibration()
            {
                Context.FireWorksManager.Types.ForEach(type => type.IsMobileVibrationActive = vibrateOnWickFired);
            }
        }
        void ApplyVisibility() => Fog.Instance.SetFogToScreenPercentage(fogExtension);
    }

    public void TransitionToNextState() => Context.CurrentState = nextState as IState;
}
