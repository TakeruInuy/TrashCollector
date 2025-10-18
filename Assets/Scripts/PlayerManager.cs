using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private PlayerMovement _playerMovement;
    private PlayerEnemyCollect _playerEnemyCollect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerEnemyCollect = GetComponent<PlayerEnemyCollect>();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
