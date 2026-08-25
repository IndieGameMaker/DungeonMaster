using System;
using System.Collections.Generic;
using UnityEngine;
using DungeonMaster.Character.Enemy.FSM;
using UnityEngine.InputSystem;

namespace DungeonMaster.Character.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        // 상태 머신 변수 선언
        protected StateMachine _stateMachine;
        
        // 상태 전환 헬퍼 메서드 
        public void ChangeState<T>() where T : IState
        {
            // 딕셔너리에서 저장된 상태(State)를 가져와서 전환
            if (_states.TryGetValue(typeof(T), out IState state))
            {
                _stateMachine.ChangeState(state);
            }
        }

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
            // 상태머신 초기화
            _stateMachine = new StateMachine(this);
            
            // 초기 상태 설정(IdleState)
            ChangeState<IdleState>();
            
            // 상태 초기화 호출
            InitStates();
            // 컴포넌트 초기화
            InitComponents();
        }

        private void Update()
        {
            // 상태 머신 업데이트
            _stateMachine.Update();
            
            TestFSM();
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
