using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/SkillFactory")]
public class SkillFactory : ScriptableObject
{
    [SerializeField] private List<Skill> skills;

    public Dictionary<string, Skill> GetSkills()
    {
        var skillDictionary = new Dictionary<string, Skill>();

        foreach (var skill in skills.Select(Instantiate))
        {
            skill.SetAsRequirementTo();
            skillDictionary[skill.Id] = skill;
        }

        return skillDictionary;
    }
}
