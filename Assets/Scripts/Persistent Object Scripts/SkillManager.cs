using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    [SerializeField] List<Skill> healthSkills;
    [SerializeField] List<Skill> manaSkills;
    [SerializeField] List<Skill> switchSkills;
    [SerializeField] List<Skill> damageSkills;
    [SerializeField] List<Skill> coinSkills;
    [SerializeField] List<Skill> activesSkills;
    [SerializeField] List<Skill> bonusSkills;
    List<Skill> allSkills = new List<Skill>();

    List<Skill> currentSkills;

    public void setCurrentSkills()
    {
        currentSkills.Clear();

        allSkills.AddRange(healthSkills);
        allSkills.AddRange(manaSkills);
        allSkills.AddRange(switchSkills);
        allSkills.AddRange(damageSkills);
        allSkills.AddRange(coinSkills);
        allSkills.AddRange(activesSkills);
        allSkills.AddRange(bonusSkills);

        foreach (Skill skill in allSkills)
        {
            if (skill.Active)
            {
                currentSkills.Add(skill);
            }
        }
    }

    private int checkNumActiveSkills(List<Skill> skills)
    {
        int i = 0;
        foreach (Skill skill in skills)
        {
            if (skill.Active)
            {
                i++;
            }
        }
        return i;
    }

    public void setSkillActive(int skillType, int skillNumber, bool pActivating)
    {
        if (skillType == 6)
        {
            bonusSkills[skillNumber].Active = pActivating;
        }
        else if (checkNumActiveSkills(allSkills) - checkNumActiveSkills(bonusSkills) < 5)
        {
            switch (skillType)
            {
                case 0:
                    healthSkills[skillNumber].Active = pActivating;
                    break;
                case 1:
                    manaSkills[skillNumber].Active = pActivating;
                    break;
                case 2:
                    switchSkills[skillNumber].Active = pActivating;
                    break;
                case 3:
                    damageSkills[skillNumber].Active = pActivating;
                    break;
                case 4:
                    coinSkills[skillNumber].Active = pActivating;
                    break;
                case 5:
                    activesSkills[skillNumber].Active = pActivating;
                    break;
            }
        }
    }

    public void setBonusSkillsActive()
    {
        const int TYPE_BONUS_NUM = 3;
        if (checkNumActiveSkills(healthSkills) >= TYPE_BONUS_NUM)
        {
            bonusSkills[0].Active = true;
        } else if (checkNumActiveSkills(manaSkills) >= TYPE_BONUS_NUM)
        {
            bonusSkills[1].Active = true;
        }
        else if (checkNumActiveSkills(switchSkills) >= TYPE_BONUS_NUM)
        {
            bonusSkills[2].Active = true;
        }
        else if (checkNumActiveSkills(damageSkills) >= TYPE_BONUS_NUM)
        {
            bonusSkills[3].Active = true;
        }
        else if (checkNumActiveSkills(coinSkills) >= TYPE_BONUS_NUM)
        {
            bonusSkills[4].Active = true;
        }
        else if (checkNumActiveSkills(activesSkills) >= TYPE_BONUS_NUM)
        {
            bonusSkills[5].Active = true;
        }
    }
}
