using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public GameObject card;                 //Game Object para instanciar a carta
    public static int gameMode;         //Decide modo de jogo (2 ou 4 fileiras) 
    private List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };     //Lista com as posicoes das cartas
    public static CardManager instance;     //Singleton para uso externo
    public bool firstCardState = false;     //***
    public bool secondCardState = false;    //Estados que determinam se uma carta
    public bool thirdCardState = false;     //foi escolhida ou nao
    public bool fourthCardState = false;    //***
    public GameObject firstCard;            //***
    public GameObject secondCard;           //Game Objects que armazenam
    public GameObject thirdCard;            //as cartas escolhidas
    public GameObject fourthCard;           //***
    public string firstCardName;            //***
    public string secondCardName;           //Strings que armazenam o nome
    public string thirdCardName;            //das cartas escolhidas
    public string fourthCardName;           //***
    private float timer;                    //Relogio para controle de tempo
    private bool timerUp = false;           //Estado ativo/desativo do relogio
    private int numTry = 0;                 // Numero de tentativas realizadas
    private int numHit = 0;                 // Numero de acertos
    AudioSource hitAudio;                   // Variavel para utilizar som de acerto


    private void Awake()        //Evita bugs do singleton
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateNumTry();
        AddCards();
        hitAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerUp)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                timerUp = false;    // Pausa timer
                if (gameMode == 2)
                {
                    if (CardCompare(firstCardName, secondCardName))
                    {
                        Destroy(firstCard);     // Retira cartas do campo
                        Destroy(secondCard);    // Retira cartas do campo
                        numHit++;               // Incrementa numero de acertos
                        hitAudio.Play();        // Toca audio de confirmacao de acerto
                        if (numHit >= 13)       // Se todas as cartas forem combinadas termina o jogo
                        {
                            PlayerPrefs.SetInt("score", numTry);    // Armazena pontuacao do usuario no PlayerPrefs
                            SceneManager.LoadScene("EndScene");     // Muda a cena
                        }
                    }
                    else
                    {
                        firstCard.GetComponent<Tile>().HideCard();      //Esconde as cartas selecionadas
                        secondCard.GetComponent<Tile>().HideCard();     //
                    }
                    firstCardState = false;     //***
                    secondCardState = false;    //
                    firstCard = null;           //  Reset do estado inicial das variaveis
                    secondCard = null;          //
                    firstCardName = "";         //
                    secondCardName = "";        //***
                    timer = 0;
                }
                else
                {
                    if (CardCompare(firstCardName, secondCardName, thirdCardName, fourthCardName))
                    {
                        Destroy(firstCard);     //***
                        Destroy(secondCard);    // Restira cartas do campo
                        Destroy(thirdCard);     //
                        Destroy(fourthCard);    //***
                        numHit++;
                        hitAudio.Play();
                        if (numHit >= 13)
                        {
                            PlayerPrefs.SetInt("score", numTry);
                            SceneManager.LoadScene("EndScene");
                        }
                    }
                    else
                    {
                        firstCard.GetComponent<Tile>().HideCard();      //***
                        secondCard.GetComponent<Tile>().HideCard();     //Esconde as cartas selecionadas
                        thirdCard.GetComponent<Tile>().HideCard();      //
                        fourthCard.GetComponent<Tile>().HideCard();     //***
                    }
                    firstCardState = false;     //***
                    secondCardState = false;    //
                    thirdCardState = false;     //
                    fourthCardState = false;    //
                    firstCard = null;           //
                    secondCard = null;          //  Reset do estado inicial das variaveis
                    thirdCard = null;           //    
                    fourthCard = null;          //
                    firstCardName = "";         //
                    secondCardName = "";        //
                    thirdCardName = "";         //
                    fourthCardName = "";        //
                    timer = 0;                  //***
                }
                numTry++;
                UpdateNumTry();
            }
        }
    }

    public void AddCards()                      //Inicializa as cartas na mesa
    {
        List<int> rnd = numbers;
        if (gameMode == 2)      // Modo de jogo com 2 fileiras de cartas
        {
            for (int j = 0; j < 2; j++)
            {
                Shuffle(rnd);
                for (int i = 0; i < 13; i++)
                {
                    GameObject c = (GameObject)(Instantiate(card, new Vector3(((i - 6) * 1.3f), (j * 2f - 1), 0), Quaternion.identity));
                    c.name = (rnd[i]) + "_" + (j);
                    c.GetComponent<Tile>().SetCard(rnd[i],j, gameMode);
                }
            }
        }
        else                    // Modo de jogo com 4 fileiras de cartas
        {
            for (int j = 0; j < 4; j++)
            {
                Shuffle(rnd);
                for (int i = 0; i < 13; i++)
                {
                    GameObject c = (GameObject)(Instantiate(card, new Vector3(((i - 6) * 1.3f), (j * 2f - 4), 0), Quaternion.identity));
                    c.name = (rnd[i]) + "_" + (j);
                    c.GetComponent<Tile>().SetCard(rnd[i], j, gameMode);
                }
            }
        }
    }

    public void CardSelection(GameObject card)      //Age na selecao de cartas e aciona o temporizador quando condicoes forem concluidas
    {
        
        if (gameMode == 2)
        {
            if (!firstCardState)
            {
                firstCardState = true;
                firstCard = card;
                firstCardName = card.name;
                card.GetComponent<Tile>().ShowCard();
            }
            else if (firstCardState && !secondCardState && card.name.Split('_')[1] != firstCardName.Split('_')[1])
            {
                
                    secondCardState = true;
                    secondCard = card;
                    secondCardName = card.name;
                    card.GetComponent<Tile>().ShowCard();
                    Timer();
                
            }
        }
        else
        {
            if (!firstCardState)
            {
                firstCardState = true;
                firstCard = card;
                firstCardName = card.name;
                card.GetComponent<Tile>().ShowCard();
            }
            else if (firstCardState && !secondCardState && card.name.Split('_')[1] != firstCardName.Split('_')[1])
            {
                secondCardState = true;
                secondCard = card;
                secondCardName = card.name;
                card.GetComponent<Tile>().ShowCard();
            }
            else if (firstCardState && secondCardState && !thirdCardState && card.name.Split('_')[1] != firstCardName.Split('_')[1] && card.name.Split('_')[1] != secondCardName.Split('_')[1])
            {
                thirdCardState = true;
                thirdCard = card;
                thirdCardName = card.name;
                card.GetComponent<Tile>().ShowCard();
            }
            else if (firstCardState && secondCardState && thirdCardState && !fourthCardState && card.name.Split('_')[1] != firstCardName.Split('_')[1] && card.name.Split('_')[1] != secondCardName.Split('_')[1] && card.name.Split('_')[1] != thirdCardName.Split('_')[1])
            {
                fourthCardState = true;
                fourthCard = card;
                fourthCardName = card.name;
                card.GetComponent<Tile>().ShowCard();
                Timer();
            }
        }
    }

    public void Timer()     //Aciona o temporizador
    {
        timerUp = true;
    }

    public bool CardCompare(string s1, string s2)       //Compara o inicion de duas strings
    {
        if (s1.StartsWith(s2.Split('_')[0])){
            return true;
        }
        return false;
    }

    public bool CardCompare(string s1, string s2,string s3, string s4)          //Compara o inicio de quatro strings
    {
        if (s1.StartsWith(s2.Split('_')[0]) && s1.StartsWith(s3.Split('_')[0]) && s1.StartsWith(s4.Split('_')[0]))
        {
            return true;
        }
        return false;
    }

    public void UpdateNumTry()              //Atualiza texto do numero de tentativas
    {
        GameObject.Find("NumTry").GetComponent<Text>().text = "Tentativas: " + numTry;
    }

    private List<int> Shuffle(List<int> alpha)          //Embaralha uma lista de inteiros
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            int temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
        return alpha;
    }
}
