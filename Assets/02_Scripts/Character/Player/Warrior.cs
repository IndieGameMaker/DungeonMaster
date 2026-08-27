using DungeonMaster.Core;
using UnityEngine;

namespace DungeonMaster.Character.Player
{
    public class Warrior : Player
    {
        [Header("적 검출 설정")]
        [SerializeField] private Vector2 _size = new Vector2(1f, 2f);
        [SerializeField] private float _offset = 0.5f;
        [SerializeField] private LayerMask _enemyLayer;
        
        [SerializeField] private WarriorSO _warriorSO;
        
        
        #region 유니티 생명주기

        protected override void Awake()
        {
            // 전사의 기본 스텟 설정
            _maxHp = _warriorSO.maxHp;
            _moveSpeed = _warriorSO.moveSpeed;
            _attackDamage = _warriorSO.attackDamage;
            _attackCooldown = _warriorSO.attackCooldown;
            
            Debug.Log($"전사의 방어력: {_warriorSO.defense}");
            base.Awake();
        }

        #endregion

        #region 공격 및 데미지 처리
        
        protected override void Attack()
        {
            Debug.Log("공격 실행");
        }
        
        // 애니메이션 이벤트에서 호출할 메서드
        public void OnAttackAnimEvent()
        {
            // 실제 공격 처리 로직
            // 공격 범위 계산 (박스, 오프셋)
            Vector2 direction = _spriteRenderer.flipX ? Vector2.left : Vector2.right;
            Vector2 center = (Vector2)transform.position + (direction * _offset);
            
            // 추출 OverlapBoxAll
            Collider2D[] colliders = Physics2D.OverlapBoxAll(center, _size, 0, _enemyLayer);

            foreach (var collider in colliders)
            {
                collider.GetComponent<IDamageable>()?.TakeDamage(_warriorSO.attackDamage);

                // if (collider.TryGetComponent<IDamageable>(out IDamageable other))
                // {
                //     other.TakeDamage(_warriorSO.defense);
                // }
            }
        }

        public override void TakeDamage(float damage)
        {
            // 방어력 적용
            float actualDamage = Mathf.Max(damage - _warriorSO.defense, 5f);
            
            base.TakeDamage(actualDamage);
            Debug.Log($"Warrior가 {actualDamage}의 피해를 입었습니다. HP : {_currHp}/{_maxHp}");
        }
        #endregion
    }
}
