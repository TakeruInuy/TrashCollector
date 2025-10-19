using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerEnemyCollect : PlayerInteraction
{
    //private int _resourcesToAdd;
    public static List<Enemy> enemiesCollected;
    public static float weightCarried;

    [Header("Enemy Collection Attributes")]
    [SerializeField] private float _enemyOnBackDepthOffset = 1f;
    [SerializeField] private float _enemyOnBackHeightOffset = 1f;
    [SerializeField] private int _startingEnemiesCollectedCapacity = 2;
    private Transform _collectedEnemiesFollowTarget;
    [SerializeField] private float _collectedEnemiesBaseStiffness = 1000f;
    [SerializeField] private float _collectedEnemiesBaseDamper = 50f;
    [SerializeField] private AnimationCurve _collectedEnemyFollowCurve;


    [HideInInspector] public int maxEnemiesCapacity;


    //[Header("Upgrades")]
    //[SerializeField] private int _upgradeCapacityIncrease = 1;
    //[SerializeField] private int _upgradeCost = 1;

    public UnityEvent onClear;



    private void Awake()
    {
        GenerateFollowTarget();
    }



    private void Start()
    {
        maxEnemiesCapacity = _startingEnemiesCollectedCapacity;
        enemiesCollected = new List<Enemy>();
    }

    protected override void DoAction(Entity entity)
    {        
        Enemy enemy = (Enemy)entity;
        if(enemy && enemiesCollected.Count < maxEnemiesCapacity && !enemiesCollected.Contains(enemy) && enemy.isDead)
        {

            enemiesCollected.Add(enemy);
            weightCarried += enemy.carryWeight;

            SetupCollectedEnemyFollow(enemy);


            base.DoAction(entity);
        }
    }

    private void SetupCollectedEnemyFollow(Enemy enemy)
    {
        enemy.transform.position = new Vector3(enemy.transform.position.x,
            enemy.transform.position.y + _enemyOnBackHeightOffset * enemiesCollected.IndexOf(enemy),
            enemy.transform.position.z);
        if (enemiesCollected.IndexOf(enemy) == 0 ) //Checks if is first enemy
        {
            enemy.followTarget.EnableFollow(GetComponent<Rigidbody>(), 
                _collectedEnemiesFollowTarget, 
                _enemyOnBackHeightOffset, 
                _enemyOnBackDepthOffset, 
                _collectedEnemiesBaseStiffness, 
                _collectedEnemiesBaseDamper);
        }
        else
        {
            enemy.followTarget.EnableFollow(enemiesCollected[enemiesCollected.IndexOf(enemy) - 1].GetComponent<Rigidbody>(), //Rigidbody of enemy on bottom
                enemiesCollected[enemiesCollected.IndexOf(enemy) - 1].transform, //transform of bottom enemy
                _enemyOnBackHeightOffset,
                0,
                _collectedEnemiesBaseStiffness * _collectedEnemyFollowCurve.Evaluate(enemiesCollected.IndexOf(enemy) / 50f), //stiffness considering position on the enemy pile
                _collectedEnemiesBaseDamper * _collectedEnemyFollowCurve.Evaluate(enemiesCollected.IndexOf(enemy) / 100f)); //damp considering position on the enemy pile
        }

        //enemy.followTarget.EnableFollow(_collectedEnemiesFollowTarget, _collectedEnemiesBaseFollowSpeed * _collectedEnemyFollowCurve.Evaluate(enemiesCollected.IndexOf(enemy) / 100f));
    }

    public void RefreshEnemiesCollectedFollow()
    {
        for (int i = 0; i < enemiesCollected.Count; i++)
        {
            if(i == 0)
            {
                enemiesCollected[i].followTarget.EnableFollow(GetComponent<Rigidbody>(),
                _collectedEnemiesFollowTarget,
                _enemyOnBackHeightOffset,
                _enemyOnBackDepthOffset,
                _collectedEnemiesBaseStiffness,
                _collectedEnemiesBaseDamper);
            }
            else
                enemiesCollected[i].followTarget.EnableFollow(enemiesCollected[enemiesCollected.IndexOf(enemiesCollected[i]) - 1].GetComponent<Rigidbody>(), //Rigidbody of enemy on bottom
                enemiesCollected[enemiesCollected.IndexOf(enemiesCollected[i]) - 1].transform, //transform of bottom enemy
                _enemyOnBackHeightOffset,
                0,
                _collectedEnemiesBaseStiffness * _collectedEnemyFollowCurve.Evaluate(enemiesCollected.IndexOf(enemiesCollected[i]) / 50f), //stiffness considering position on the enemy pile
                _collectedEnemiesBaseDamper * _collectedEnemyFollowCurve.Evaluate(enemiesCollected.IndexOf(enemiesCollected[i]) / 100f)); //damp considering position on the enemy pile
        }
    }


    private void GenerateFollowTarget()
    {
        _collectedEnemiesFollowTarget = new GameObject().transform;
        _collectedEnemiesFollowTarget.parent = transform;
        _collectedEnemiesFollowTarget.name = "Follow Target";
        _collectedEnemiesFollowTarget.position = new Vector3(transform.position.x, transform.position.y - _enemyOnBackHeightOffset - 10, transform.position.z - _enemyOnBackDepthOffset);
    }

    public void ClearEnemies()
    {

        if (enemiesCollected.Count > 0)
        {
            foreach (var enemy in enemiesCollected)
            {
                //_resourcesToAdd += enemy.resourceDropped;
                enemy.gameObject.SetActive(false);
            }
            //ResourceManager.Instance.AddResource(_resourcesToAdd);
            enemiesCollected.Clear();
            //_resourcesToAdd = 0;
            weightCarried = 0;
            onClear.Invoke();
        }

    }

    //public void IncreaseCapacity()
    //{
    //    if (ResourceManager.Instance.resource >= _upgradeCost)
    //    {
    //      maxEnemiesCapacity += _upgradeCapacityIncrease;
    //        ResourceManager.Instance.RemoveResource(_upgradeCost);
    //    }
        
    //}
}