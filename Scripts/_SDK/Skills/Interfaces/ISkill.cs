using _SDK.Entities;
using System.Collections.Generic;

namespace Assets._SDK.Skills
{
    public interface ISkill
    {
        public List<ISkillLevel> SkillLevels { get; }

        public ISkillLevel GetSkillLevelBy(int levelIndex);
    }
}