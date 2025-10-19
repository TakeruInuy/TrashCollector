using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : Entity
{

    public TrashColor.TrashColorTag trashColor;


    [SerializeField] private int _baseHitPoints = 2;
    private int _currentHitPoints;
    public float carryWeight = 5f;
    [SerializeField] private Image _hpBar;
    //public int resourceDropped = 2;
    public bool isDead = false;
    //[HideInInspector]public FollowTarget followTarget;
    [HideInInspector] public InertiaFollowTarget followTarget;

    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;




    private void Awake()
    {
        followTarget = GetComponent<InertiaFollowTarget>();
        _currentHitPoints = _baseHitPoints;
        onDamageTaken.AddListener(UpdateHPBar);
    }

    private void OnEnable()
    {
        followTarget.DisableFollow();
        _currentHitPoints = _baseHitPoints;
    }

    public void TakeDamage(int damageToTake)
    {

        _currentHitPoints -= damageToTake;
        onDamageTaken.Invoke();
        if (_currentHitPoints <= 0)
        {
            Die();
        }       
    }

    public void Die()
    {
        if(!isDead)
        {
            isDead = true;
            onDeath.Invoke();
        }        
    }

    public void UpdateHPBar()
    {
        if(_hpBar)
            _hpBar.fillAmount = _currentHitPoints/ (float)_baseHitPoints;
    }
}
