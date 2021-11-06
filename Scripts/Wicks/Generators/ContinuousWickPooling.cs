﻿using UnityEngine;

public class ContinuousWickPooling : ContinuousWickGeneration
{
    [Header("References")]
    [SerializeField]
    private WickPool wickPool;
    public WickPool WickPool => wickPool;

    protected override Transform GenerateWick(Vector2 previousWickPosition)
    {
        Vector2 position = previousWickPosition + Vector2.up * CalculateSpacingBetweenWicks();

        GameObject pooledWick = wickPool.Pool(position, wicksBatch);
        pooledWick.name = autogeneratedName;

        ConfigurateWick();

        NotifyGenerationToScore(pooledWick);

        return pooledWick.transform;

        void ConfigurateWick()
        {
            Wick wickScript = pooledWick.GetComponent<Wick>();
            wickScript.Flyweight = wicksBatch.GetComponent<WicksFlyweight>();
            wickScript.enabled = true;
        }
    }
}