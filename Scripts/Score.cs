using UnityEngine;

public class Score
{
    private static Score instance;
    public static Score Instance
    {
        get
        {
            if (instance == null)
                instance = new Score();

            return instance;
        }
    }

    public int Hits { get; set; }
    public int TotalHits { get; set; }
    public float HitPercentage => ((float)Hits / (float)TotalHits) * 100;

    public int Truenos { get; set; }
    public int TotalTruenos { get; set; }

    public int AverageDecibels { get; set; }

    private Score()
    {
        ExplodeDetector.OnAnyWickFired += HitMadeOn;
    }

    public void HitMadeOn(Wick wick)
    {
        if (wick is Trueno)
            Truenos++;
        else
            Hits++;
    }

    public void Destroy() => instance = null;
}
