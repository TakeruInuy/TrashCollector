using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class PlayerEnemyDump : PlayerInteraction
{
    [SerializeField] private float _dumpInterval = 2f;
    [SerializeField] private float _dumpSpeed = 1f;

    private bool isMoving = false;



    protected override void DoAction(Entity entity)
    {

        StartMovingStack(entity.GetComponent<Dumpster>());

       
    }

    public void StartMovingStack(Dumpster dumpster)
    {
        if (!isMoving)
            StartCoroutine(DumpObjects(dumpster));
    }

    private IEnumerator DumpObjects(Dumpster dumpster)
    {
        isMoving = true;
        PlayerMovement.canMove = false;



        for (int i = PlayerEnemyCollect.enemiesCollected.Count - 1; i >= 0; i--)
        {
     
            InertiaFollowTarget objFollowComponent = PlayerEnemyCollect.enemiesCollected[i].GetComponent<InertiaFollowTarget>();
            
            if (objFollowComponent == null) continue;

            objFollowComponent.DisableFollow();
            objFollowComponent.rb.linearVelocity = Vector3.zero;

            bool containTagColor = false;
            
            foreach (TrashColor.TrashColorTag tagColor in dumpster.Tags)
            {
                if (tagColor == PlayerEnemyCollect.enemiesCollected[i].trashColor)
                    containTagColor = true;                
            }
            Debug.Log("contain tag color = " + containTagColor);

            if (containTagColor)
            {
                Vector3 dir = (dumpster.transform.position - objFollowComponent.rb.position).normalized;
                objFollowComponent.rb.linearVelocity = dir * _dumpSpeed;


                yield return new WaitForSeconds(_dumpInterval);

                PlayerEnemyCollect.enemiesCollected.Remove(PlayerEnemyCollect.enemiesCollected[i]);
                objFollowComponent.rb.linearVelocity = Vector3.zero;
                objFollowComponent.gameObject.SetActive(false);
            }





        }

        //PlayerEnemyCollect.enemiesCollected.Clear();

        PlayerMovement.canMove = true;
        isMoving = false;


        base.DoAction(dumpster);

        yield return null;

    }
}
