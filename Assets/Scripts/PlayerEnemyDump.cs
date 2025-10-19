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
        var enemies = PlayerEnemyCollect.enemiesCollected;


        for (int i = PlayerEnemyCollect.enemiesCollected.Count - 1; i >= 0; i--)
        {
     
            InertiaFollowTarget objFollowComponent = enemies[i].GetComponent<InertiaFollowTarget>();
            
            if (objFollowComponent == null) continue;

            objFollowComponent.DisableFollow();
            objFollowComponent.rb.linearVelocity = Vector3.zero;

            bool containTagColor = false;

 
            
            foreach (TrashColor.TrashColorTag tagColor in dumpster.Tags)
            {
                if (tagColor == enemies[i].trashColor)
                {
                    containTagColor = true;
                    break;
                }
            }


            if (!containTagColor) continue;


            // Stores color to check
            var targetColor = enemies[i].trashColor;
            List<Enemy> groupToDump = new List<Enemy> { enemies[i] };

            // Checks for same color in sequence
            int j = i - 1;
            while (j >= 0 && enemies[j].trashColor == targetColor)
            {
                groupToDump.Add(enemies[j]);

                j--;
            }

            // Move all consecutive colors at the same time
            foreach (var obj in groupToDump)
            {
                var follow = obj.GetComponent<InertiaFollowTarget>();
                if (follow == null) continue;
                Debug.Log(obj);
                Vector3 dir = (dumpster.transform.position - follow.rb.position).normalized;
                follow.rb.linearVelocity = dir * _dumpSpeed;
                Debug.Log(follow.rb.linearVelocity);
            }


            yield return new WaitForSeconds(_dumpInterval);

            // removes dumped items
            foreach (var obj in groupToDump)
            {
                var follow = obj.GetComponent<InertiaFollowTarget>();
                if (follow == null) continue;

                follow.rb.linearVelocity = Vector3.zero;
                obj.gameObject.SetActive(false);
                enemies.Remove(obj);
            }

            // Adjusts main loop
            i = j + 1;


            //Vector3 dir = (dumpster.transform.position - objFollowComponent.rb.position).normalized;
            //    objFollowComponent.rb.linearVelocity = dir * _dumpSpeed;


            //    yield return new WaitForSeconds(_dumpInterval);

            //    PlayerEnemyCollect.enemiesCollected.Remove(PlayerEnemyCollect.enemiesCollected[i]);
            //    objFollowComponent.rb.linearVelocity = Vector3.zero;
            //    objFollowComponent.gameObject.SetActive(false);

        }

        //PlayerEnemyCollect.enemiesCollected.Clear();

        PlayerMovement.canMove = true;
        isMoving = false;


        base.DoAction(dumpster);

        yield return null;

    }
}
