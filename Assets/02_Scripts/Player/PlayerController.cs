using UnityEngine;
using DungeonMaster.InputSystem;

public class PlayerController : MonoBehaviour
{

    private void OnEnable()
    {
        // 구독처리
        InputHandler.OnMoveAction += OnPlayerMove;
    }

    private void OnDisable()
    {
        // 구독해지
        InputHandler.OnMoveAction -= OnPlayerMove;
    }

    private void OnPlayerMove(Vector2 ctx)
    {
        Debug.Log($"PlayerMove: {ctx}");
    }
}
