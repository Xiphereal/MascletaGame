using UnityEngine;

public class ContinuousWickGeneration : WickGenerator
{
    [SerializeField]
    [Range(0.1f, 3f)]
    private float delayBetweenGenerations = 20f;
    public float DelayBetweenGenerations { get => delayBetweenGenerations; set => delayBetweenGenerations = value; }

    private float elapsedTimeSinceLastGeneration;

    protected Transform previousWick;

    protected void OnEnable() => CreateNewGeneration();

    void Update() => CheckForNewGeneration();

    protected void CheckForNewGeneration()
    {
        elapsedTimeSinceLastGeneration += Time.deltaTime;

        if (elapsedTimeSinceLastGeneration >= delayBetweenGenerations)
        {
            CreateNewGeneration();

            elapsedTimeSinceLastGeneration = 0f;
        }
    }

    protected virtual void CreateNewGeneration()
    {
        Transform generatedWick = IsPreviousWickValid()
            ? GenerateWick(previousWick.position)
            : GenerateWick(wicksBatch.transform.position);

        previousWick = generatedWick;

        bool IsPreviousWickValid()
        {
            if (previousWick == null)
                return false;

            Wick wickScript = previousWick.GetComponent<Wick>();

            return wickScript != null && wickScript.Flyweight != null;
        }
    }
}
