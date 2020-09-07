using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}
