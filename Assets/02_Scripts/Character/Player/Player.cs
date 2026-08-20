using UnityEngine;
using DungeonMaster.InputSystem;

namespace DungeonMaster.Character.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(InputHandler))]
    public class Player : MonoBehaviour
    {
        #region 기본 스텟

        [Header("기본 스텟")]
        [SerializeField] protected float _maxHp = 100f;
        [SerializeField] protected float _currHp = 100f;
        [SerializeField] protected float _moveSpeed = 5f;
        [SerializeField] protected float _attackDamage = 20f;
        [SerializeField] protected float _attackCooldown = 0.5f;

        protected bool _isDead => _currHp <= 0f;
        #endregion

        #region 프로퍼티
        public float MaxHp => _maxHp; // public float MaxHp {get;}
        public float CurrHp => _currHp;
        public float MoveSpeed => _moveSpeed;
        public float AttackDamage => _attackDamage;
        public float AttackCooldown => _attackCooldown;
        #endregion

        #region 컴포넌트 캐싱
        protected Rigidbody2D _rb;
        protected Animator _animator;
        protected SpriteRenderer _spriteRenderer;
        protected InputHandler _inputHandler;
        #endregion
    }
}
