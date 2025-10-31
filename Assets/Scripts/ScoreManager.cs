using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class ScoreManager : MonoBehaviour
{


    public static ScoreManager Instance { get; private set; }

    public int score;
    public float timerMultiplier = 1f;
    public UnityEvent onScoreChange = new UnityEvent();

    public int trashRequired = 20;
    private int trashCollected = 0;
    public UnityEvent onTrashCollectedChange = new UnityEvent();

    public UnityEvent onWin = new UnityEvent();

    [SerializeField] private TextMeshProUGUI _scoreUI;
    [SerializeField] private TextMeshProUGUI _finalScoreUI;
    [SerializeField] private TextMeshProUGUI _trashCollectedUI;
    [SerializeField] private TextMeshProUGUI _timerBonusUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _scoreUI.text = score.ToString();
        trashCollected = 0;
        UpdateTrashCollectedUI();

        onScoreChange.AddListener(UpdateScoreUI);

        onTrashCollectedChange.AddListener(CheckWin);
        onTrashCollectedChange.AddListener(UpdateTrashCollectedUI);

        onWin.AddListener(ApplyTimerBonus);
        onWin.AddListener(UpdateFinalScoreUI);
    }

    public void AddScore(int amount)
    {
        score += amount;
        onScoreChange.Invoke();
    }

    public void RemoveScore(int amount)
    {
        score -= amount;
        onScoreChange.Invoke();
    }

    public void UpdateScoreUI()
    {        
        _scoreUI.text = score.ToString();
    }

    public void UpdateFinalScoreUI()
    {
        _finalScoreUI.text = score.ToString();
        _timerBonusUI.text = new string("Time Bonus: " + timerMultiplier * 100 + "%");
    }


    public void IncreaseTrashCollected(int amount)
    {
        trashCollected += amount;
        onTrashCollectedChange.Invoke();
    }

    public void UpdateTrashCollectedUI()
    {
        _trashCollectedUI.text = new string(trashCollected + "/" + trashRequired);
    }

    public void CheckWin()
    {
        if(trashCollected >= trashRequired)
        {
            onWin.Invoke();
        }
    }

    public void ApplyTimerBonus()
    {
        float finalScore = score * (1f + timerMultiplier);
        Debug.Log(finalScore);
        score = (int)finalScore;
    }

    public void ResetScore()
    {
        score = 0;
        trashCollected = 0;
    }

}
