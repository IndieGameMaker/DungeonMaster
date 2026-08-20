using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonMaster.InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        // InputSystem_Action 의 인스턴스를 저장하기 위한 변수
        private InputSystem_Actions _inputActions;
    
        // 액션을 참조할 변수
        private InputAction _moveAction;
        private InputAction _attackAction;
        private InputAction _interactAction;  

        // 이벤트 선언
        public event Action<Vector2> OnMoveAction;
        public event Action OnAttackAction;
        public event Action<bool> OnInteractAction;
                
        #region 유니티 생명주기

        private void Awake()
        {
            _inputActions = new InputSystem_Actions();
            
            // 액션 Binding
            _moveAction = _inputActions.Player.Move;
            _attackAction = _inputActions.Player.Attack;
            _interactAction = _inputActions.Player.Interact;
        }

        private void OnEnable()
        {
            // 액션 시스템을 활성화
            _inputActions.Enable();

            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;

            _attackAction.performed += OnAttack;

            _interactAction.performed += OnInteract;
            _interactAction.canceled += OnInteract;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            
            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMove;

            _attackAction.performed -= OnAttack;
            
            _interactAction.performed -= OnInteract;
            _interactAction.canceled -= OnInteract;
        }
        #endregion

        /*
         * Vector2 : 2차원 좌표(x,y)를 저장하는 데이터 타입 , 구조체(Struct)
         * Vector3 : 3차원 좌표(x,y,z)를 저장하는 데이터 타입 , 구조체(Struct)
         * 구조체(struct) : 값 타입 (Value Type) , 스텍(Stack), 상속 불가능
         * 클래스(class) : 참조 타입 (Reference Type), 힙(Heap), 상속 가능
         */
        
        // CallBack Method (CallBack Function, Event)
        #region 콜백 메서드
        private void OnMove(InputAction.CallbackContext ctx)
        {
            //Debug.Log($"Move: {ctx.ReadValue<Vector2>()}");
            OnMoveAction?.Invoke(ctx.ReadValue<Vector2>());
        }
        
        private void OnAttack(InputAction.CallbackContext ctx)
        {
            // Debug.Log($"Attack: 공격");
            OnAttackAction?.Invoke();
        }      
        
        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                Debug.Log("상호작용 시작");
                OnInteractAction?.Invoke(true);
            }
            else if (ctx.phase == InputActionPhase.Canceled)
            {
                Debug.Log("상호작용 종료");
                OnInteractAction?.Invoke(false);
            }
        }        
        #endregion
    
    }
}
