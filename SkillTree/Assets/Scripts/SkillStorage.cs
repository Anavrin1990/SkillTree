using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/SkillStorage")]
public class SkillStorage : ScriptableObject
{
    [SerializeField] private List<Skill> skills;

    private Dictionary<Skill.SkillId, Skill> skillDictionary = new Dictionary<Skill.SkillId, Skill>();
    
    public Dictionary<Skill.SkillId, Skill> GetSkills()
    {
        if (skillDictionary.IsEmpty())
        {
            InstantiateSkills();
            SetAsRequirementTo();
        }
        
        return skillDictionary;
    }

    private void InstantiateSkills()
    {
        foreach (var skill in skills.Select(Instantiate))
        {
            skillDictionary[skill.Id] = skill;
        }
    }

    private void SetAsRequirementTo()
    {
        foreach (var skill in skillDictionary)
        {
            foreach (var id in skill.Value.Requirements)
            {
                skillDictionary[id].AsRequirementTo.Add(skill.Value.Id);
            }
        }
    }
}
