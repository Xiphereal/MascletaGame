using UnityEngine;

[CreateAssetMenu(fileName = "Game Over Data", menuName = "ScriptableObjects/Game Over Data", order = 1)]
public class GameOverData : ScriptableObject, IState
{
    public Mascleta Context { get; set; }

    public string Name => "Game Over";

    public ScriptableObject NextState => null;

    public int StateID => throw new System.NotSupportedException($"The {Name} state shouldn't have {nameof(StateID)}");

    public float WicksSpeed => throw new System.NotSupportedException($"The {Name} state shouldn't have {nameof(WicksSpeed)}");

    public void ApplyState()
    {
        TryGetContextReference();

        GameOverScreen.Instance.OnGameEnds();
        Context.gameObject.SetActive(false);

        void TryGetContextReference()
        {
            Context = Mascleta.Instance;

            if (Context == null)
                throw new System.NullReferenceException($"The context in {Name} is null. Check if Mascleta script is present.");
        }
    }

    public void TransitionToNextState()
    {
        throw new System.NotSupportedException($"The {Name} state has neither next state or transition to it.");
    }
}
