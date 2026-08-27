using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DungeonMaster.Character.Enemy.FSM;
using DungeonMaster.Core;
using UnityEngine;

namespace DungeonMaster.Character.Enemy
{
    public class Slime : Enemy
    {
        [Header("슬라임 공격 스텟")] 
        [SerializeField] private float _dashSpeed = 10f;
        [SerializeField] private float _returnSpeed = 8f;
        [SerializeField] private float _dashDistance = 0.5f;
        [SerializeField] private float _waitingTime = 0.2f;
        
        // 슬라임 공격 시작위치 저장(원래 위치)
        private Vector2 originPosition;
        // 공격 여부
        private bool _isAttacking = false;
        // 마지막 공격 시간 기록
        public float LastAttackTime {get; private set;} 
        
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

        }

        #region 공격 메서드

        public IEnumerator DashAttack()
        {
            _isAttacking = true;

            // 마지막 공격 시간을 갱신
            LastAttackTime = Time.time;
            // 현재 위치 저장
            originPosition = transform.position;
            
            // 목표 좌표 계산
            // 현재 위치 Vector2
            Vector2 currPosition = new Vector2(transform.position.x, transform.position.y);
            // 공격 방향 벡터를 계산 (벡터의 뺄셈 연산)
            Vector2 dashDir = (target.position - transform.position).normalized;
            // 공격할 좌표를 계산
            Vector2 dashTarget = currPosition + dashDir * _dashDistance;
            
            // 실제로 이동한 시간
            float dashTime = 0f;
            // 이동 시간 계산
            float dashDuration = _dashDistance / _dashSpeed;
            
            // while : 대시 처리 (앞으로 점진적으로 이동)
            while (dashTime < dashDuration)
            {
                transform.position = Vector2.MoveTowards(transform.position, dashTarget, Time.deltaTime * _dashSpeed);
                dashTime += Time.deltaTime;
                yield return null;
            }

            // 잠시 대기
            yield return new WaitForSeconds(_waitingTime);
            
            // while : 원위치로 복귀
            float returnTime = 0f;
            float returnDistance = Vector2.Distance(transform.position, originPosition);
            float returnDuration = returnDistance / _returnSpeed;

            while (returnTime < returnDuration)
            {
                transform.position = Vector2.MoveTowards(transform.position, originPosition, Time.deltaTime * _returnSpeed);
                returnTime += Time.deltaTime;
                yield return null;
            }
            
            _isAttacking = false;
        }

        #endregion

        #region 충돌감지 로직

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"충돌 콜백 호출 : {other.gameObject.name}");
            
            if (other.CompareTag("PLAYER"))
            {
               other.GetComponent<IDamageable>()?.TakeDamage(_enemySO.attackDamage);                 
            }
        }
        
        /*
         * Collider / Collider2D => IsTrigger 체크
         * OnTriggerEnter / OnTriggerStay / OnTriggerExit
         *
         * IsTrigger 언체크
         * OnCollisionEnter / OnCollisionStay / OnCollisionExit
         */
        #endregion

    }
}
