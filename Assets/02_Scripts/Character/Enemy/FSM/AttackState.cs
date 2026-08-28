using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class AttackState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            enemy.StopMoving();
        }

        public void OnUpdate(Enemy enemy)
        {
            // 넉백 중에는 거리 판정, 공격 시작 모두 스킵
            if (enemy.IsKnockBacking) return;
            
            // 공격 범위 밖에 있을 경우 추적 상태로 전환
            if (!enemy.IsPlayerAttackRange())
            {
                enemy.ChangeState<ChaseState>();
                return;
            }
            
            // 대시 공격은 슬라임 전용
            if (enemy is Slime slime && enemy.CanAttack(slime.LastAttackTime))
            {
                enemy.StartCoroutine(slime.DashAttack());
            }
        }

        public void OnExit(Enemy enemy)
        {

        }
    }
}
