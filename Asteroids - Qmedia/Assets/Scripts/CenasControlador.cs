using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenasControlador : MonoBehaviour
{
    //Funcionalidade nos botoes para navegar entre nos menus
    public void Jogar()
    {
        SceneManager.LoadScene("QmediaAsteroids");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Fechar()
    {
        Application.Quit();
    }
}
