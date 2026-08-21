using UnityEngine;

namespace DungeonMaster.Character.Player
{
    public class Warrior : Player
    {
        [Header("전사 전용 스텟")] [SerializeField] private float _defense = 10f;

        #region 유니티 생명주기

        protected override void Awake()
        {
            // 전사의 기본 스텟 설정
            _maxHp = 150f;
            _moveSpeed = 4f;
            _attackDamage = 25f;
            _attackCooldown = 0.7f;
            
            Debug.Log($"전사의 방어력: {_defense}");
            base.Awake();
        }

        #endregion
        
        protected override void Attack()
        {
            Debug.Log("공격 실행");
        }

        public override void TakeDamage(float damage)
        {
            // 방어력 적용
            float actualDamage = Mathf.Max(damage - _defense, 5f);
            
            base.TakeDamage(actualDamage);
            Debug.Log($"Warrior가 {actualDamage}의 피해를 입었습니다. HP : {_currHp}/{_maxHp}");
        }
    }
}
