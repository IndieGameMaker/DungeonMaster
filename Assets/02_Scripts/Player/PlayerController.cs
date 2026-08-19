using UnityEngine;
using DungeonMaster.InputSystem;

public class PlayerController : MonoBehaviour
{

    private void OnEnable()
    {
        // 구독처리
        InputHandler.OnMoveAction += OnPlayerMove;
        InputHandler.OnAttackAction += OnPlayerAttack;
    }


    private void OnDisable()
    {
        // 구독해지
        InputHandler.OnMoveAction -= OnPlayerMove;
        InputHandler.OnAttackAction -= OnPlayerAttack;
    }

    private void OnPlayerMove(Vector2 ctx)
    {
        Debug.Log($"플레이어 이동: {ctx}");
    }
    
    private void OnPlayerAttack()
    {
        Debug.Log($"플레이어 공격!!!");
    }
}
