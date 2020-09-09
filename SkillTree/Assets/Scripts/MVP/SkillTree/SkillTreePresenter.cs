using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class SkillTreePresenter : MonoBehaviour
{
    private SkillTreeView skillTreeView;
    private SkillManager skillManager;

    private ReactiveProperty<Skill> selectedSkill = new ReactiveProperty<Skill>();
    
    [Inject] void Init
    (
        SkillTreeView skillTreeView,
        SkillManager skillManager
    )
    {
        this.skillTreeView = skillTreeView;
        this.skillManager = skillManager;
    }

    private void Start()
    {
        var skillButtonOnClick = skillTreeView.SkillButtons
            .Select(x => x.onClick)
            .Merge()
            .Share();
        
        skillButtonOnClick
            .Subscribe(skill => selectedSkill.Value = skill)
            .AddTo(this);

        skillButtonOnClick
            .Subscribe(skill =>
            {
                foreach (var skillButton in skillTreeView.SkillButtons)
                {
                    var isSelected = skill.Id == skillButton.Skill.Id;
                    skillButton.SetSelected(isSelected);
                }
            })
            .AddTo(this);

        skillButtonOnClick
            .Select(x => x.Cost.ToString())
            .Subscribe(cost => skillTreeView.CostText.text = $"Цена {cost}")
            .AddTo(this);
        
        skillManager.CanForgetSkill(selectedSkill)
            .Subscribe(canLearn => skillTreeView.ForgetButton.interactable = canLearn)
            .AddTo(this);
        
        skillManager.CanLearnSkill(selectedSkill)
            .Subscribe(canLearn => skillTreeView.LearnButton.interactable = canLearn)
            .AddTo(this);

        foreach (var skillButton in skillTreeView.SkillButtons)
        {
            skillManager.GetSkillState(skillButton.Skill)
                .Subscribe(state => skillButton.SetState(state))
                .AddTo(this);
        }

        var learnButtonOnClick = skillTreeView.LearnButton
            .OnClickAsObservable();
        
        learnButtonOnClick
            .Select(_ => Skill.SkillState.learned)
            .Subscribe(state => selectedSkill.Value.State.Value = state)
            .AddTo(this);

        learnButtonOnClick
            .WithLatestFrom(selectedSkill, (_, skill) => -skill.Cost)
            .Subscribe(cost => skillManager.ChangeScore(cost))
            .AddTo(this);
        
        var forgetButtonOnClick = skillTreeView.ForgetButton
            .OnClickAsObservable();
        
        forgetButtonOnClick
            .Select(_ => Skill.SkillState.locked)
            .Subscribe(state => selectedSkill.Value.State.Value = state)
            .AddTo(this);
    }

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}