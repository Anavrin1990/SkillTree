using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField] private int id;

    public int Id => id;
}
