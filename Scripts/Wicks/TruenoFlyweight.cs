public class TruenoFlyweight : WicksFlyweight
{
    public override bool IsMobileVibrationActive { get => true; set => base.IsMobileVibrationActive = value; }
    public bool IsFeedbackDelayed { get; set; } = true;

    public override void OnWickFired()
    {
        RemainingWicks--;

        Manager.RequestTransitionToNextState();
    }
}
