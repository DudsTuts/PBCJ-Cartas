using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Sprite cardFront;            // Carta
    public Sprite cardBack;             // Verso da carta

    public void OnMouseDown()   //Passa para o CardManager a carta selecionada
    {
        CardManager.instance.CardSelection(gameObject);
    }
    public void ShowCard()      //Mostra a carta escolhida
    {
        GetComponent<SpriteRenderer>().sprite = cardFront;
    }

    public void HideCard()      //Esconde a carta escolhida
    {
        GetComponent<SpriteRenderer>().sprite = cardBack;
    }

    public void SetCard(int i,int j, int mode)      //Define as cartas com base nos argumentos recebidos
    {
        string cardName = "";
        string cardNumber;

        if (i == 0)         //Determina o numero das cartas
        {
            cardNumber = "Ace";
        }
        else if (i == 10)
        {
            cardNumber = "Jack";
        }
        else if (i == 11)
        {
            cardNumber = "Queen";
        }
        else if (i == 12)
        {
            cardNumber = "King";
        }
        else
        {
            cardNumber = "" + (i+1);
        }

        if (mode == 2)      //Determina o naipe das cartas com base no modo de jogo
        {
            if (j == 0)
            {
                cardName = cardNumber + "_of_clubs";
            }
            else if (j == 1)
            {
                cardName = cardNumber + "_of_spades";
            }
        }
        else
        {
            if (j == 0)
            {
                cardName = cardNumber + "_of_clubs";
            }
            else if (j == 1)
            {
                cardName = cardNumber + "_of_hearts";
            } 
            else if (j == 2)
            {
                cardName = cardNumber + "_of_spades";
            }
            else
            {
                cardName = cardNumber + "_of_diamonds";
            }
        }

        Sprite s1 = (Sprite)(Resources.Load<Sprite>(cardName));     //Carrega a sprite das cartas
        cardFront = s1;     //Determina a sprite como face da carta
    }
}
