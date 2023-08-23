using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._SDK.Missions
{
    public abstract class AbstractMissionInventory 
    {
        public List<AbstractMission> Misions { get; protected set; }

        public AbstractMission PlayingMission { get; protected set; }
        public AbstractMission LastPlayedMission { get; protected set; }
        public abstract void LoadAllMissions();

        public abstract void LoadNextPlayingMission();

        public abstract void LoadLastPlayedMission();

    }
}