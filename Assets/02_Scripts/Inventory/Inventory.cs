using System;
using System.Collections.Generic;
using UnityEngine;

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
                if (_slots[i].itemData == null) return i;
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
            _slots[emptySlot].itemData = item;
            Debug.Log($"{item.itemName} 아이템이 인벤토리에 추가되었습니다.");
            return true;
        }

        #endregion
    }
}
