using UnityEngine;

public class WickCleanUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wick")
            other.GetComponent<Wick>().Dispatch();
    }
}
