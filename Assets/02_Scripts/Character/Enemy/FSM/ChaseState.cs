using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class ChaseState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            Debug.Log("Chase 진입");
            // 애니메이션을 Chase 변경
        }

        public void OnUpdate(Enemy enemy)
        {
            Debug.Log("Chase 갱신");
            // 플레이어와의 거리를 측정하고 공격사정거리 이내이면 AttackState로 변경
        }

        public void OnExit(Enemy enemy)
        {
            Debug.Log("Chase 종료");
        }
    }
}
