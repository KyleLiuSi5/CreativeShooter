using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCoolDown : MonoBehaviour
{
    public Image icon;
    public float coolDown;

    public string skillName;

    public float currentCoolDown;

    private Button skillButton;

    public void UseSkill(string skillName)
    {
        if (currentCoolDown >= coolDown)
        {
            currentCoolDown = 0;
        }
    }

    void Start()
    {
        this.skillButton = this.GetComponent<Button>();
        skillButton.onClick.AddListener(() => this.UseSkill(skillName));
        currentCoolDown = coolDown;
    }

    void Update()
    {
        if (currentCoolDown < coolDown)
        {
            currentCoolDown += Time.deltaTime;
            this.icon.fillAmount = currentCoolDown / coolDown;
        }       
    }
}
