using System.Collections;
using UnityEngine;

public class BoundedContinuousWickPooling : ContinuousWickPooling
{
    [Header("Configuration")]
    [SerializeField]
    [Range(0, 30)]
    private int quantityToGenerate = 8;
    public int QuantityToGenerate { get => quantityToGenerate; set => quantityToGenerate = value; }

    public Transform PreviousWick => previousWick;

    protected void Awake() => enabled = false;

    protected new void OnEnable()
    {
        if (DisableIfThereAreNoWicksToGenerate())
            return;

        base.OnEnable();
    }

    private bool DisableIfThereAreNoWicksToGenerate()
    {
        if (quantityToGenerate == 0)
        {
            enabled = false;
            transform.parent.GetComponent<FireWorksManager>().TryRequestTransitionToNextState();
            return true;
        }

        return false;
    }

    protected override void CreateNewGeneration()
    {
        base.CreateNewGeneration();

        quantityToGenerate--;

        DisableIfThereAreNoWicksToGenerate();
    }
}
