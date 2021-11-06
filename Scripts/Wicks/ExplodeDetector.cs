using UnityEngine;

public class ExplodeDetector : MonoBehaviour
{
    private Feedback feedback;

    public delegate void OnWickFiredAction(Wick wick);
    public static event OnWickFiredAction OnAnyWickFired;

    private void Awake() => feedback = GetComponent<Feedback>();

    public void OnTouch()
    {
        feedback.Hit();

        var wick = GetComponent<Wick>();
        OnAnyWickFired(wick);

        wick.Dispatch();
    }
}
