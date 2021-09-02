using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelector : MonoBehaviour
{
    [SerializeField] Abilty.AbilityType type = Abilty.AbilityType.Default;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameSession.HandleSkillSelection(type);
    }
}
