using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenasControlador : MonoBehaviour
{

    public void Jogar()
    {
        SceneManager.LoadScene("QmediaAsteroids");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
