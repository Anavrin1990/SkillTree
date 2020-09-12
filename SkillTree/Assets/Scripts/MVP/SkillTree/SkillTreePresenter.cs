using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class SkillTreePresenter : MonoBehaviour
{
    private SkillTreeView _skillTreeView;
    private SkillManager _skillManager;

    private readonly ReactiveProperty<Skill> _selectedSkill = new ReactiveProperty<Skill>();
    
    [Inject] void Init
    (
        SkillTreeView skillTreeView,
        SkillManager skillManager
    )
    {
        _skillTreeView = skillTreeView;
        _skillManager = skillManager;
    }

    private void Start()
    {
        SkillSelectHandle();
        UIUpdateHandle();
        LearnSkillHandle();
        ForgetSkillHandle();
    }

    private void SkillSelectHandle()
    {
        var skillButtonOnClick = _skillTreeView.SkillButtons
            .Select(x => x.OnClick)
            .Merge()
            .Share()
            .StartWith(_skillTreeView.SkillButtons.First().Skill);

        skillButtonOnClick
            .Subscribe(skill => _selectedSkill.Value = skill)
            .AddTo(this);

        skillButtonOnClick
            .Subscribe(skill =>
            {
                foreach (var skillButton in _skillTreeView.SkillButtons)
                {
                    var isSelected = skill.Id == skillButton.Skill.Id;
                    skillButton.SetSelected(isSelected);
                }
            })
            .AddTo(this);

        skillButtonOnClick
            .Select(x => x.Cost.ToString())
            .Subscribe(cost => _skillTreeView.CostText.text = $"Цена {cost}")
            .AddTo(this);
    }
    
    private void UIUpdateHandle()
    {
        _skillManager.CanForgetSkill(_selectedSkill)
            .Subscribe(canForget => _skillTreeView.ForgetButton.interactable = canForget)
            .AddTo(this);

        _skillManager.CanLearnSkill(_selectedSkill)
            .Subscribe(canLearn => _skillTreeView.LearnButton.interactable = canLearn)
            .AddTo(this);

        foreach (var skillButton in _skillTreeView.SkillButtons)
        {
            _skillManager.GetSkillState(skillButton.Skill)
                .Subscribe(state => skillButton.SetState(state))
                .AddTo(this);
        }
    }

    private void LearnSkillHandle()
    {
        var learnButtonOnClick = _skillTreeView.LearnButton
            .OnClickAsObservable();

        learnButtonOnClick
            .Select(_ => Skill.SkillState.learned)
            .Subscribe(state => _selectedSkill.Value.State.Value = state)
            .AddTo(this);

        learnButtonOnClick
            .WithLatestFrom(_selectedSkill, (_, skill) => -skill.Cost)
            .Subscribe(cost => _skillManager.ChangeScore(cost))
            .AddTo(this);
    }

    private void ForgetSkillHandle()
    {
        var forgetButtonOnClick = _skillTreeView.ForgetButton
            .OnClickAsObservable();

        forgetButtonOnClick
            .Select(_ => Skill.SkillState.locked)
            .Subscribe(state => _selectedSkill.Value.State.Value = state)
            .AddTo(this);

        forgetButtonOnClick
            .WithLatestFrom(_selectedSkill, (_, skill) => skill.Cost)
            .Subscribe(cost => _skillManager.ChangeScore(cost))
            .AddTo(this);
        
        _skillTreeView.ForgetAllButton
            .OnClickAsObservable()
            .Subscribe(_ => _skillManager.ForgetAll())
            .AddTo(this);
    }

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}