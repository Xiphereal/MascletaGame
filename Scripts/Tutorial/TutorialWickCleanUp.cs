using UnityEngine;

public class TutorialWickCleanUp : WickCleanUp
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "TutorialWick")
            other.GetComponent<TutorialWick>().Dispatch();
    }
}
