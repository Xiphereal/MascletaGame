using UnityEngine;
using UnityEngine.SceneManagement;

public class MascletaSceneManager : MonoBehaviour
{
    public void ReloadScene()
    {
        Score.Instance.Destroy();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        FMODUnity.RuntimeManager.GetBus("Bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
