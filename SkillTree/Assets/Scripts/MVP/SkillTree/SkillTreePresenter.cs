using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreePresenter : MonoBehaviour
{
    [SerializeField] private SkillTreeView skillTreeView;

    private void Start()
    {
        
    }

    public void ToggleSetActive()
    {
        transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }
}
