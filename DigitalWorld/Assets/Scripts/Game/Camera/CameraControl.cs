using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.Game
{
    /// <summary>
    /// ս�����������
    /// </summary>
    public class CameraControl : DreamEngine.Core.Singleton<CameraControl>
    {
        #region Params
        public Transform focused;

        public ControlUnit FocusedUnit
        {
            get
            {
                if (null == focused)
                    return null;

                return focused.GetComponent<ControlUnit>();
            }
        }

        protected Transform trans;
        /// <summary>
        /// �����
        /// </summary>
        public Camera MainCamera => mainCamera;
        protected Camera mainCamera;

        public float distance = 15;

        public Vector2 distanceClamp = new Vector2(5, 25);
        public Vector2 verticalAngleClamp = new Vector2(-85, 85);

        public const float mouseScrollWheelSpeed = 5;


        /// <summary>
        /// ����Ҽ���ˮƽ��ת��
        /// </summary>
        private Quaternion inputHorizontalRotation;
        /// <summary>
        /// ����Ҽ��Ĵ�ֱ��ת��
        /// </summary>
        private Quaternion inputVerticalRotation;

        /// <summary>
        /// ���һ�ε�Ŀ��λ��
        /// </summary>
        private Vector3 lastedTargetPosition;

        /// <summary>
        /// ��һ�εĴ���λ��
        /// </summary>
        private Vector3 prevTouchedPosition;
        public float mouseMoveSpeed = 1;

        private float standardDirSqrMagnitude = 0;
        private const float standardDirSqrMagnitudeLimit = 1;
        #endregion

        #region Common
        private Quaternion GetStandardHorizontalRotation()
        {
            return Quaternion.Euler(0, focused.rotation.eulerAngles.y, 0);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            return Mathf.Clamp(angle, min, max);
        }
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
            mainCamera = GetComponent<Camera>();
        }

        protected override void OnDestroy()
        {
            Events.EventManager.Instance?.UnregisterListener(Events.EEventType.Escape, OnUnselectTarget);

            base.OnDestroy();
        }

        private void Start()
        {
            trans = this.transform;
            inputHorizontalRotation = Quaternion.identity;
            inputVerticalRotation = Quaternion.Euler(45, 0, 0);
        }

        private void LateUpdate()
        {
            if (null != focused)
            {
                if (!IsUITouching)
                {
                    do
                    {
                        float mag = (focused.position - lastedTargetPosition).sqrMagnitude;
                        lastedTargetPosition = focused.position;

                        float axis = Input.GetAxis("Mouse ScrollWheel");
                        this.distance = Mathf.Clamp(this.distance + axis * mouseScrollWheelSpeed, distanceClamp.x, distanceClamp.y);

                        standardDirSqrMagnitude += mag * 10f;

                        if (Input.GetMouseButtonDown(1))
                        {
                            inputHorizontalRotation = GetStandardHorizontalRotation();
                        }

                        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                        {
                            prevTouchedPosition = Input.mousePosition;
                        }

                        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                        {
                            Vector3 deltaPosition = Input.mousePosition - prevTouchedPosition;
                            this.prevTouchedPosition = Input.mousePosition;

                            inputHorizontalRotation *= Quaternion.AngleAxis(deltaPosition.x, Vector3.up);
                            inputVerticalRotation *= Quaternion.AngleAxis(deltaPosition.y, Vector3.right);
                           
                            Vector3 eulerAngles = inputVerticalRotation.eulerAngles;
                            eulerAngles = new Vector3(eulerAngles.x >= 180 ? eulerAngles.x - 360 : eulerAngles.x, 0, 0);
                            eulerAngles = new Vector3(Mathf.Clamp(eulerAngles.x, verticalAngleClamp.x, verticalAngleClamp.y), 0, 0);
                            inputVerticalRotation = Quaternion.Euler(eulerAngles);

                            if (Input.GetMouseButton(0))
                            {
                                standardDirSqrMagnitude = 0;
                            }
                            else if (Input.GetMouseButton(1))
                            {
                                standardDirSqrMagnitude = standardDirSqrMagnitudeLimit;
                            }
                        }

                        Quaternion quaternion = Quaternion.identity;

                        quaternion *= Quaternion.Slerp(inputHorizontalRotation, GetStandardHorizontalRotation(), standardDirSqrMagnitude / standardDirSqrMagnitudeLimit);
                        quaternion *= inputVerticalRotation;

                        trans.position = focused.position + quaternion * -Vector3.forward * distance;
                        trans.LookAt(focused.position);

                        UpdateSelect();


                    } while (false);
                }
            }
        }
        #endregion

        #region Select
        /// <summary>
        /// ѡ��λ
        /// </summary>
        /// <returns></returns>
        private bool UpdateSelect()
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (!IsUITouching)
                {
                    Camera camera = MainCamera;
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                    bool ret = Physics.Raycast(ray, out RaycastHit hit);
                    if (ret)
                    {
                        Collider collider = hit.collider;
                        if (collider.gameObject.TryGetComponent<ControlUnit>(out var target))
                        {
                            SelectTarget(target);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void SelectTarget(ControlUnit target)
        {
            ControlUnit unit = FocusedUnit.Unit;
            ControlSituation situation = unit.Situation;
            situation.SelectTarget(new UnitHandle(target));

            Events.EventManager.Instance.RegisterListener(Events.EEventType.Escape, OnUnselectTarget, Events.EHandleAddMode.Replace);
        }

        /// <summary>
        /// ����ѡ��Ŀ��
        /// </summary>
        /// <param name="args"></param>
        private void OnUnselectTarget(Events.EEventType type, System.EventArgs args)
        {
            Events.EventManager.Instance.UnregisterListener(Events.EEventType.Escape, OnUnselectTarget);

            ControlUnit unit = FocusedUnit.Unit;
            ControlSituation situation = unit.Situation;
            situation.SelectTarget(default);
        }
        #endregion

        #region Input
        /// <summary>
        /// ��ǰUI�Ƿ����ڴ�����
        /// </summary>
        /// <returns></returns>
        private static bool IsUITouching
        {
            get
            {
                return EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0;
            }
        }
        #endregion
    }
}

