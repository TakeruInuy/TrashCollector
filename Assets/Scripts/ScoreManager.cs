using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class ScoreManager : MonoBehaviour
{


    public static ScoreManager Instance { get; private set; }

    public int score;
    public UnityEvent onValueChange = new UnityEvent();


    [SerializeField] private TextMeshProUGUI _scoreUI;

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
        onValueChange.AddListener(UpdateScore);
    }

    public void AddScore(int amount)
    {
        score += amount;
        onValueChange.Invoke();
    }

    public void RemoveScore(int amount)
    {
        score -= amount;
        onValueChange.Invoke();
    }

    public void UpdateScore()
    {        
        _scoreUI.text = score.ToString();
    }
}
