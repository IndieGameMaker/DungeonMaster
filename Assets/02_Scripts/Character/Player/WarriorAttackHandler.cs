using UnityEngine;
using DungeonMaster.Character.Player;

public class WarriorAttackHandler : MonoBehaviour
{
    private Warrior _warrior;

    private void Start()
    {
        _warrior = transform.root.GetComponent<Warrior>();
    }
    
    // 공격 애니메이션 이벤트
    public void OnAttackAnimExtEvent()
    {
        _warrior.OnAttackAnimEvent();
    }
}
