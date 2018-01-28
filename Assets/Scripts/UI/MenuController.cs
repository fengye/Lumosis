using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public static MenuController instance;

    public void Awake(){
        if (instance != null) Destroy(instance);
        instance = this;
    }

    public enum MenuState {
        MAIN_MENU,
        GAME_OVER,
        CREDITS,
        PAUSE
    }

    public RectTransform mainMenu;
    public RectTransform gameOver;
    public RectTransform pause;
    public RectTransform credits;

    CanvasGroup canvasGroup;

	bool menuActive = true;
	public bool isMenuActive { get {return menuActive;}}

	MenuState menuState = MenuState.MAIN_MENU;
	public MenuState currentMenuState { get {return menuState;}}

    Coroutine visibilityCoroutine;

    void Start(){
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        UpdateMenuState();
        if (!menuActive)
        {
            HideMenu();
        }
    }

    public void ShowMainMenu(){
        ShowMenu(MenuState.MAIN_MENU);
    }

    public void ShowCreditsMenu(){
        ShowMenu(MenuState.CREDITS);
    }

    public void ShowPauseMenu(){
        ShowMenu(MenuState.PAUSE);
    }

	public void ShowMenu(MenuState newMenuState){
		menuState = newMenuState;
		UpdateMenuState();

        if (!menuActive)
        {
            if (visibilityCoroutine != null) StopCoroutine(visibilityCoroutine);
            visibilityCoroutine = StartCoroutine(_ShowMenu());
        }
    }

    public void HideMenu(){
        if (visibilityCoroutine != null) StopCoroutine(visibilityCoroutine);
        visibilityCoroutine = StartCoroutine(_HideMenu());
    }

    private IEnumerator _ShowMenu(){
		float velocity = 0f;
		menuActive = true;
        while (canvasGroup.alpha <= 0.9999)
        {
            canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, 1, ref velocity, 0.3f);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator _HideMenu(){
        menuActive = false;
        float velocity = 0f;
        while (canvasGroup.alpha >= 0.0001)
        {
            canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, 0, ref velocity, 0.3f);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    private void UpdateMenuState(){
        mainMenu.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);

        switch (menuState)
        {
            case MenuState.MAIN_MENU:
                mainMenu.gameObject.SetActive(true);
                break;

            case MenuState.GAME_OVER:
                gameOver.gameObject.SetActive(true);
                break;

            case MenuState.PAUSE:
                pause.gameObject.SetActive(true);
                break;

            case MenuState.CREDITS:
                credits.gameObject.SetActive(true);
                break;
        }
    }
}
