using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WrittenTest {
    public class ClickCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
        #region 变量
        private Action m_PointerDownEvent;
        private Action m_PointerUpEvent;
        private string m_TargetName;
        private Action m_DragEvent;
        private Action m_RecoverEvent;
        #endregion

        #region 属性
        public Action PointerDownEvent {
            set => m_PointerDownEvent = value;
        }

        public string TargetName {
            set => m_TargetName = value;
        }

        public Action DragEvent {
            set => m_DragEvent = value;
        }

        public Action RecoverEvent {
            set => m_RecoverEvent = value;
        }

        public Action PointUpEvent {
            set => m_PointerUpEvent = value;
        }
        #endregion

        #region 点击拖拽事件
        public void OnPointerDown(PointerEventData eventData) {
            m_PointerDownEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData) {
            m_PointerUpEvent?.Invoke();
        }

        public void OnDrag(PointerEventData eventData) {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count == 0) {
                m_RecoverEvent?.Invoke();
                return;
            }

            foreach (var rayResult in raycastResults) {
                if (rayResult.gameObject.tag == m_TargetName) {
                    m_DragEvent?.Invoke();
                    return;
                }
            }

            m_RecoverEvent?.Invoke();
        }
        #endregion
    }
}