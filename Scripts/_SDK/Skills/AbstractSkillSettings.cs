using _GAME.Scripts.Inventory;
using Assets._SDK.Entities;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._SDK.Skills
{
    public abstract class AbstractSkillSettings: AbstractEntitySettings<AbstractSkill>
    {
        public abstract AbstractSkill Skill { get; }
    }
}