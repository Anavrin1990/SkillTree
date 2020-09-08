using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillButton : MonoBehaviour
{
    public IObservable<Skill> onClick;
    
    [SerializeField] private Skill.SkillId skillId;
    
    private Skill skill;

    [Inject] void Init(SkillStorage skillStorage)
    {
        skill = skillStorage.GetSkills()[skillId];
    }
    
    private void Start()
    {
        GetComponentInChildren<Text>().text = skill.Title;
        
        onClick = GetComponent<Button>()
            .OnClickAsObservable()
            .Select(_ => skill);
    }
}
