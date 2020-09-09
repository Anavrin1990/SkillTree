using System;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillButton : MonoBehaviour
{
    public IObservable<Skill> onClick;
    public Skill Skill;
    
    [SerializeField] private Skill.SkillId skillId;
    
    private Button button;

    [Inject] void Init(SkillStorage skillStorage)
    {
        Skill = skillStorage.GetSkills()[skillId];
    }
    
    private void Start()
    {
        button = GetComponent<Button>();
        
        GetComponentInChildren<Text>().text = Skill.Title;

        onClick = button
            .OnClickAsObservable()
            .Select(_ => Skill);
    }

    public void SetSelected(bool isSelected)
    {
        transform.GetChild(0).gameObject.SetActive(isSelected);
    }
    
    public void SetState(Skill.SkillState skillState)
    {
        var image = button.GetComponent<Image>();
        
        switch (skillState)
        {
            case Skill.SkillState.learned:
                image.color = Color.white;
                break;
            case Skill.SkillState.available:
                image.color = Color.grey;
                break;
            case Skill.SkillState.locked:
                image.color = Color.red;
                break;
        }
    }
}
