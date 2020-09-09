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
        return skillStream
            .Where(x => x != null)
            .CombineLatest(gameStats.Score, (skill, score) =>
        {
            var hasRequiredSkills = skill.Requirements
                .Select(id => skillStorage.GetSkills()[id].State.Value)
                .Contains(Skill.SkillState.learned);

            var hasRequiredScore = score >= skill.Cost;

            return hasRequiredSkills && hasRequiredScore;
        });
    }

    public IObservable<Skill.SkillState> GetSkillState(Skill skill)
    {
        var skills = skillStorage.GetSkills();

        return skills[skill.Id].Requirements
            .Select(id => skills[id])
            .Select(x => x.State)
            .CombineLatest()
            .CombineLatest(skill.State, gameStats.Score, (requirementsState, state, score) =>
            {
                if (state == Skill.SkillState.learned)
                {
                    return Skill.SkillState.learned;
                }
                else if (requirementsState.Contains(Skill.SkillState.learned) && score >= skill.Cost)
                {
                    return Skill.SkillState.available;
                }
                
                return Skill.SkillState.locked;
            });
    }

    public void ChangeScore(int change)
    {
        gameStats.Score.Value += change;
    }
}