using _SDK.Entities;
using Assets._SDK.Entities;
using Assets._SDK.Skills;
using System.Collections;
using UnityEngine;
using static ItemSupportSkillUpgrade;

namespace Assets._SDK.Inventory.Interfaces
{
    public interface IGameItemSuport : IEntity
    {
		public AbstractSkillSettings SkillUpgrade { get; }
		public GameObject ModelItemSupport { get; set; }

		public TypeItem TypeItems { get; set; }
		public Sprite AvatarSkill { get; }

		public int LevelIndex { get; set; }
	}
}