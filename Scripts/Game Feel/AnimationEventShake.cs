using UnityEngine;
using System.Collections;

public class AnimationEventShake : MonoBehaviour
{
    [SerializeField]
    private Wick mockupWick;

    public void ShakeOnDemand() => GetComponent<ShakeGroup>().ShakeAllElementsInGroup(mockupWick);
}
