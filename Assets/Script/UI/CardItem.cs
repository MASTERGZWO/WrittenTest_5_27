using UnityEngine;

namespace WrittenTest {
    public class CardItem : MonoBehaviour {
        #region 变量

        public ClickCard m_ClickCard;
        public GameObject go_Arrow; //在预制体绑定
        public DragArrowItem m_DragArrowItem;
        private const string CONST_ENEMY_NAME = "Enemy";

        #endregion

        #region UI绑定

        private void StaticBind() {
            m_ClickCard = gameObject.transform.GetComponent<ClickCard>();
            m_DragArrowItem = go_Arrow.GetComponent<DragArrowItem>();
        }

        #endregion

        #region EventFunctions

        private void Awake() {
            StaticBind();
        }

        private void Start() {
            Init();
        }

        #endregion

        private void Init() {
            InitClickEvent();
        }

        private void InitClickEvent() {
            m_ClickCard.PointerDownEvent = () => {
                go_Arrow.SetActive(true);
                SetArrowStartPoint();
            };
            m_ClickCard.PointUpEvent = () => {
                go_Arrow.SetActive(false);
                m_DragArrowItem.TurnWhite();
            };
            m_ClickCard.TargetName = CONST_ENEMY_NAME;
            m_ClickCard.DragEvent = m_DragArrowItem.TurnRed;
            m_ClickCard.RecoverEvent = m_DragArrowItem.TurnWhite;
        }

        private void SetArrowStartPoint() {
            var RectTransform = gameObject.GetComponent<RectTransform>();
            Vector3[] Corners = new Vector3[4];
            RectTransform.GetWorldCorners(Corners);
            float x = Corners[0].x + (Corners[3].x - Corners[0].x) / 2;
            float y = Corners[0].y + (Corners[1].y - Corners[0].y) / 2;
            Vector3 StartPosition = new Vector3(x, y, 0);
            m_DragArrowItem.SetStartPoint(StartPosition);
        }
    }
}