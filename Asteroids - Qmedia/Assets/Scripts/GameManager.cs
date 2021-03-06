﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //quantidade de asteroides na cena
    public int qtdAsteroides;
    //contagem level
    public int level = 1;
    //linkar asteroid
    public GameObject asteroid;
    //linkar alien
    public AlienControlador alien;

    public void UpdateQtdAsteroides(int change)
    {
        //aumenta a qnt asteroides
        qtdAsteroides += change;

        if(qtdAsteroides <= 0)
        {
            Invoke("StartNewLevel", 2f);
        }
    }
    void StartNewLevel()
    {
        //acrescenta 1 nivel 
        level++;
        //criar novos asteroides
        for(int i = 0; i < level*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-8.5f, 8.5f), 11f);
            Instantiate(asteroid, spawnPosition, Quaternion.identity);
            qtdAsteroides++;
        }
        //linkar o alien
        alien.NewLevel();

    }
    //verifica a maior pontuacao
    public bool CheckforHighScores(int score)
    {
        int atualHighScore = PlayerPrefs.GetInt("highScore");
        if(score > atualHighScore)
        {
            return true;
        }
        return false;
    }
}
