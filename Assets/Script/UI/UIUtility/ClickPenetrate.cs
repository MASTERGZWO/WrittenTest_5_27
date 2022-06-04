using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WrittenTest {
    public class ClickPenetrate : MonoBehaviour, IDragHandler, IEndDragHandler {
        private string m_TargetName;
        private Action m_DragEvent;
        private Action m_EndDragEvent;
        private Action m_RecoverEvent;

        public string TargetName {
            set => m_TargetName = value;
        }

        public Action DragEvent {
            set => m_DragEvent = value;
        }

        public Action EndDragEvent {
            set => m_EndDragEvent = value;
        }

        public Action RecoverEvent {
            set => m_RecoverEvent = value;
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

        public void OnEndDrag(PointerEventData eventData) {
            m_EndDragEvent?.Invoke();
        }
    }
}