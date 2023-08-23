/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using _GAME.Scripts.Inventory;

namespace FancyScrollView.LobbyMissionList
{
    class MissionCell : FancyCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;
        [SerializeField] Button button = default;
        [SerializeField] Image SelectImage;
        [SerializeField] Sprite sprite;

        private bool isUnlock;

        [Header("Item")]
        [SerializeField] private Image BackGroundImg;
        [SerializeField] private Image DifficultyImg;
        [SerializeField] private Image DescriptionLabel;
        [SerializeField] private GameObject LockImage;
        [SerializeField] private TMP_Text Name;
        [SerializeField] private TMP_Text Description;
        [SerializeField] private Image PhotoMission;
        [SerializeField] private TMP_Text PriceText;
        [SerializeField] private Image Outline;
        [SerializeField] private Image EffectBg;

        private Mission _mission;
        private void GoUnLockMission()
        {
            Context.OnCellUnlockRequest?.Invoke(_mission);
        }

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        public void SetDataItem(Mission mission)
        {
            this._mission = mission;

            Name.SetText(string.Format("{0}. {1}", mission.Order, mission.Name));
            Description.SetText(mission.Description.Replace("xx", mission.TargetWin + ""));
            BackGroundImg.sprite = mission.Avatar;
            DifficultyImg.sprite = mission.DiffcultyAvatar;
            DescriptionLabel.sprite = mission.ModeGamePlayAvatar;
            HasGetData = true;
            PriceText.SetText($"{mission.MoneyToUnlock}");

            Outline.color = mission.IsSpecial ?  new Color32(210, 19, 228, 255) : new Color32(228, 19, 19, 255);
            EffectBg.color = mission.IsSpecial ? new Color32(210, 19, 228, 255) : new Color32(228, 19, 19, 255);

            if (mission.PhotoTypeMission != PhotoTypeMission.None)
                PhotoMission.gameObject.SetActive(true);

            if (mission.Order == 1)
                mission.Own();

            isUnlock = mission.IsOwned;
            LockImage.SetActive(!isUnlock);
		}

        public override void Initialize()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
            LockImage.GetComponent<Button>().onClick.AddListener(GoUnLockMission);
            image.sprite = sprite;
            HasGetData = false;
        }
        public override void UpdateContent(ItemData itemData = null)
        {
            message.text = itemData?.Message;

            var isSelected = Context.SelectedIndex == Index;

            SelectImage.gameObject.SetActive(isSelected);

            if (isSelected)
                this.transform.SetAsLastSibling();

            LockImage.GetComponent<Button>().enabled = isSelected;
        }
        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }
            animator.speed = 0;
        }
        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        // Tiếng nhật thật à .___.
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
