using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/Skill")]
public class Skill : ScriptableObject
{
    public bool isLearned;
    
    public string Name => name;
    public string Id => id;
    public int Cost => cost;
    public IEnumerable<Skill> Requirements => requirements;
    public IEnumerable<Skill> AsRequirementTo => asRequirementTo;
    
    [SerializeField] private string name;
    [SerializeField] private string id;
    [SerializeField] private int cost;
    [SerializeField] private Skill[] requirements;

    private List<Skill> asRequirementTo = new List<Skill>();
    
    public void SetAsRequirementTo()
    {
        foreach (var skill in requirements)
        {
            if (skill)
                skill.asRequirementTo.Add(this);
        }
    }
}
