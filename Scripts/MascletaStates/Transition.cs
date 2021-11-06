using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Transition", menuName = "ScriptableObjects/New Transition", order = 2)]
public class Transition : ScriptableObject, IState
{
    public Mascleta Context { get; set; }

    public string Name => $"From{(previousState as IState)}To{(nextState as IState).Name}";

    [SerializeField]
    private ScriptableObject previousState;

    [SerializeField]
    private ScriptableObject nextState;
    public ScriptableObject NextState => nextState;

    [SerializeField]
    private int stateID = -1;
    public int StateID => stateID != -1 ? stateID : (previousState as IState).StateID;

    [SerializeField]
    private string truenoEventID;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    [Range(1, 10)]
    private int quantityToGenerate;

    [SerializeField]
    private bool delayedFeedback = true;

    [Header("\"Terremoto\" only")]
    [SerializeField, Range(0.01f, 10f)]
    private float wickSpeed;
    [SerializeField]
    private float appearanceDelay;

    public float WicksSpeed
    {
        get
        {
            if (IsTerremoto())
                return wickSpeed;

            return (previousState as IState).WicksSpeed;
        }
        set => wickSpeed = value;
    }

    private bool IsTerremoto() => previousState is Transition;

    public void ApplyState()
    {
        TryGetContextReference();

        TruenoFlyweight trueno = Context.FireWorksManager.Types.OfType<TruenoFlyweight>().First();

        ChangeSound(trueno);
        ChangeSpeed(trueno);
        ChangeSprite(trueno);

        if (IsTerremoto())
        {
            var truenoGenerator = Context.FireWorksManager.WickGenerators.OfType<BatchWickGenerator>().First();

            SetTerremotoOnFlyweight(truenoGenerator);
            ChangeSize(trueno);
            DelayAppearance(truenoGenerator);
            Fog.Instance.SetSortingLayerToDefault();
        }

        SetFeedbackDelay(trueno);
        SetNumberOfWicks();

        Context.EnableWicksMovement();

        void TryGetContextReference()
        {
            Context = Mascleta.Instance;

            if (Context == null)
                throw new System.NullReferenceException($"The context in {Name} is null. Check if Mascleta script is present.");
        }
        void ChangeSound(TruenoFlyweight type) => type.EventEmitter.Event = "event:/Sounds/Trueno" + truenoEventID;
        void ChangeSpeed(TruenoFlyweight type) => type.Speed = WicksSpeed;
        void ChangeSprite(TruenoFlyweight type)
        {
            if (sprite != null)
                type.Sprite = sprite;
        }
        void SetTerremotoOnFlyweight(BatchWickGenerator truenoGenerator) => truenoGenerator.IsTerremoto = IsTerremoto();
        void ChangeSize(TruenoFlyweight type) => type.SizeRatio *= 1.1f;
        void DelayAppearance(BatchWickGenerator truenoGenerator) => truenoGenerator.GenerationDelay = appearanceDelay;
        void SetFeedbackDelay(TruenoFlyweight type) => type.IsFeedbackDelayed = delayedFeedback;
        void SetNumberOfWicks()
        {
            List<WickGenerator> generators = Context.FireWorksManager.WickGenerators;
            var continuousGenerators = generators.OfType<BoundedContinuousWickPooling>().ToList();

            continuousGenerators.Find(generator => generator.name == "Ultra Low").QuantityToGenerate = 0;
            continuousGenerators.Find(generator => generator.name == "Low").QuantityToGenerate = 0;
            continuousGenerators.Find(generator => generator.name == "Mid").QuantityToGenerate = 0;
            continuousGenerators.Find(generator => generator.name == "Whistle").QuantityToGenerate = 0;

            var truenoGenerator = generators.OfType<BatchWickGenerator>().First();
            truenoGenerator.QuantityToGenerate = quantityToGenerate;
        }
    }

    public void TransitionToNextState()
    {
        Context.CrowdReactions.StartEndOfStageReaction(Score.Instance.HitPercentage);
        Context.CurrentState = nextState as IState;
    }
}
