using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject upperCleanUpTrigger;

    private void Reset()
    {
        upperCleanUpTrigger = gameObject.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        upperCleanUpTrigger.SetActive(false);
        gameObject.SetActive(false);
    }

    public void EndTutorial()
    {
        DisableContinuousGeneration();
        CleanUpRemainingWicks();

        void DisableContinuousGeneration()
        {
            foreach (ContinuousWickGeneration wickGeneration in GetComponentsInChildren<ContinuousWickGeneration>())
                wickGeneration.enabled = false;
        }
        void CleanUpRemainingWicks()
        {
            upperCleanUpTrigger.SetActive(true);
            StartCoroutine(DisableGameObjectAfterAWhile());

            IEnumerator DisableGameObjectAfterAWhile()
            {
                yield return new WaitForSeconds(5);

                gameObject.SetActive(false);
                //upperCleanUpTrigger.SetActive(false);
            }
        }
    }
}
