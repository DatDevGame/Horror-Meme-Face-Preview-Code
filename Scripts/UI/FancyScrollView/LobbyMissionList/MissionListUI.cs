/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using _GAME.Scripts.Inventory;
using System.Collections.Generic;
using System.Collections;

namespace FancyScrollView.LobbyMissionList
{
    class MissionListUI : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;
        [SerializeField] Button prevCellButton = default;
        [SerializeField] Button nextCellButton = default;
        //[SerializeField] Text selectedItemInfo = default;

        public Transform content;
        [SerializeField] private Sprite sprite;

        private Dictionary<int, MissionSettings> missionSettings;
        private MissionInventory missionInventory => GameManager.Instance.MissionInventory;
        private int SelectedOrder;
        [SerializeField] private UnlockMissionPanel _unlockMissionPanel;

        public MissionCell CurrentMissionCell { get; set; }
        void Start()
        {
            prevCellButton?.onClick.AddListener(scrollView.SelectPrevCell);
            nextCellButton?.onClick.AddListener(scrollView.SelectNextCell);
            scrollView.OnSelectionChanged(OnSelectionChanged);

            missionSettings = GameManager.Instance.Resources.AllMissionSettings;
            ItemData[] items = Enumerable.Range(0, missionSettings.Values.Count)
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);
            scrollView.SetUnlockCellAction(ShowUnlockPanel);

            var setDataItem = Enumerable.Range(0, missionSettings.Values.Count)
                .Select(i => new AddData(content, i, missionInventory))
                .ToArray();
        }

        private void OnEnable()
        {
            StartCoroutine(OnSelectBefore());
        }

        public void RefeshUI()
        {
			int orderMission = missionInventory.GetIndexActiveMisionSetting() - 1;
			scrollView.SelectCell(orderMission);
		}

        public void ShowUnlockPanel(Mission mission)
        {
            _unlockMissionPanel.SetData(mission);
            _unlockMissionPanel.gameObject.SetActive(true);
        }

        IEnumerator OnSelectBefore()
        {
            yield return new WaitForSeconds(0.5f);
            RefeshUI();
		}
        void OnSelectionChanged(int index)
        {
            missionInventory.SetActiveMission(index);

            int orderMission = missionInventory.GetIndexActiveMisionSetting();

            for (int i = 0; i < missionSettings.Values.Count; i++)
            {
                if (missionSettings.ElementAt(i).Value.Mission.Order == orderMission)
                    SelectedOrder = orderMission;
            }

            //selectedItemInfo.text = $"Selected item info: index {index}";
        }
    }
}
