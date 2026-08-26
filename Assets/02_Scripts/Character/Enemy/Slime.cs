using System;
using System.Collections;
using System.Collections.Generic;
using DungeonMaster.Character.Enemy.FSM;
using UnityEngine;

namespace DungeonMaster.Character.Enemy
{
    public class Slime : Enemy
    {
        protected override void InitStates()
        {
            _states = new Dictionary<Type, IState>
            {
               // Indexer 방식 값 추가
               [typeof(IdleState)] = new IdleState(),
               [typeof(ChaseState)] = new ChaseState(),
               [typeof(AttackState)] = new AttackState()
            };
            
            Debug.Log("Slime 상태 초기화 완료");
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(ExampleCoroutine());
            // StartCoroutine("ExampleCoroutine");
            // StartCoroutine(nameof(ExampleCoroutine));
        }
        
        // 코루틴 정의
        private IEnumerator ExampleCoroutine()
        {
            Debug.Log("코루틴 시작");
            yield return null;  // 다음 프레임까지 양보
            Debug.Log("코루틴 종료");
        }
    }
}
