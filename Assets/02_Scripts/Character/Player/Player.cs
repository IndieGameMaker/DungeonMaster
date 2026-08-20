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
        
        // Facing 처리를 위한 Weapon Arm
        protected Transform _weaponArm;
        
        // 애니메이터 파라메터 해시(Hash)값 미리 추출
        protected static readonly int hashIsWalk = Animator.StringToHash("IsWalk");

        #region 유니티 생명주기 메서드

        protected virtual void Awake()
        {
            // 초기 체력 설정
            _currHp = _maxHp;
            // 컴포넌트 캐싱
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _inputHandler = GetComponent<InputHandler>();
            
            // weaponArm 설정
            _weaponArm = transform.Find("Arm");
            
            //weaponArm = this.gameObject.GetComponent<Transform>().Find("Arm");
            // GameObject.Find  => Root 에서 처음부터 재귀적으로 검색
            // Transform.Find   => 해당 Transform 의 위치에서 부터 재귀적으로 검색
        }

        protected void OnEnable()
        {
            _inputHandler.OnMoveAction += OnMove;
            _inputHandler.OnAttackAction += OnAttack;
            _inputHandler.OnInteractAction += OnInteract;
        }
        
        protected void OnDisable()
        {
            _inputHandler.OnMoveAction -= OnMove;
            _inputHandler.OnAttackAction -= OnAttack;
            _inputHandler.OnInteractAction -= OnInteract;
        }
        #endregion

        #region 공통 메서드
        // Facing 처리
        private void FlipDirection(bool facingRight)
        {
            if (facingRight)
            {
                // 오른쪽 방향
                _spriteRenderer.flipX = false;
                _weaponArm.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                // 왼쪽 방향
                _spriteRenderer.flipX = true;
                _weaponArm.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        #endregion
        
        #region 입력 처리 메서드

        /* 벡터의 정규화 (Normalize)
         *
         * a + b = c
         * c.normalized
         */
        
        private void OnMove(Vector2 ctx)
        {
            Debug.Log($"이동: {ctx} , 벡터 크기: {ctx.magnitude}");

            if (_isDead) return;
            // 이동 처리
            _rb.linearVelocity = ctx * _moveSpeed;
            // 방향 전환
            if (ctx.x != 0)
            {
                FlipDirection(ctx.x > 0);
            }
            // 애니메이션 처리
            _animator.SetBool(hashIsWalk, ctx.sqrMagnitude > 0f);
        }

        private void OnAttack()
        {
            
        }

        private void OnInteract(bool obj)
        {
            
        }
        #endregion
    }
}
