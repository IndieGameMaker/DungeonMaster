using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

        public bool respawned = false;
        
        // 코루틴 정의
        private IEnumerator ExampleCoroutine()
        {
            Debug.Log("코루틴 시작");
            
            // Thread.Sleep(3500); // Block
            // yield return null;  // 다음 프레임까지 양보
            // yield return new WaitForSeconds(3.5f);  // 지정한 시간(초)동안 메인 메시지루프에게 제어권을 양보

            yield return new WaitUntil(() => respawned == true);  // ~ 일때까지 제어권 양보
            
            // yield return new WaitWhile(() => !respawned); // ~ 하는 동안 계속 제어권 양보

            // yield return StartCoroutine(다른 코루틴); // 다른 코루틴이 완료될 때까지 제어권을 양보
            
            Debug.Log("코루틴 종료");
        }
        
        public bool isDead = false;

        private IEnumerator DeadCoroutine()
        {
            while (!isDead)
            {
                // 로직 처리
                yield return null; 
            }
        }
    }
}
