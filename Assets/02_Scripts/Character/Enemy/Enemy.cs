using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DungeonMaster.Character.Enemy.FSM;
using UnityEngine.InputSystem;

namespace DungeonMaster.Character.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("기본 스텟")] 
        [SerializeField] protected EnemySO _enemySO;

        [Header("주인공 레이어 마스크")]
        [SerializeField] protected LayerMask _playerMask;
        
        // 상태 머신 변수 선언
        protected StateMachine _stateMachine;
        
        // 상태 머신 프로퍼티
        public StateMachine StateMachine => _stateMachine;
        // 현재 상태 표기
        public string CurrentStateName => StateMachine?._currentState?.GetType().Name ?? "None";

        // 상태를 저장할 딕셔너리 선언
        protected Dictionary<Type, IState> _states;
        
        // 상태를 초기화 시키는 추상 메서드
        protected abstract void InitStates();
        
        // 컴포넌트 캐싱
        protected Rigidbody2D _rb;
        protected SpriteRenderer _spriteRenderer;
        protected Animator _animator;
        
        // 애니메이션 해시 추출
        protected static readonly int hashIsWalk = Animator.StringToHash("IsWalk");
        protected static readonly int hashHit = Animator.StringToHash("Hit");
        
        #region 유니티 생명주기

        protected void Awake()
        {
            // 상태 초기화 호출
            InitStates();
            // 컴포넌트 초기화
            InitComponents();
        }

        protected void Start()
        {
            // 상태머신 초기화                             
            _stateMachine = new StateMachine(this); 
                                        
            // 초기 상태 설정(IdleState)                  
            ChangeState<IdleState>();                       
        }

        private void Update()
        {
            // 상태 머신 업데이트
            _stateMachine.Update();
        }
        #endregion

        #region 초기화 메서드

        private void InitComponents()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        #endregion

        #region 상태 관련 메서드
        // 상태 전환 헬퍼 메서드 
        public void ChangeState<T>() where T : IState
        {
            // 딕셔너리에서 저장된 상태(State)를 가져와서 전환
            if (_states.TryGetValue(typeof(T), out IState state))
            {
                _stateMachine.ChangeState(state);
            }
        }
        
        // 가장 가까이 있는 주인공을 검출
        public Transform target;
        public bool DetectPlayer()
        {
            // (원점, 반지름, 레이어마스크)
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _enemySO.chaseDistance, _playerMask);
            
            // 가장 가까운 플레이어 검출
            // LINQ (SQL Select, From, Where, OrderBy, Having, Join, ...)
            if (colliders.Length > 0)
            {
                // A, B  Vector2.Distance(A, B)
                // A, B  (A - B).magnitude
                // A, B  (A - B).sqrMagnitude
                target = colliders
                    .OrderBy(c => (c.transform.position - transform.position).sqrMagnitude)
                    .First()
                    .transform;
                return target != null;
            }

            target = null;
            return false;
        }
        #endregion
        
        #region 테스트 코드
        private void TestFSM()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                ChangeState<IdleState>();
            }
            
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                ChangeState<ChaseState>();
            }
            
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                ChangeState<AttackState>();
            }
        }

        #endregion
    }
}
