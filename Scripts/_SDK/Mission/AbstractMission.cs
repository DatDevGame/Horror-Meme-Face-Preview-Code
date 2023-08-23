using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Assets._SDK.Entities;
using _SDK.Entities;

namespace Assets._SDK.Missions
{
    [Serializable]
    public class AbstractMission : AbstractEntity, IMission
    {
        public static readonly string ActivatedMissionKey = "ActivatedMissionKey";
        public override int Id => (nameof(AbstractMission) + Name).GetHashCode() + Order;
        [field: SerializeField]
        public int Order { get; set; }

        public bool IsPlayed => ActivatedMissionId == Id;

        public void ActivatePlayed()
        {
            PlayerPrefs.SetInt(ActivatedMissionKey, Id);
        }

        public void DeActivatePlayed()
        {
            PlayerPrefs.DeleteKey(ActivatedMissionKey);
        }

        public static bool HasPlayedMission => PlayerPrefs.HasKey(ActivatedMissionKey);
        public static int ActivatedMissionId => PlayerPrefs.GetInt(ActivatedMissionKey);
    }
}