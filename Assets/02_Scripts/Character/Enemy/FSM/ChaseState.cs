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
                if (enemy.DetectPlayer())
                {
                    // 공격 범위 내에 플레이어가 있는지 확인
                    float attackRange = Vector2.Distance(enemy.target.position, enemy.transform.position);

                    if (attackRange <= enemy.EnemySO.attackDistance)
                    {
                        // 공격 쿨타임 확인, 공격이 가능할 때만 AttackState 전환
                        if (enemy is Slime slime && !slime.CanAttack(slime.LastAttackTime)) return;
                        
                        // 공격 상태로 전환
                        enemy.ChangeState<AttackState>();
                        return;
                    }
                    
                    enemy.MoveToPlayer();
                }
                else
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