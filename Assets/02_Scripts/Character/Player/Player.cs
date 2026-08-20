using UnityEngine;
using DungeonMaster.InputSystem;

namespace DungeonMaster.Character.Player
{
    public class Player : MonoBehaviour
    {
        #region 기본 스텟

        [SerializeField] private float _maxHp = 100f;
        [SerializeField] private float _currHp = 100f;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _attackDamage = 20f;
        [SerializeField] private float _attackCooldown = 0.5f;

        #endregion
    }
}
