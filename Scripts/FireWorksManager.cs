using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireWorksManager : MonoBehaviour
{
    public List<WicksFlyweight> Types { get; } = new List<WicksFlyweight>();

    public bool HasAnyWick => Types.Any(type => type.RemainingWicks != 0);

    public List<WickGenerator> WickGenerators { get; private set; }

    private int requestForTransitionToNextState;

    void Start()
    {
        InitializeWicks();
        ReferenceWickGenerators();

        void InitializeWicks()
        {
            foreach (WicksFlyweight wickFlyweight in GetComponentsInChildren<WicksFlyweight>())
            {
                wickFlyweight.Manager = this;
                Types.Add(wickFlyweight);
            }
        }
        void ReferenceWickGenerators()
        {
            WickGenerators = GetComponentsInChildren<WickGenerator>().ToList();

            foreach (var generator in WickGenerators)
                generator.enabled = false;
        }
    }

    public void TryRequestTransitionToNextState()
    {
        requestForTransitionToNextState++;

        if (NoWickLeftToGenerate())
        {
            RequestTransitionToNextState();
            requestForTransitionToNextState = 0;
        }

        bool NoWickLeftToGenerate() => requestForTransitionToNextState == 4;
    }

    public void RequestTransitionToNextState() => Mascleta.Instance.TransitionToNextState();

    public void EnableWicksMovement()
    {
        AutoGenerateRandomWicks();

        foreach (WicksFlyweight type in Types)
            type.EnableWicksMovement();

        void AutoGenerateRandomWicks()
        {
            foreach (var generator in WickGenerators)
                if (HasAnyWickToGenerate(generator))
                    generator.enabled = true;

            bool HasAnyWickToGenerate(WickGenerator generator)
            {
                return (generator is BatchWickGenerator && (generator as BatchWickGenerator).QuantityToGenerate != 0)
                    || (generator is BoundedContinuousWickPooling && (generator as BoundedContinuousWickPooling).QuantityToGenerate != 0);
            }
        }
    }
}
