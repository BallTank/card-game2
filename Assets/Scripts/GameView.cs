using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    // Deisplay on UI

    [Header("UIHeader")]
    public Transform playerHandContainer;
    public Transform playerExpressionContainer;
    public Transform aIHandContainer;
    public Transform aIExpressionContainer;
    public GameObject cardPrefab;

    [Header("Buttons")]
    public Button calculatorBtn;
    public Button restartBtn;
    public Button resetBtn;

    public TMP_Text resultText;
    public TMP_Text playerResultText;
    public TMP_Text aiResultText;

    private HumanPlayer player;
    private AIPlayer ai;
    private int playerResult;
    private int aiResult;
    public GameManager gameManager;
    private void Awake()
    {
        // why i can't use rectTransform?
        restartBtn.gameObject.SetActive(false);
    }

    private void Start()
    {
        player = gameManager.player;
        ai = gameManager.ai;

        AddOnClickCalculator();
        AddOnClickRestart();
        AddOnClickResetExpression();
    }


    public void DisplayExpression(List<Card> expression, bool isPlayer)
    {
        Transform container = isPlayer ? playerExpressionContainer : aIExpressionContainer;

        DisplayCard(expression, container);
    }

    public void SpawnHand(List<Card> handCards, bool isPlayer, 
        System.Action<Card> onCardClicked = null)
    {
        Transform container = isPlayer ? playerHandContainer : aIHandContainer;
        bool isExpression = false;

        DisplayCard(handCards: handCards, container: container, 
            onCardClicked: onCardClicked, isPlayer: isPlayer, isExpression: isExpression);
    }

    public void DisplayCard(List<Card> handCards, Transform container, 
        System.Action<Card> onCardClicked = null, bool isPlayer = false, bool isExpression = false)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in handCards)
        {
            DisplaySingleCard(card: card, onClick: onCardClicked,
                container: container, isPlayer: isPlayer, isExpression: isExpression);
        }
    }

    public void DisplaySingleCard(Card card, System.Action<Card> onClick = null, 
        Transform container = null, bool isPlayer=false, bool isExpression=false)
    {
        if (container == null)
        {
            container = playerExpressionContainer;
        }

        GameObject obj = Instantiate(cardPrefab, container);
        CardDisplay display = obj.GetComponent<CardDisplay>();
        display.Setup(card: card, onClick: onClick, 
            isPlayer: isPlayer, isExpression: isExpression);
    }

    private void AddOnClickCalculator()
    {
        string msg = "";

        calculatorBtn.onClick.RemoveAllListeners();
        calculatorBtn.onClick.AddListener(() =>
        {
            msg = gameManager.OnCalculatePressed(player, ai);
            UpdateResult(msg);
            restartBtn.gameObject.SetActive(true);
        });
    }

    private void AddOnClickRestart()
    {
        restartBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.AddListener(OnRestartPressed);
    }

    private void AddOnClickResetExpression()
    {
        resetBtn.onClick.RemoveAllListeners();
        resetBtn.onClick.AddListener(OnResetPressed);
    }

 

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnResetPressed()
    {
        foreach (Transform child in playerExpressionContainer)
        {
            Destroy(child.gameObject);
        }

        gameManager.ResetExpression();

        DisplayResult(result: 0, isPlayer: true);
    }

    public void DisplayResult(float result, bool isPlayer)
    {
        TMP_Text textContainer = isPlayer ? playerResultText : aiResultText;
        textContainer.text = "";
        textContainer.text = result.ToString();
    }

    public void UpdateResult(string text)
    {
        resultText.text = text;
    }
}
