using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    AudioSource record;     // Variavel para tocar musica quando bater o record

    void Start()
    {
        record = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            UpdateText(PlayerPrefs.GetInt("score"));
        }
    }

    public void Retry()     // Funcao para mudanca de cena
    {
        SceneManager.LoadScene("OpenScene");
    }

    public void Finish()    // Funcao para mudanca de cena 
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void Easy()      // Funcao para mudanca de cena com variavel para selecionar modo de jogo facil
    {
        CardManager.gameMode = 2;
        SceneManager.LoadScene("SampleScene");
    }
    public void Hard()      // Funcao para mudanca de cena com variavel para selecionar modo de jogo dificil
    {
        CardManager.gameMode = 4;
        SceneManager.LoadScene("SampleScene");
    }

    private void UpdateText(int score)      // Funcao para atualizar texto pos-jogo e atualizacao do record caso necessario
    {
        if(score < PlayerPrefs.GetInt("recordEasy",99999) && CardManager.gameMode == 2)
        {
            PlayerPrefs.SetInt("recordEasy", score);
            GameObject.Find("EndText").GetComponent<Text>().text = "Parabéns! Você bateu o record!";
            record.Play();
        }
        else if (score < PlayerPrefs.GetInt("recordHard",99999) && CardManager.gameMode == 4)
        {
            PlayerPrefs.SetInt("recordHard", score);
            GameObject.Find("EndText").GetComponent<Text>().text = "Parabéns! Você bateu o record!";
            record.Play();
        }
        else
        {
            GameObject.Find("EndText").GetComponent<Text>().text = "Parabéns! Você ganhou o jogo!";
        }
    }
}
