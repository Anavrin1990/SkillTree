using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class SkillManager
{
    private GameStats gameStats;
    private SkillStorage skillStorage;
    
    public SkillManager
    (
        SkillStorage skillStorage,
        GameStats gameStats
    )
    {
        this.gameStats = gameStats;
        this.skillStorage = skillStorage;
    }

    public IObservable<bool> CanLearnSkill(IObservable<Skill> skillStream)
    {
        return skillStream.CombineLatest(gameStats.Score, (skill, score) =>
        {
            var hasRequiredSkills = skill.Requirements
                .Select(id => skillStorage.GetSkills()[id].IsLearned.Value)
                .Contains(true);

            var hasRequiredScore = score >= skill.Cost;

            return hasRequiredSkills && hasRequiredScore;
        });
    }
}