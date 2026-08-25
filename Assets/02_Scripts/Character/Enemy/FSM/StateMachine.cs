namespace DungeonMaster.Character.Enemy.FSM
{
    public class StateMachine
    {
        private Enemy _enemy;
        
        // 생성자
        public StateMachine(Enemy enemy)
        {
            _enemy = enemy;
        }
        
        // 현재 상태를 저장하는 변수
        public IState _currentState;
        
        // 상태를 전환하는 메서드
        public void ChangeState(IState newState)
        {
            _currentState?.OnExit(_enemy);
            _currentState = newState;
            _currentState?.OnEnter(_enemy);
        }
        
        // 상태를 업데이트하는 메서드
        public void Update()
        {
            _currentState?.OnUpdate(_enemy);
        }
    }
}
