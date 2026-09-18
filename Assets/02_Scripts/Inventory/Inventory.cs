using System;
using System.Collections.Generic;
using DungeonMaster.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonMaster.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        // 슬롯
        [SerializeField] private List<Slot> _slots;
        
        // 아이템 저장 배열
        private ItemData[] _items;
        // 선택된 슬롯의 인덱스 
        private int _selectedSlotIndex = -1;

        // Player 참조
        private Player _player;
        
        #region 유니티 생명주기

        private void Awake()
        {
            // 아이템 초기화
            _items = new ItemData[_slots.Count];
            // 슬롯 인덱스 설정
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].slotIndex = i;
            }
            
            // 첫 번째 슬롯을 기본 선택
            _slots[0].isDefaultSelected = true;
            _selectedSlotIndex = 0;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("PLAYER")?.GetComponent<Player>();
            
            // var playerObj = GameObject.FindGameObjectWithTag("PLAYER");
            // playerObj.TryGetComponent(out _player);
        }

        private void Update()
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                if (_selectedSlotIndex != -1 && _items[_selectedSlotIndex] != null)
                {
                    UseItem(_selectedSlotIndex);
                }
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (_selectedSlotIndex != -1 && _items[_selectedSlotIndex] != null)
                {
                    EquipItem(_selectedSlotIndex);
                }                
            }
        }
        
        private void OnEnable()
        {
            Slot.OnSlotSelected += SlotSelected;
        }

        private void OnDisable()
        {
            Slot.OnSlotSelected -= SlotSelected;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("아이템 초기 지급"))
            {
                ItemData hpPotion = Resources.Load<ItemData>("ItemData/HpPotionLarge");
                AddItem(hpPotion);
                
                // 기본 장착 무기 (Addressable 변환)
                ItemData sword = Resources.Load<ItemData>("ItemData/RustySword");
                AddItem(sword);
                
                sword = Resources.Load<ItemData>("ItemData/IronSword");
                AddItem(sword);
                
                sword = Resources.Load<ItemData>("ItemData/LegendSword");
                AddItem(sword);
            }
        }
        #endregion


        #region 슬롯 처리 메서드
        private void SlotSelected(Slot slot)
        {
            // 모든 슬롯을 선택 해제
            ClearAllSelectedSlots();
            
            // 선택된 슬롯만 선택 처리
            slot.isSelected = true;
            slot.selectedMarkImage.enabled = true;
            
            _selectedSlotIndex = slot.slotIndex;
            
            Debug.Log($"{slot.name} is selected");
        }

        private void ClearAllSelectedSlots()
        {
            foreach (var slot in _slots)
            {
                if (slot == null) continue;

                slot.isSelected = false;
                slot.selectedMarkImage.enabled = false;
            }
        }
        #endregion

        #region 아이템 처리 메서드
        // 빈 슬롯 인덱스 찾기
        private int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].ItemData == null) return i;
            }

            return -1;
        }
        
        // 아이템 추가
        private bool AddItem(ItemData item)
        {
            // 빈 슬롯 검색
            int emptySlot = FindEmptySlot();
            if (emptySlot == -1)
            {
                Debug.Log("빈 슬롯이 없습니다.");
                return false;
            }
            
            // 실제로 아이템 추가
            _items[emptySlot] = item;
            _slots[emptySlot].ItemData = item;
            Debug.Log($"{item.itemName} 아이템이 인벤토리에 추가되었습니다.");
            return true;
        }

        // 아이템 제거
        private void RemoveItem(int index)
        {
            if (_items[index] != null)
            {
                _items[index] = null;
                _slots[index].ItemData = null;
            }
        }
        
        // 소모성 아이템 사용
        private void UseItem(int index)
        {
            var item = _items[index];

            if (item.ItemType != ItemType.Consumable)
            {
                Debug.Log("소모성 아이템이 아닙니다.");
                return;
            }

            UseConsumableItem(item as ConsumableItemDataSO);
        }
        
        // 장착 아이템 사용
        private void EquipItem(int index)
        {
            var item = _items[index];

            if (item.ItemType != ItemType.Equipment)
            {
                Debug.Log("장착 아이템이 아닙니다.");
                return;
            }
            
            EquipSelectedItem(item as EquipmentItemDataSO);
        }


        #endregion

        #region 아이템 사용 및 장착

        private void UseConsumableItem(ConsumableItemDataSO item)
        {
            // 플레이어 힐 처리
            _player.Heal(item.hpRecovery);
            RemoveItem(_selectedSlotIndex);
        }
        
        private void EquipSelectedItem(EquipmentItemDataSO item)
        {
            // 현재 장착된 무기 해제
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] is EquipmentItemDataSO equippedItem && equippedItem.isEquip)
                {
                    // 장착 해제
                    equippedItem.isEquip = false;
                    _slots[i].ItemData = equippedItem;
                    break;
                }
            }
            
            // 장비 장착
            _player.EquipWeapon(item);
            
            // 새로 장삭할 아이템의 슬롯에 장착 표시
            item.isEquip = true;
            _slots[_selectedSlotIndex].ItemData = item;
        }
        #endregion
    }
}
