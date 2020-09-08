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
        var selectedSkill = skillTreeView.SkillButtons
            .Select(x => x.onClick)
            .Merge()
            .Share();
        
        skillManager.CanLearnSkill(selectedSkill)
            .Subscribe(canLearn => skillTreeView.LearnButton.interactable = canLearn)
            .AddTo(this);
        
        
    }

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}