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
        skillTreeView.SkillButtons
            .Select(x => x.onClick)
            .Merge()
            .Subscribe(skill => selectedSkill.Value = skill)
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
        
        skillTreeView.LearnButton
            .OnClickAsObservable()
            .Select(_ => Skill.SkillState.learned)
            .Subscribe(state => selectedSkill.Value.State.Value = state)
            .AddTo(this);
    }

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}