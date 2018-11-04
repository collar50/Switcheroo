using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSkillsPane : MonoBehaviour {

    [SerializeField] Animator uiAnimator;

    public void popUI()
    {
        uiAnimator.SetBool("isPopping", true);
        

        StartCoroutine("unPop");
    }

    private IEnumerator unPop()
    {
        yield return new WaitForSeconds(.5f);
        uiAnimator.SetBool("isPopping", false);
        uiAnimator.SetBool("isClosed", !uiAnimator.GetBool("isClosed"));
    }
}
