# Enemy FSM 구조

Unity 2D 던전 게임의 적(Enemy) AI 상태 머신 구조 문서.
대상 코드: `Assets/02_Scripts/Character/Enemy/`

---

## 1. 클래스 구조 (정적 구조)

```
                          ┌───────────────────────────┐
                          │   <<interface>> IState    │
                          ├───────────────────────────┤
                          │ + OnEnter(Enemy)          │
                          │ + OnUpdate(Enemy)         │
                          │ + OnExit(Enemy)           │
                          └─────────────△─────────────┘
                                        │ implements
                    ┌───────────────────┼───────────────────┐
                    │                   │                   │
            ┌───────┴───────┐   ┌───────┴───────┐   ┌───────┴───────┐
            │  IdleState    │   │  ChaseState   │   │  AttackState  │
            │  (정지/대기)   │   │  (추적/이동)   │   │  (공격 실행)   │
            └───────────────┘   └───────────────┘   └───────────────┘
                    ▲                   ▲                   ▲
                    │  ┌────────────────┴────────┐          │
                    └──┤  Dictionary<Type,IState> ├──────────┘
                       │        _states           │  ← Slime.InitStates()에서 생성
                       └────────────┬─────────────┘     인스턴스는 딱 1개씩 (재사용)
                                    │ 보관
      ┌─────────────────────────────┴──────────────────────────────┐
      │                                                            │
┌─────┴──────────────────────────┐              ┌──────────────────┴─────────────┐
│   Enemy : MonoBehaviour        │  1        1  │      StateMachine              │
│         , IDamageable          ├──────────────┤  (순수 C# 클래스, MonoB 아님)   │
├────────────────────────────────┤    소유      ├────────────────────────────────┤
│ # _states : Dictionary         │              │ - _enemy   : Enemy             │
│ # _stateMachine : StateMachine │◀─────────────┤ + _currentState : IState       │
│ # _rb / _spriteRenderer        │  역참조       ├────────────────────────────────┤
│ # _animator / _currHP          │  (생성자)     │ + ChangeState(IState)          │
│ + IsKnockBacking / IsDead      │              │ + Update()                     │
├────────────────────────────────┤              └────────────────────────────────┘
│ ▼ 상태들이 호출하는 "행동" API   │
│ + ChangeState<T>()             │      상태 클래스는 필드를 갖지 않는다
│ + DetectPlayer()               │      → 모든 데이터는 Enemy가 들고 있고
│ + PlayerDetectable()           │        상태는 인자로 받은 enemy를 통해 접근
│ + IsPlayerAttackRange()        │        (Flyweight / 무상태 State 패턴)
│ + CanAttack(lastAttackTime)    │
│ + MoveToPlayer() / StopMoving()│
│ + SetWalk(bool)                │
│ + TakeDamage(float) [virtual]  │
└────────────△───────────────────┘
             │ extends
     ┌───────┴─────────────────────────────────┐
     │  Slime : Enemy                          │
     ├─────────────────────────────────────────┤
     │ + LastAttackTime  { get; private set; } │
     │ - isAttacking : bool                    │
     ├─────────────────────────────────────────┤
     │ # InitStates()          ← 어떤 상태를 쓸지 결정
     │ + DashAttack()          ← IEnumerator (코루틴)
     │ - Knockback()           ← IEnumerator (코루틴)
     │ + TakeDamage() override                 │
     │ - OnTriggerEnter2D()    ← 실제 타격 판정  │
     └─────────────────────────────────────────┘
```

**핵심 포인트**: `Enemy`는 *데이터와 행동*을, `IState` 구현체는 *판단(언제 무엇을 할지)* 만 가진다.
상태 클래스에 필드가 하나도 없어서 `Slime`이 10마리 있어도 상태 인스턴스는 각자 3개씩만 만들어진다.

---

## 2. 실행 흐름 (매 프레임)

```
   Unity Engine
        │
        │ 매 프레임
        ▼
   Enemy.Update()                              Enemy.cs:85
        │
        └──▶ _stateMachine.Update()            StateMachine.cs:27
                    │
                    └──▶ _currentState?.OnUpdate(_enemy)
                                │
                                │  현재 상태에 따라 분기
                                ▼
                    ┌───────────┴───────────┐
                    │  판단 → 필요 시 전환   │
                    └───────────┬───────────┘
                                │
                                ▼
                    enemy.ChangeState<T>()      Enemy.cs:116
                                │
                                │ _states 딕셔너리에서 T 타입 조회
                                ▼
                    StateMachine.ChangeState(newState)
                                │
                        ┌───────┴────────┐
                        ▼                ▼
              ① 이전.OnExit()    ② _currentState = new
                                        ▼
                                ③ 새로운.OnEnter()   ← 여기서 불변식 확립
                                                        (StopMoving, SetWalk 등)
```

---

## 3. 상태 전이도 (동적 구조)

```
                              ┌──────────────────────────┐
                              │  Enemy.Start()           │
                              │  ChangeState<IdleState>  │
                              └────────────┬─────────────┘
                                           │
                                           ▼
    ┌──────────────────────────────────────────────────────────────────────┐
    │                            IdleState                                 │
    │  OnEnter : SetWalk(false)  +  StopMoving()                           │
    │  OnUpdate: PlayerDetectable() && DetectPlayer() ?                    │
    └───────────────┬──────────────────────────────────▲───────────────────┘
                    │                                  │
      플레이어 검출   │                                  │  DetectPlayer() == false
      (chaseDistance │                                  │  (추적 범위 5 밖으로 이탈)
       = 5 이내)     │                                  │
                    ▼                                  │
    ┌──────────────────────────────────────────────────┴───────────────────┐
    │                            ChaseState                                │
    │  OnEnter : SetWalk(true)                                             │
    │  OnUpdate: ┌ IsKnockBacking → StopMoving() + return  (전이 차단)      │
    │            ├ PlayerDetectable() 간격(0.3s) 통과 시에만 판정           │
    │            ├ DetectPlayer() 실패      → IdleState                    │
    │            ├ IsPlayerAttackRange()  ┐                                │
    │            │   && CanAttack()       ┴→ AttackState                   │
    │            └ 그 외                    → MoveToPlayer()  (rb.velocity) │
    └───────────────┬──────────────────────────────────▲───────────────────┘
                    │                                  │
     사정거리(2) 안   │                                  │  IsPlayerAttackRange()
     + 쿨타임(1s) OK │                                  │  == false (사정거리 밖)
                    ▼                                  │
    ┌──────────────────────────────────────────────────┴───────────────────┐
    │                           AttackState                                │
    │  OnEnter : StopMoving()                                              │
    │  OnUpdate: ┌ IsKnockBacking → return           (전이·공격 모두 차단)  │
    │            ├ !IsPlayerAttackRange() → ChaseState                     │
    │            └ CanAttack(LastAttackTime) → StartCoroutine(DashAttack)  │
    └──────────────────────────────────────────────────────────────────────┘

    ※ AttackState → Idle 로 가는 직접 경로는 없음. 반드시 Chase를 경유한다.
```

---

## 4. FSM 밖에서 도는 코루틴

이 프로젝트에서 가장 헷갈리는 지점. **대시와 넉백은 상태가 아니라 코루틴**이라,
FSM과 **병렬로** 돈다.

```
     시간 ────────────────────────────────────────────────────────────▶

  FSM 트랙     [ AttackState ]══════════════════════════════════════
  (매 프레임)         │  ↑                                    ↑
                     │  └── IsKnockBacking 플래그로 스스로 멈춤 ┘
                     │        (return만 할 뿐 상태는 그대로)
                     ▼
  코루틴 트랙   DashAttack()  ──▶ 전진 ─▶ 대기 ─▶ 복귀 ─▶ 끝
  (독립 실행)         ╎
                     ╎ 피격!
                     ╎  Slime.TakeDamage()
                     ╎     ├─ isAttacking = false ──▶ DashAttack이 yield break
                     ╎     └─ StartCoroutine(Knockback())
                     ▼
               Knockback()  ──▶ IsKnockBacking = true
                                    Translate 이동 (0.033s)
                                    WaitForSeconds(1.0s)  ← 경직
                                    IsKnockBacking = false
                                    LastAttackTime = Time.time  ← 쿨타임 리셋
```

```
   ┌─ 두 트랙의 연결 고리 ─────────────────────────────────────────┐
   │                                                             │
   │   IsKnockBacking  :  코루틴 ──(쓰기)──▶ 플래그 ──(읽기)──▶ 상태 │
   │   isAttacking     :  TakeDamage ─(쓰기)─▶ 플래그 ─▶ DashAttack │
   │   LastAttackTime  :  코루틴 ──(쓰기)──▶ 값 ──(읽기)──▶ CanAttack│
   │                                                             │
   │   → 상태 클래스는 코루틴을 직접 알지 못하고,                    │
   │     오직 Enemy/Slime의 플래그를 통해서만 간접적으로 소통한다.    │
   └─────────────────────────────────────────────────────────────┘
```

---

## 5. 한 장 요약

```
  ┌────────────┐  소유   ┌──────────────┐  위임   ┌────────────┐
  │   Enemy    │────────▶│ StateMachine │────────▶│   IState   │
  │  (데이터·   │         │  (전이 관리)  │         │   (판단)    │
  │   행동)     │◀────────┤              │◀────────┤            │
  └────────────┘  참조    └──────────────┘  콜백    └────────────┘
        ▲                                                │
        │                                                │
        └────────────────────────────────────────────────┘
              OnUpdate(enemy) 인자로 다시 Enemy를 받아
              enemy.MoveToPlayer() 같은 "행동"을 호출

     역할 분리:  누가 무엇을 아는가?
     ─────────────────────────────────────────────────
     Enemy        →  자기 스탯, 컴포넌트, 물리적 행동 방법
     StateMachine →  현재 상태와 전이 절차(Exit→교체→Enter)
     IState       →  "지금 상황에서 무엇을 해야 하는가"
     Slime        →  자기만의 공격 연출(대시)과 피격 반응(넉백)
```

---

## 6. 주요 수치 (SlimeSO / Slime.cs)

| 항목 | 값 | 출처 | 비고 |
|---|---|---|---|
| `chaseDistance` | 5 | SlimeSO | Idle → Chase 진입 거리 |
| `attackDistance` | 2 | SlimeSO | Chase → Attack 진입 거리 (상한만 검사) |
| `attackCooldown` | 1s | SlimeSO | `CanAttack()` 판정 |
| `moveSpeed` | 3 | SlimeSO | 추적 이동 속도 |
| `detectInterval` | 0.3s | Enemy.cs | 검출 판정 주기 (매 프레임 아님) |
| `dashDistance` | 0.5 | Slime.cs | 대시 전진 거리 |
| `dashSpeed` | 10 | Slime.cs | 대시 소요 시간 = 0.05s |
| `waitingTime` | 0.2s | Slime.cs | 대시 후 정지 시간 |
| `returnSpeed` | 8 | Slime.cs | 원위치 복귀 속도 |
| `knockbackDistance` | 0.5 | Slime.cs | 넉백 이동 거리 |
| `knockbackSpeed` | 15 | Slime.cs | 넉백 소요 시간 = 0.033s |
| 경직 시간 | 1.0s | Slime.cs | 넉백 이동 후 추가 대기 |
| Warrior 리치 | 1.0 | Warrior.cs | `offset 0.5 + size.x/2 0.5` |

### 알려진 수치 불일치

- `attackDistance`(2) > `dashDistance`(0.5) — 사정거리 최대에서 공격을 시작하면
  대시가 1.5만큼 모자라 `OnTriggerEnter2D`가 발생하지 않는다. 즉 유효타가 나지 않는다.
- `knockbackDistance`(0.5) < Warrior 리치(1.0) — 넉백을 당해도 여전히 플레이어의
  공격 범위 안에 남는다.
- `IsPlayerAttackRange()`는 **상한만** 검사한다. 하한이 없어 밀착 상태(거리 0.2)도
  "사정거리 안"으로 통과하므로, 경직이 풀린 자리에서 그대로 공격이 나간다.
