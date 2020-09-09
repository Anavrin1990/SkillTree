using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UniRx;
using UnityEngine;
using Zenject;

public class SkillManager
{
    private GameStats gameStats;
    private SkillStorage _skillStorage;
    
    public SkillManager
    (
        SkillStorage skillStorage,
        GameStats gameStats
    )
    {
        this.gameStats = gameStats;
        this._skillStorage = skillStorage;
    }

    public IObservable<bool> CanLearnSkill(IObservable<Skill> skillStream)
    {
        return skillStream
            .Where(x => x != null)
            .CombineLatest(gameStats.Score, (skill, score) =>
        {
            var hasRequiredSkills = skill.Requirements
                .Select(id => _skillStorage.GetSkills()[id].State.Value)
                .Contains(Skill.SkillState.learned);

            var hasRequiredScore = score >= skill.Cost;

            return hasRequiredSkills && hasRequiredScore;
        });
    }

    public IObservable<Skill.SkillState> GetSkillState(Skill skill)
    {
        var skillStorage = _skillStorage.GetSkills();
        
        return skillStorage[skill.Id].Requirements
            .Select(id => skillStorage[id].State)
            .CombineLatest()
            .CombineLatest(skill.State, gameStats.Score, (requirementsState, state, score) =>
            {
                if (state == Skill.SkillState.learned)
                {
                    return Skill.SkillState.learned;
                }
                
                if (requirementsState.Contains(Skill.SkillState.learned) && score >= skill.Cost)
                {
                    return Skill.SkillState.available;
                }
                
                return Skill.SkillState.locked;
            });
    }

    public IObservable<bool> CanForgetSkill(IObservable<Skill> skillStream)
    {
        var skillStorage = _skillStorage.GetSkills();

        return skillStream
            .Where(x => x != null)
            .Select(skill => (skill, skill.DependentSkills.Select(id => skillStorage[id])))
            .Select(skills =>
            {
                var (currentSkill, dependentSkills) = skills;

                foreach (var dependentSkill in dependentSkills)
                {
                    if (dependentSkill.State.Value != Skill.SkillState.learned) continue;

                    var requirements = dependentSkill.Requirements
                        .Where(id => id != currentSkill.Id)
                        .Select(id => skillStorage[id])
                        .ToArray();

                    if (requirements.IsEmpty())
                        return false;

                    return requirements
                        .Select(skill => skill.State.Value == Skill.SkillState.learned)
                        .Contains(true);
                }

                return true;
            });
    }
    
    public void ChangeScore(int change)
    {
        gameStats.Score.Value += change;
    }
}