using DungeonMaster.Character.Enemy;
using DungeonMaster.Character.Enemy.FSM;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy), true)]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        Enemy enemy = (Enemy)target;
        
        EditorGUILayout.Space();
        GUI.enabled = Application.isPlaying;
        
        if (GUILayout.Button("Idle 상태"))
        {
            enemy.ChangeState<IdleState>();
        }
        if (GUILayout.Button("Chase 상태"))
        {
            enemy.ChangeState<ChaseState>();
        }
        if (GUILayout.Button("Attack 상태"))
        {
            enemy.ChangeState<AttackState>();
        }
        // 주의사항 true 전환
        GUI.enabled = true;
    }
}