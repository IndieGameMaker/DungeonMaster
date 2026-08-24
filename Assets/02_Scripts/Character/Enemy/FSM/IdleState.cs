using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class IdleState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            Debug.Log("Idle 진입");
            // 애니메이션을 Idle 변경
        }

        public void OnUpdate(Enemy enemy)
        {
            Debug.Log("Idle 갱신");
            // 플레이어와의 거리를 측정하고 추적사정거리 이내이면 ChaseState로 변경
        }

        public void OnExit(Enemy enemy)
        {
            Debug.Log("Idle 종료");
        }
    }
}
