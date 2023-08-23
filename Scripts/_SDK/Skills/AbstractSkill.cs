using Assets._SDK.Entities;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class AbstractSkill : AbstractEntity, ISkill
{
    public abstract List<ISkillLevel> SkillLevels { get; }

    public ISkillLevel GetSkillLevelBy(int levelIndex) => SkillLevels.First(skillLevel => skillLevel.Index == levelIndex);

}
