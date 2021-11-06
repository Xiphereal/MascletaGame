using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mascleta : MonoBehaviour
{
    public static Mascleta Instance { get; private set; }

    private CrowdReactions crowdReactions;
    public CrowdReactions CrowdReactions
    {
        get
        {
            if (crowdReactions == null)
                crowdReactions = GetComponentInChildren<CrowdReactions>();

            return crowdReactions;
        }
    }

    [SerializeField]
    private FireWorksManager fireWorksManager;
    public FireWorksManager FireWorksManager { get => fireWorksManager; }

    [SerializeField]
    private List<GameObject> ingameUI;

    [SerializeField]
    private ProgressBar progressBar;

    public List<IState> States { get; set; }
    public IState CurrentState { get; set; }

    void Awake()
    {
        EnsureSingleton();

        gameObject.SetActive(false);
        ReferenceStatesFromAssets();

        void EnsureSingleton()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(this);
        }
    }

    public void ReferenceStatesFromAssets()
    {
        object[] assets = Resources.LoadAll("GameConfiguration/MascletaStates");
        List<IState> states = new List<IState>();

        foreach (object asset in assets)
            states.Add(asset as IState);

        this.States = states;
    }

    private void OnEnable()
    {
        CurrentState = States.Find(state => state.Name == "Stage1");
        CurrentState.ApplyState();
        ingameUI.ForEach(ui => ui.SetActive(true));

        ExplodeDetector.OnAnyWickFired += OnWickFired;
    }

    private void OnDisable() => ExplodeDetector.OnAnyWickFired -= OnWickFired;

    public void EnableWicksMovement() => fireWorksManager.EnableWicksMovement();

    public void UpdateProgressBar() => progressBar?.MoveBarToNextPoint();

    public void OnWickFired(Wick wick) => CrowdReactions.RestartCrowdReactionCoroutine();

    public void TransitionToNextState()
    {
        CurrentState.TransitionToNextState();
        CurrentState.ApplyState();
    }

    #region Debug
    [ContextMenu("Change to 2nd Stage")]
    private void ChangeTo2ndStage()
    {
        CurrentState = States.Find(state => state.Name == "Stage2");
        CurrentState.ApplyState();
    }

    [ContextMenu("Change to 3rd Stage")]
    private void ChangeTo3rdStage()
    {
        CurrentState = States.Find(state => state.Name == "Stage3");
        CurrentState.ApplyState();
    }

    [ContextMenu("Change to 4th Stage")]
    private void ChangeTo4thStage()
    {
        CurrentState = States.Find(state => state.Name == "Stage4");
        CurrentState.ApplyState();
    }
    #endregion
}