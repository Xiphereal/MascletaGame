using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private BackgroundCrowd backgroundCrowd;

    private void Awake() => pausePanel.SetActive(false);

    public void Open()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        PauseAllSounds();

        void PauseAllSounds()
        {
            FMODUnity.RuntimeManager.GetBus("Bus:/Pausable").setPaused(true);
            backgroundCrowd.OnPause();
        }
    }

    public void Close()
    {
        ResumeAllSounds();
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        void ResumeAllSounds()
        {
            FMODUnity.RuntimeManager.GetBus("Bus:/Pausable").setPaused(false);
            backgroundCrowd.OnResume();
        }
    }
}
