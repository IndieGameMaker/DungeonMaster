using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMaster.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<Slot> _slots;
        
        private void OnEnable()
        {
            Slot.OnSlotSelected += SlotSelected;
        }

        private void OnDisable()
        {
            Slot.OnSlotSelected -= SlotSelected;
        }


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
