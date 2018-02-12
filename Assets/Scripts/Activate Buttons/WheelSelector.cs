using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WheelSelector : MonoBehaviour
{

	public string[] sceneNames;
	public UpdateBasedOnWheel wheel;
	[SerializeField] private Animator animator;
	[SerializeField] private Image black;

	private void LoadScene ()
	{
		string sceneName = sceneNames [wheel.currentWheelOption];
		SceneManager.LoadScene (sceneName);
	}

	IEnumerator Fading(){
		animator.SetBool("Fade", true);
		yield return new WaitUntil(()=>black.color.a==1);
		LoadScene();
	}

	public void Activate(){
		StartCoroutine(Fading());
	}
}
