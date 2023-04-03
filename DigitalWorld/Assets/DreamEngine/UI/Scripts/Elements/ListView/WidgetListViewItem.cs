﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace DreamEngine.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ListViewItem : MonoBehaviour
    {
        [SerializeField] GameObject m_selectedGameObject;

        public int id { get; private set; }
        public WidgetListView.ESelectType selectType { get; private set; }
        Action<ListViewItem> m_onValueChanged;
        Action<ListViewItem> m_onClicked;//适用于只在Item被单击时做操作的情况

        RectTransform m_rectTransform;
        Button m_button;

        bool m_isSelected;

        public bool isSelected
        {
            get => m_isSelected;
            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;
                    UpdateSelectedUI();
                }
            }
        }

        void Awake()
        {
            id = GetInstanceID();
            m_button = GetComponent<Button>();
            isSelected = false;
            m_button.onClick.AddListener(OnClicked);

            m_rectTransform = GetComponent<RectTransform>();
            m_rectTransform.anchorMin = Vector2.up;
            m_rectTransform.anchorMax = Vector2.up;
            m_rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public void Init(WidgetListView.ESelectType type, Action<ListViewItem> onValueChanged, Action<ListViewItem> onClicked)
        {
            selectType = type;
            m_onValueChanged = onValueChanged;
            m_onClicked = onClicked;
        }

        void OnClicked()
        {
            bool isValueChange = false;
            if (selectType == WidgetListView.ESelectType.Single)
            {
                if (!isSelected)
                    isValueChange = true;
                isSelected = true;
            }
            else
            {
                isValueChange = true;
                isSelected = !isSelected;
            }
            if (isValueChange)
                m_onValueChanged?.Invoke(this);
            m_onClicked?.Invoke(this);
        }

        void ClearSelected()
        {
            isSelected = false;
        }

        protected virtual void UpdateSelectedUI()
        {
            if (m_selectedGameObject != null)
            {
                m_selectedGameObject.SetActive(isSelected);
            }
        }
    }
}
