using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonMaster.InventorySystem
{
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

        // 슬롯 선택시 발생시킬 이벤트 선언
        public static event Action<Slot> OnSlotSelected;
        
        private void Start()
        {
            InitSlot();
        }
        
        #region 초기화

        private void InitSlot()
        {
            // 디폴트 선택여부 확인
            isSelected = isSelected || isDefaultSelected;
            selectedMarkImage.enabled = isSelected;
        }
        #endregion
        
        #region 마우스 이벤트 처리
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 마우스 오버 (Hovering)
            selectedMarkImage.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 마우스 아웃
            selectedMarkImage.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 선택 처리
            isSelected = !isSelected;
            selectedMarkImage.enabled = isSelected;
            
            // 슬롯 선택 이벤트 발생
            OnSlotSelected?.Invoke(this);
        }
        #endregion
    }
}
