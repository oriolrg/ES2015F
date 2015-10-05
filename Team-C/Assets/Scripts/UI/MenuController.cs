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
        quitMenu.enabled = false; //Quick menu disabled

    }

    public void ExitPress() //This function will be used on our Exit button

    {
        quitMenu.enabled = true; //enable the Quit menu when we click the Exit button

        //Disable the Play and Exit buttons so they cannot be clicked
        startText.enabled = false; 
        exitText.enabled = false;

    }

    public void NoPress() //This function will be used for our "NO" button in our Quit Menu

    {
        quitMenu.enabled = false; //Disable the quit menu, meaning it won't be visible anymore

        //Enable the Play and Exit buttons again 
        startText.enabled = true; 
        exitText.enabled = true;

    }

    public void StartLevel() //this function will be used on our Play button

    {
        quitMenu.enabled = false;
		gameObject.SetActive (false);

        switch( menuType )
        {
            case MenuType.MainMenu:
                Application.LoadLevel(1); //this will load our first level 
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
                Application.Quit(); //this will quit our game
                break;

            case MenuType.PauseMenu:
                Application.LoadLevel(0);//this will return to our main menu
                break;
        }

    }
}
