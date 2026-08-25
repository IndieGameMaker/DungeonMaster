using System;
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
    }
}
