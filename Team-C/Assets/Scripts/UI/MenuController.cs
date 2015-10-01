using UnityEngine;
using UnityEngine.UI;

public enum MenuType { MainMenu, PauseMenu }

public class MenuController : MonoBehaviour {

    [SerializeField] private MenuType menuType;
    public Canvas quitMenu;
    public Button startText;
    public Button exitText;

    void Start()

    {
        quitMenu.enabled = false;

    }

    void Update()
    {
        if( menuType == MenuType.PauseMenu && Input.GetKeyDown(KeyCode.Escape) )
        {
            GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
        }
    }

    public void ExitPress() //this function will be used on our Exit button

    {
        quitMenu.enabled = true; //enable the Quit menu when we click the Exit button
        startText.enabled = false; //then disable the Play and Exit buttons so they cannot be clicked
        exitText.enabled = false;

    }

    public void NoPress() //this function will be used for our "NO" button in our Quit Menu

    {
        quitMenu.enabled = false; //we'll disable the quit menu, meaning it won't be visible anymore
        startText.enabled = true; //enable the Play and Exit buttons again so they can be clicked
        exitText.enabled = true;

    }

    public void StartLevel() //this function will be used on our Play button

    {
        quitMenu.enabled = false;
        GetComponent<Canvas>().enabled = false;
        switch( menuType )
        {
            case MenuType.MainMenu:
                Application.LoadLevel(1); //this will load our first level from our build settings
                break;

            case MenuType.PauseMenu:
                Time.timeScale = 1;
                break;
        }
        

    }

    public void ExitGame() //This function will be used on our "Yes" button in our Quit menu

    {
        switch( menuType )
        {
            case MenuType.MainMenu:
                Application.Quit(); //this will quit our game. Note this will only work after building the game
                break;
            case MenuType.PauseMenu:
                Application.LoadLevel(0);
                break;
        }

    }
}
