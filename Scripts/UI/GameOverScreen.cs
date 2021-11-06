using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StudioEventEmitter))]
public class GameOverScreen : MonoBehaviour
{
    private static GameOverScreen instance;
    public static GameOverScreen Instance => instance;

    [SerializeField]
    private GameObject gameOverUI;
    private Animator gameOverUIAnimator;

    [SerializeField]
    private Image overallResult;
    [SerializeField]
    private Text hits;
    [SerializeField]
    private Text totalHits;
    [SerializeField]
    private Text truenos;
    [SerializeField]
    private Text totalTruenos;
    [SerializeField]
    private Text averageDecibels;

    [Header("Message strings")]
    [SerializeField]
    private Sprite winMessage;
    [SerializeField]
    private Sprite loseMessage;

    private void Awake()
    {
        EnsureSingleton();

        gameOverUIAnimator = gameOverUI.GetComponent<Animator>();

        void EnsureSingleton()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }
    }

    void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void OnGameEnds()
    {
        if (gameOverUI == null)
            return;

        gameOverUI.SetActive(true);

        SetPlayerResults();
        PlayGameOverSound();

        void SetPlayerResults()
        {
            int hits = Score.Instance.Hits;
            int totalHits = Score.Instance.TotalHits;

            this.hits.text = hits.ToString();
            this.totalHits.text = totalHits.ToString();

            this.truenos.text = Score.Instance.Truenos.ToString();
            this.totalTruenos.text = Score.Instance.TotalTruenos.ToString();

            SetUiBasedOnGameResult();

            averageDecibels.text = Score.Instance.AverageDecibels.ToString();

            void SetUiBasedOnGameResult()
            {
                bool isTheGameWon = hits >= totalHits * 0.5f;

                if (!isTheGameWon)
                    gameOverUIAnimator.Play("GameOverTransitionLose");

                overallResult.sprite = isTheGameWon ? winMessage : loseMessage;
            }
        }
        void PlayGameOverSound()
        {
            StudioEventEmitter eventEmitter = GetComponent<StudioEventEmitter>();

            eventEmitter.Play();
            eventEmitter.SetParameter("WinLose", Score.Instance.HitPercentage);

            if (!eventEmitter.IsPlaying())
                throw new UnityException("FMOD event in GameOverScreen is not playing when it should!");
        }
    }
}
