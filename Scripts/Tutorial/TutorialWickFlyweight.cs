public class TutorialWickFlyweight : WicksFlyweight
{
    public override int RemainingWicks
    {
        get => remainingWicks;
        set => remainingWicks = value;
    }

    public override void OnWickFired() { }
}
