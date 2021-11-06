using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)]
    private float smoothRate;

    [SerializeField]
    private List<float> stagePoints = new List<float>();
    private float currentPoint;

    private Slider bar;

    private void Awake() => bar = GetComponent<Slider>();

    private void Start() => gameObject.SetActive(false);

    [ContextMenu("MoveBarToNextPoint")]
    public void MoveBarToNextPoint()
    {
        ExtractFirstElement();

        if (currentPoint == 0)
            SetSliderToCurrentPoint();
        else
            StartCoroutine(LerpSliderToCurrentPoint());

        void ExtractFirstElement()
        {
            currentPoint = stagePoints[0];
            stagePoints.Remove(currentPoint);
        }
        void SetSliderToCurrentPoint() => bar.value = currentPoint;
        IEnumerator LerpSliderToCurrentPoint()
        {
            while (bar.value != currentPoint)
            {
                bar.value = Mathf.Lerp(bar.value, currentPoint, Time.deltaTime * smoothRate);

                yield return null;
            }
        }
    }
}
