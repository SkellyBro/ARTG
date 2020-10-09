using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class handles the start screen navigation
public class StartScreenNav : MonoBehaviour {

    //scene declaration

    public string Registration; 
    public string Login; 

    public void OnRegister()
    {
        SceneManager.LoadScene(Registration);
    }

    public void OnLogin()
    {
        SceneManager.LoadScene(Login);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
