using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/SkillManager")]
public class SkillManager : ScriptableObject
{
    [SerializeField] private List<Skill> skills;

    public Skill GetSkill(int index)
    {
        return skills[index];
    }
}
