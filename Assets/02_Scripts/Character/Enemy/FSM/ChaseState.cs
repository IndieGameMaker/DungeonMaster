using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class ChaseState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            // 애니메이션을 Chase 변경
            enemy.SetWalk(true);
        }

        public void OnUpdate(Enemy enemy)
        {
            if (enemy.PlayerDetectable())
            {
                // 거리가 멀어지면 다시 IdleState 로 전환
                if (!enemy.DetectPlayer())
                {
                    enemy.ChangeState<IdleState>();
                }
            }

            // TODO: 플레이어와의 거리를 측정하고 공격사정거리 이내이면 AttackState로 변경
        }

        public void OnExit(Enemy enemy)
        {
        }
    }
}