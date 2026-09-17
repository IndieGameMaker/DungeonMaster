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
        #endregion


        #region 슬롯 처리 메서드
        private void SlotSelected(Slot slot)
        {
            // 모든 슬롯을 선택 해제
            ClearAllSelectedSlots();
            
            // 선택된 슬롯만 선택 처리
            slot.isSelected = true;
            slot.selectedMarkImage.enabled = true;
            
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
    }
}
