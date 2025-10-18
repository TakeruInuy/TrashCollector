using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerInteraction : MonoBehaviour
{

    [SerializeField] protected float _actionRadius = 5f;
    [SerializeField] protected LayerMask _entityLayer;
    [SerializeField] protected Transform radiusUI;
    [SerializeField] protected float _UIScaleMultiplier = 1;
    public UnityEvent onAction;



    // Update is called once per frame
    void Update()
    {
        CheckForEntity();
    }

    protected void CheckForEntity()
    {
        var entitiesHit = Physics.OverlapSphere(transform.position, _actionRadius, _entityLayer);

        foreach (var entity in entitiesHit)
        {
            var entityComponent = entity.GetComponent<Entity>();
            DoAction(entityComponent);
        }
    }

    protected virtual void DoAction(Entity entity)
    {
        onAction.Invoke();
    }


    protected virtual void OnValidate()
    {
        if (radiusUI != null)
        {
            radiusUI.localScale = new Vector3(_actionRadius * _UIScaleMultiplier, _actionRadius * _UIScaleMultiplier, _actionRadius * _UIScaleMultiplier);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Color gizmosColor = Color.yellow;
        gizmosColor.a = 0.3f;

        Gizmos.color = gizmosColor;
        
        Gizmos.DrawSphere(transform.position, _actionRadius);
    }
}