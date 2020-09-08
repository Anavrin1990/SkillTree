using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SkillManager
{
    private GameStats gameStats;
    private Dictionary<string, Skill> skills;
    
    public SkillManager
    (
        SkillFactory skillFactory,
        GameStats gameStats
    )
    {
        this.gameStats = gameStats;
        this.skills = skillFactory.GetSkills();
    }
}