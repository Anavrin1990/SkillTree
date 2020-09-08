using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/Skill")]
public class Skill : ScriptableObject
{
    public enum SkillId
    {
        main, walk, run, jump, fight, swim, dash, fly, idle, lie, move
    }
    
    public enum SkillState
    {
        learned, available, locked
    }
    
    public ReactiveProperty<SkillState> State;
    
    public string Title => title;
    public SkillId Id => id;
    public int Cost => cost;
    public IEnumerable<SkillId> Requirements => requirements;
    
    public List<SkillId> AsRequirementTo;
    
    [SerializeField] private string title;
    [SerializeField] private SkillId id;
    [SerializeField] private int cost;
    [SerializeField] private SkillId[] requirements;
}
