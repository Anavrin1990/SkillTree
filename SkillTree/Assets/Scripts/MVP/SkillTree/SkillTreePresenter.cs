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
        // gameStats.LearnedSkills
        //     .ObserveEveryValueChanged(x => x.Value)
        //     .Where(skills => skills.Length > 0)
        //     .Subscribe(skills =>
        //     {
        //         var skillButtons = skillTreeView.SkillButtons;
        //         
        //         for (var i = 1; i < skillButtons.Length; i++)
        //         {
        //             skillButtons[i].interactable = gameStats.LearnedSkills.Value[i] != null;
        //         }
        //     })
        //     .AddTo(this);
    }

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}