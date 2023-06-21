using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlimUI.ModernMenu{
	public class UIMenuManager : MonoBehaviour {
		private Animator CameraObject;

		// campaign button sub menu
        [Header("MENUS")]
        [Tooltip("The Menu for when the MAIN menu buttons")]
        public GameObject mainMenu;
        [Tooltip("THe first list of buttons")]
        public GameObject firstMenu;
        [Tooltip("The Menu for when the EXIT button is clicked")]
        public GameObject exitMenu;

        public enum Theme {custom1, custom2, custom3};
        [Header("THEME SETTINGS")]
        public Theme theme;
        private int themeIndex;
        public ThemedUIData themeController;

				public Text timerText;
        

		[Header("SFX")]
        [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
        public AudioSource hoverSound;
        [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
        public AudioSource swooshSound;

		void Start(){
			CameraObject = transform.GetComponent<Animator>();

			exitMenu.SetActive(false);
			firstMenu.SetActive(true);
			mainMenu.SetActive(true);

			SetThemeColors();
		}

		void Update()
		{
			float timer = LevelManager.timer;
			timerText.text = "Game duration: " + timer.ToString("f2");
		}

		void SetThemeColors()
		{
			themeController.currentColor = themeController.custom1.graphic1;
			themeController.textColor = themeController.custom1.text1;
			themeIndex = 0;
		}

		public void ReturnMenu(){
			exitMenu.SetActive(false);
			mainMenu.SetActive(true);
		}

		public void Position8(){
			CameraObject.SetFloat("Animate3",0);
		}

		public void Position7(){
			CameraObject.SetFloat("Animate3",1);
		}

		public void Position6(){
			CameraObject.SetFloat("Animate2",0);
		}

		public void Position5(){
			CameraObject.SetFloat("Animate2",1.5f);

			GameObject npc = GameObject.FindWithTag("WelcomeNPC");
			Animator npcAnimator = npc.GetComponent<Animator>();
			npcAnimator.SetInteger("animState", 1);
		}

		public void Position4(){
			CameraObject.SetFloat("Animate2",0.75f);
		}

		public void Position3(){
			CameraObject.SetFloat("Animate2",1);
		}

		public void Position2(){
			CameraObject.SetFloat("Animate",1);
		}

		public void Position1(){
			CameraObject.SetFloat("Animate",0);
		}

		public void PlayHover(){
			hoverSound.Play();
		}

		public void PlaySwoosh(){
			swooshSound.Play();
		}

		// Are You Sure - Quit Panel Pop Up
		public void AreYouSure(){
			exitMenu.SetActive(true);
		}

		public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}

		public void LoadFirstLevel()
		{
			FindObjectOfType<LevelManager>().LoadFirstLevel();
		}

		public void AnimateNPC(string selectedSchool) {
			FindObjectOfType<LevelManager>().selectedSchool = selectedSchool;
			GameObject npc = GameObject.FindWithTag(selectedSchool);
			Animator npcAnimator = npc.GetComponent<Animator>();
			npcAnimator.SetInteger("animState", 1);

			Invoke("Position5", 3);
		}
	}
}