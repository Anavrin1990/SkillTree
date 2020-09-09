using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/SkillStorage")]
public class SkillStorage : ScriptableObject
{
    [SerializeField] private List<Skill> _skills;

    private Dictionary<Skill.SkillId, Skill> _skillDictionary = new Dictionary<Skill.SkillId, Skill>();
    
    public Dictionary<Skill.SkillId, Skill> GetSkills()
    {
        if (_skillDictionary.IsEmpty())
        {
            InstantiateSkills();
            SetAsRequirementTo();
        }
        
        return _skillDictionary;
    }

    private void InstantiateSkills()
    {
        foreach (var skill in _skills.Select(Instantiate))
        {
            _skillDictionary[skill.Id] = skill;
        }
    }

    private void SetAsRequirementTo()
    {
        foreach (var skill in _skillDictionary)
        {
            foreach (var id in skill.Value.Requirements)
            {
                _skillDictionary[id].DependentSkills.Add(skill.Value.Id);
            }
        }
    }
}
