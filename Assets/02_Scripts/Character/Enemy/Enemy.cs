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
        public void ChangeState(IState newState)
        {
            _stateMachine.ChangeState(newState);
        }

        // 상태를 저장할 딕셔너리 선언
        protected Dictionary<Type, IState> _states;
        
        // 상태를 초기화 시키는 추상 메서드
        protected abstract void InitStates();
        
        #region 유니티 생명주기

        protected void Awake()
        {
            // 상태머신 초기화
            _stateMachine = new StateMachine(this);
            
            // 초기 상태 설정(IdleState)
            ChangeState(new IdleState());
            
            // 상태 초기화 호출
            InitStates();
        }

        private void Update()
        {
            // 상태 머신 업데이트
            _stateMachine.Update();
            
            TestFSM();
        }
        #endregion

        #region 테스트 코드
        private void TestFSM()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                ChangeState(new IdleState());
            }
            
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                ChangeState(new ChaseState());
            }
            
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                ChangeState(new AttackState());
            }
        }

        #endregion
    }
}
