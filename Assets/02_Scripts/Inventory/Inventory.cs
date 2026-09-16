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

        private void SlotSelected(Slot slot)
        {
            Debug.Log($"{slot.name} is selected");
        }
    }
}
