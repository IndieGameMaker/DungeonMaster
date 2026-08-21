using UnityEngine;

namespace DungeonMaster.Character.Player
{
    public class Warrior : Player
    {
        protected override void Attack()
        {
            Debug.Log("공격 실행");
        }
    }
}
