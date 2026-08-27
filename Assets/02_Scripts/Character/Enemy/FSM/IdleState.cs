using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class IdleState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            // 애니메이션을 Idle 변경
            enemy.SetWalk(false);
            // Idle 상태는 반드시 정지 상태이어야 만 한다.
            enemy.StopMoving();
        }

        public void OnUpdate(Enemy enemy)
        {
            // 플레이어와의 거리를 측정하고 추적사정거리 이내이면 ChaseState로 변경
            if (enemy.PlayerDetectable() && enemy.DetectPlayer())
            {
                // 추적 상태로 전환
                enemy.ChangeState<ChaseState>();
            }
        }

        public void OnExit(Enemy enemy)
        {
            
        }
    }
}
