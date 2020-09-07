using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class GameStats : MonoBehaviour
{
    public ReactiveProperty<int> Score;
    public List<Skill> LearnedSkills;

    private SkillManager skillManager;

    [Inject] void Init(SkillManager skillManager)
    {
        this.skillManager = skillManager;
    }
    
    private void Start()
    {
        LearnedSkills.Add(skillManager.GetSkill(0));
    }
}
