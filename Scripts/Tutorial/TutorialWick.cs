public class TutorialWick : Wick
{
    public override string FireWorkSoundEventName => "Muted Event";

    protected new void Awake() => enabled = false;

    protected void Start() => Flyweight = GetComponentInParent<TutorialWickFlyweight>();

    protected override ContinuousWickPooling GetWickGenerator()
    {
        return Flyweight.GetComponentInParent<ContinuousWickPooling>();
    }
}
