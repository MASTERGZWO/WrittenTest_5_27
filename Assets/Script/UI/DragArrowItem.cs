using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WrittenTest {
    public class DragArrowItem : MonoBehaviour {
        #region 变量

        public Transform tfm_root;
        public Transform tfm_arrowHead;
        public GameObject go_ArrowStem;
        public List<GameObject> m_ArrowStem;
        public ClickPenetrate m_ClickPenetrate;

        //0：起始点 1：控制点1 2：控制点2 3：终点
        private Vector2[] m_Points = new Vector2[4];
        private Vector2 m_ControlPoint1 = new Vector3(-1, 1);
        private Vector2 m_ControlPoint2 = new Vector3(0.1f, 1.1f);

        private Ray m_MouseRay;
        private RaycastHit[] m_HitInofos;

        private const uint CONST_STEP_NUM = 20;
        private const uint CONST_LOG_DIVISOR = 10;
        private const float CONST_SCALE_DIVISOR = 0.3f;

        #endregion

        #region ui绑定

        private void StaticBind() {
            tfm_root = gameObject.transform;
            tfm_arrowHead = tfm_root.Find("ArrowHead");
            m_ClickPenetrate = tfm_root.GetComponent<ClickPenetrate>();
        }

        private void DynamicBind() {
            go_ArrowStem = tfm_root.Find("Stem01").gameObject;
        }

        #endregion

        #region EventFunctions

        private void Awake() {
            StaticBind();
            DynamicBind();
        }

        private void Start() {
            Init();
        }

        private void Update() {
            UpdatePoints();
            List<Vector2> TsPoints = UIUtility.Instance.CreateThirdOrderCurve(m_Points, CONST_STEP_NUM);
            Refresh(TsPoints);
        }

        #endregion

        private void Init() {
            m_Points[0] = new Vector2(Screen.width / 2, Screen.height / 2);
            gameObject.SetActive(false);
            m_ArrowStem = new List<GameObject>();
            m_ArrowStem.Add(tfm_arrowHead.gameObject);
            for (int i = 0; i < CONST_STEP_NUM - 1; i++) {
                GameObject TsPoint = Instantiate<GameObject>(go_ArrowStem, tfm_root, true);
                TsPoint.SetActive(true);
                TsPoint.transform.SetAsFirstSibling();
                m_ArrowStem.Add(TsPoint);
            }
        }

        public void SetStartPoint(Vector3 startPoint) {
            m_Points[0] = startPoint;
        }

        private void UpdatePoints() {
            m_Points[3].x = Input.mousePosition.x;
            m_Points[3].y = Input.mousePosition.y;
            m_Points[1] = m_Points[0] + (m_Points[3] - m_Points[0]) * m_ControlPoint1;
            m_Points[2] = m_Points[0] + (m_Points[3] - m_Points[0]) * m_ControlPoint2;
            Debug.DrawLine(m_Points[0], m_Points[1]);
            Debug.DrawLine(m_Points[3], m_Points[2]);
        }

        private void Refresh(List<Vector2> points) {
            for (var index = 0; index < m_ArrowStem.Count; index++) {
                var item = m_ArrowStem[index];
                Vector3 TsPostion = new Vector3(points[index].x, points[index].y, 0);
                item.transform.position = TsPostion;

                //旋转
                if (index > 0) {
                    Vector3 vec3_angle = m_ArrowStem[index - 1].transform.position - item.transform.position;
                    Vector2 vec2_angle = new Vector2(vec3_angle.x, vec3_angle.y);
                    var Euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, vec2_angle));
                    item.transform.rotation = Quaternion.Euler(Euler);
                }

                //缩放
                float ScaleDivisor = Mathf.Log((int) CONST_STEP_NUM - index + 1, CONST_LOG_DIVISOR);
                item.transform.localScale = new Vector3(ScaleDivisor, ScaleDivisor, ScaleDivisor) * CONST_SCALE_DIVISOR;
            }

            //对大箭头特殊处理
            m_ArrowStem[0].transform.rotation = m_ArrowStem[1].transform.rotation;
            m_ArrowStem[0].transform.localScale = new Vector3(2, 2, 2) * CONST_SCALE_DIVISOR;
        }

        public void TurnRed() {
            foreach (var stem in m_ArrowStem) {
                stem.GetComponent<Image>().color = Color.red;
            }
        }

        public void TurnWhite() {
            foreach (var stem in m_ArrowStem) {
                stem.GetComponent<Image>().color = Color.white;
            }
        }
    }
}