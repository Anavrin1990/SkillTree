using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeView : MonoBehaviour
{
    [SerializeField] private List<Button> skills;
    
    [SerializeField] private Button learnButton;
    [SerializeField] private Button forgetButton;
    [SerializeField] private Button forgetAllButton;
}
