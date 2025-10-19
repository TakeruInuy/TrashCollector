using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollector : MonoBehaviour
{

    //[SerializeField] private List<GameObject> objectsList;
    //[SerializeField] private Transform _target;

    //[SerializeField] private float _dumpSpeed = 3f;         
    //[SerializeField] private float _dumpInterval = 1f;

    //private bool isMoving = false;

    //public void StartMovingStack()
    //{
    //    if (!isMoving)
    //        StartCoroutine(DumpObjects());
    //}

    //private IEnumerator DumpObjects()
    //{
    //    isMoving = true;

    //    // percorre a lista de cima para baixo
    //    for (int i = PlayerEnemyCollect.enemiesCollected.Count - 1; i >= 0; i--)
    //    {
    //        Enemy obj = PlayerEnemyCollect.enemiesCollected[i];
    //        if (obj == null) continue;

    //        // move o objeto até a caixa
    //        yield return //criar metodo para mover os bicho

    //        // espera um tempo antes do próximo
    //        yield return new WaitForSeconds(_dumpInterval);
    //    }

    //    isMoving = false;
    //}
}
