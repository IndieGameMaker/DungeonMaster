using UnityEngine;

namespace DungeonMaster.Character.Enemy.FSM
{
    public class AttackState : IState
    {
        public void OnEnter(Enemy enemy)
        {
            Debug.Log("Attack 진입");
            // 애니메이션을 Attack 변경
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
