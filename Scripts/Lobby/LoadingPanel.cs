using Assets._SDK.Game;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using _GAME.Scripts.Inventory;
using _GAME.Scripts.Shop;

namespace _SDK.UI
{
    public class LoadingPanel : AbstractPanel
	{
        [SerializeField]
        private Button _nextButton;

        public TMP_Text loadingProcessText;
		public Slider loadingBar;

        [SerializeField] private WeaponsShopLoadingPanel _weaponsShopLoadingPanel;

        public WeaponsShopLoadingPanel WeaponsShopLoadingPanel => _weaponsShopLoadingPanel;
		private void Start()
        {
			SetProgressUI(0);

            _nextButton.onClick.AddListener(() =>
            {
                GameManager.Instance.NextGame();
                _nextButton.gameObject.SetActive(false);
            });
        }

		public void SetProgressUI(float progress) // progress is from 0 to 1
		{
			loadingProcessText.SetText(string.Format("{0:0}", (progress * 100)) + "%");
			loadingBar.value = progress;
            _nextButton.gameObject.SetActive(progress >= 1);
        }


	}
}