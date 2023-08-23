using System;
using System.Collections.Generic;
using System.Linq;
using _SDK.Inventory;
using Assets._GAME.Scripts.Game;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _GAME.Scripts.Inventory
{
    public class SkinInventory : AbstractInventory<Skin>
    {
        public List<Skin> Skins { get => Items?.ToList<Skin>(); }

        public override Skin DefaultItem => gameResources.PictureEnemySetting.skin;

        private GameResources gameResources =>  GameManager.Instance.Resources;

        public Skin GetActivatingItem()
        {
            var item = gameResources.AllEnemySettings.FirstOrDefault(item => Skin.IsActivatedKey(item.Key));

			return item.Value?.skin ?? DefaultItem;
        }

        public override void Load()
        {
            Items = gameResources.AllEnemySettings.Values.Select(settings =>
            {
                Skin item = (Skin)(settings).Entity;

                return item;
            }).ToList();

        }

		public List<Skin> GetListItemsOwned()
		{
            Load();
			List<Skin> ListItemsOwned = new List<Skin>();

            foreach (var skin in Skins)
                if(skin.IsOwned)
					ListItemsOwned.Add(skin);

            return ListItemsOwned;
		}

	}
}