using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonMaster.InventorySystem
{
    public class Slot : MonoBehaviour
    {
        // 슬롯 인덱스
        public int slotIndex;
        // 선택여부
        public bool isSelected;
        // 디폴드 슬롯 여부
        public bool isDefaultSelected;
        
        // 슬롯 하위에 있는 UI 객체
        public GameObject item;
        public Image selectedMarkImage;
        public Image itemImage;
        public TextMeshProUGUI equipText;
    }
}
