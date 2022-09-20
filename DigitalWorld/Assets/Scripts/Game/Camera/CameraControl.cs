using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalWorld.Game
{
    /// <summary>
    /// 战斗相机控制器
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
        /// 主相机
        /// </summary>
        public Camera MainCamera => mainCamera;
        protected Camera mainCamera;

        public float distance = 15;

        public Vector2 distanceClamp = new Vector2(5, 25);
        public Vector2 verticalAngleClamp = new Vector2(-85, 85);

        public const float mouseScrollWheelSpeed = 5;


        /// <summary>
        /// 鼠标右键的相机和目标的偏移量
        /// </summary>
        private Vector3 inputAngles;

        /// <summary>
        /// 最后一次的目标位置
        /// </summary>
        private Vector3 lastedTargetPosition;

        private Vector3 oldMousePosition;
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
            inputAngles = new Vector3(0, 1, 1) * distance;
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

                        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                        {
                            oldMousePosition = Input.mousePosition;
                        }

                        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                        {
                            Vector3 deltaPosition = Input.mousePosition - oldMousePosition;
                            this.oldMousePosition = Input.mousePosition;

                            inputAngles += mouseMoveSpeed * deltaPosition;
                            inputAngles = new Vector3(inputAngles.x, ClampAngle(inputAngles.y, verticalAngleClamp.x, verticalAngleClamp.y), 0);
                        }

                        if (Input.GetMouseButton(0))
                        {
                            standardDirSqrMagnitude = 0;
                        }
                        else if (Input.GetMouseButton(1))
                        {
                            standardDirSqrMagnitude = standardDirSqrMagnitudeLimit;
                        }

                        Quaternion quaternion = Quaternion.identity;
                        Quaternion inputHorizontalRotation = Quaternion.AngleAxis(inputAngles.x, Vector3.up);
                        Quaternion inputVerticalRotation = Quaternion.AngleAxis(inputAngles.y, Vector3.right);
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
        /// 选择单位
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
        /// 放弃选择目标
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
        /// 当前UI是否正在触摸中
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

