using System;
using System.Linq;
using ModestTree;
using UniRx;
using UniRx.Diagnostics;

public class SkillManager
{
    private readonly GameStats _gameStats;
    private readonly SkillStorage _skillStorage;
    
    public SkillManager
    (
        SkillStorage skillStorage,
        GameStats gameStats
    )
    {
        _gameStats = gameStats;
        _skillStorage = skillStorage;
    }

    public IObservable<bool> CanLearnSkill(IObservable<Skill> skillStream)
    {
        return skillStream
            .Where(x => x != null)
            .CombineLatest(_gameStats.Score, (skill, score) =>
            {
                if (skill.State.Value == Skill.SkillState.learned) return false;

                var hasRequiredSkills = skill.Requirements
                    .Select(id => _skillStorage.GetSkills()[id].State.Value)
                    .Contains(Skill.SkillState.learned);

                var hasRequiredScore = score >= skill.Cost;

                return hasRequiredSkills && hasRequiredScore;
            });
    }

    public IObservable<bool> CanForgetSkill(IObservable<Skill> skillStream)
    {
        var skillStorage = _skillStorage.GetSkills();

        var skillChanged = skillStream
            .Where(x => x != null)
            .SelectMany(skill => skill.State);
        
        return skillStream.CombineLatest(skillChanged, (skill, _) => skill)
            .Where(x => x != null)
            .Select(skill => (skill, skill.DependentSkills.Select(id => skillStorage[id])))
            .Select(skills =>
            {
                var (currentSkill, dependentSkills) = skills;

                if (currentSkill.State.Value != Skill.SkillState.learned
                    || currentSkill.Id == Skill.SkillId.main) return false;

                foreach (var dependentSkill in dependentSkills)
                {
                    if (dependentSkill.State.Value != Skill.SkillState.learned) continue;

                    var requirements = dependentSkill.Requirements
                        .Where(id => id != currentSkill.Id)
                        .Select(id => skillStorage[id]);

                    if (requirements.IsEmpty())
                        return false;

                    return requirements
                        .Select(skill => skill.State.Value == Skill.SkillState.learned)
                        .Contains(true);
                }

                return true;
            });
    }
    
    public IObservable<Skill.SkillState> GetSkillState(Skill skill)
    {
        var skillStorage = _skillStorage.GetSkills();
        
        return skillStorage[skill.Id].Requirements
            .Select(id => skillStorage[id].State)
            .CombineLatest()
            .CombineLatest(skill.State, _gameStats.Score, (requirementsState, state, score) =>
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

    public void ForgetAll()
    {
        var skills = _skillStorage.GetSkills().Values
            .Where(skill => 
                skill.State.Value == Skill.SkillState.learned && skill.Id != Skill.SkillId.main);
        
        foreach (var skill in skills)
        {
            skill.State.Value = Skill.SkillState.locked;
            ChangeScore(skill.Cost);
        }
    }
    
    public void ChangeScore(int change)
    {
        _gameStats.Score.Value += change;
    }
}