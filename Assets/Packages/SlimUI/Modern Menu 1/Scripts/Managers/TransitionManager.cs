using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlimUI.ModernMenu{
	public class TransitionManager : MonoBehaviour {
		private Animator CameraObject;

		public enum Theme {custom1, custom2, custom3};
		public Theme theme;
		private int themeIndex;
		public ThemedUIData themeController;
		
		public AudioSource hoverSound;

		void Start(){
			CameraObject = transform.GetComponent<Animator>();

			SetThemeColors();
		}

		void SetThemeColors()
		{
			themeController.currentColor = themeController.custom1.graphic1;
			themeController.textColor = themeController.custom1.text1;
			themeIndex = 0;
		}

		public void PlayHover(){
			hoverSound.Play();
		}

		public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}
}