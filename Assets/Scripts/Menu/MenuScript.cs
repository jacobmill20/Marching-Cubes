using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject buildingPoint;
    
    private GameObject activeMenu;

    // Start is called before the first frame update
    void Start()
    {
        activeMenu = mainMenu;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMenuActive(GameObject pane)
    {
        activeMenu.SetActive(false);
        pane.SetActive(true);
        activeMenu = pane;
    }

    public void ReturnToMenu()
    {
        activeMenu.SetActive(false);
        mainMenu.SetActive(true);
        activeMenu = mainMenu;
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
