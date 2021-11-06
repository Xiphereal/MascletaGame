using System.Collections;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public static Fog Instance { get; private set; }

    [SerializeField]
    private Transform minPosition;
    [SerializeField]
    private Transform maxPosition;

    [SerializeField]
    private Transform fogImage;

    [SerializeField]
    [Range(0f, 20f)]
    private float minimumRequiredDistanceToTarget;
    [SerializeField]
    [Range(0f, 1f)]
    private float rateOfChange;

    private Coroutine activeCoroutine;

    private void Awake()
    {
        EnsureSingleton();
        ResetFogPosition();

        void EnsureSingleton()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null)
                Destroy(this);
        }
        void ResetFogPosition() => fogImage.position = minPosition.position;
    }

    public void SetFogToScreenPercentage(float percentage)
    {
        float fogTargetYposition = Mathf.Lerp(minPosition.position.y, maxPosition.position.y, percentage / 100f);

        StopActiveCorotine();

        activeCoroutine = StartCoroutine(SmoothLerpFogTo(fogTargetYposition));

        void StopActiveCorotine()
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
        }
        IEnumerator SmoothLerpFogTo(float targetOnY)
        {
            float actualYPosition = fogImage.transform.position.y;

            while (FogNotInTargetPosition())
            {
                MoveFogOneStep();

                yield return null;
            }

            bool FogNotInTargetPosition()
            {
                return Mathf.Abs(actualYPosition - targetOnY) > minimumRequiredDistanceToTarget;
            }
            void MoveFogOneStep()
            {
                float newYPosition = Mathf.Lerp(actualYPosition, targetOnY, rateOfChange * Time.deltaTime);

                fogImage.transform.position = new Vector2(fogImage.transform.position.x, newYPosition);

                actualYPosition = newYPosition;
            }
        }
    }

    public void SetSortingLayerToDefault()
    {
        fogImage.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Default";
    }
}
