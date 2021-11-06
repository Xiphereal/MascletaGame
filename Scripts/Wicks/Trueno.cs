public class Trueno : Wick
{
    public override WicksFlyweight Flyweight
    {
        get => base.Flyweight;
        set
        {
            base.Flyweight = value;

            if (Flyweight != null)
                transform.localScale *= Flyweight.SizeRatio;
        }
    }

    public override void Dispatch()
    {
        if (Flyweight != null)
            Flyweight.OnWickFired();

        Destroy(gameObject);
    }
}
