using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class AttackState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            Debug.Log("Attack 진입");
            
            enemy.StopMoving();
            
            // 대시 공격은 슬라임 전용
            if (enemy is Slime slime && enemy.CanAttack(slime.LastAttackTime))
            {
                enemy.StartCoroutine(slime.DashAttack());
            }
        }

        public void OnUpdate(Enemy enemy)
        {
            Debug.Log("Attack 갱신");
        }

        public void OnExit(Enemy enemy)
        {
            Debug.Log("Attack 종료");
        }
    }
}
