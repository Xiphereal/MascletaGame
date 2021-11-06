using UnityEngine;

public interface IState
{
    Mascleta Context { get; set; }
    string Name { get; }
    int StateID { get; }
    ScriptableObject NextState { get; }
    float WicksSpeed { get; }

    void ApplyState();
    void TransitionToNextState();
}