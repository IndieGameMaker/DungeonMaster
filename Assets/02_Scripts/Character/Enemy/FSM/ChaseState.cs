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
                    Debug.Log($"플레이어 검출 : {Time.time}");
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