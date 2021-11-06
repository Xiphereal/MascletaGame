using System.Collections.Generic;
using UnityEngine;

public class WickPool : MonoBehaviour
{
    [SerializeField]
    private GameObject wickType;
    public GameObject WickType => wickType;
    [SerializeField]
    private Transform pool;

    [SerializeField]
    [Range(0, 50)]
    private int quantityOfWicksToAllocateAtAwake;

    private Queue<GameObject> wicksInPool = new Queue<GameObject>();

    private void Awake()
    {
        AllocateWicksInPool();

        void AllocateWicksInPool()
        {
            for (int i = 0; i < quantityOfWicksToAllocateAtAwake; i++)
                wicksInPool.Enqueue(Instantiate(wickType, pool));
        }
    }

    public GameObject Pool(Vector3 position, GameObject wickBatch)
    {
        GameObject wick = null;

        if (IsAnyWickUnused())
            wick = PoolWickOfType();
        else
            wick = InstantiateNewWick();

        return wick;

        bool IsAnyWickUnused() => wicksInPool.Count > 0;
        GameObject PoolWickOfType()
        {
            GameObject pooledWick = wicksInPool.Dequeue();
            pooledWick.transform.parent = wickBatch.transform;
            pooledWick.transform.position = position;

            return pooledWick;
        }
        GameObject InstantiateNewWick()
        {
            return Instantiate(wickType, position, Quaternion.identity, wickBatch.transform);
        }
    }

    public void ReturnToPool(GameObject wick)
    {
        wick.transform.parent = transform;
        wick.transform.localPosition = Vector3.zero;

        wicksInPool.Enqueue(wick);
    }
}
