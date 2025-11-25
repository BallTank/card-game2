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
        restartBtn.transform.localScale = new Vector2(0, 0);        
    }

    private void Start()
    {
        player = gameManager.player;
        ai = gameManager.ai;

        playerResult = player.ReturnResult();
        aiResult = ai.ReturnResult();

        DisplayExpression(player.myExpression, playerExpressionContainer);
        DisplayExpression(ai.myExpression, aIExpressionContainer);
        DisplayResult(textContainer: playerResultText, result: playerResult);
        DisplayResult(textContainer: aiResultText, result: aiResult);

        AddOnClickCalculator();
        AddOnClickRestart();
        AddOnClickResetExpression();
    }

    // get the player's hand
    public void DisplayPlayerExpression(Card clickedCard)
    {
        // PlayerBase class has myExpression list. 
        // To use a list to display, either destroy all the gameobj,
        // or search from where not on display should be done
        // So, just make the objects when the cards are clicked.
        GameObject obj = Instantiate(cardPrefab, playerExpressionContainer);
        CardDisplay display = obj.GetComponent<CardDisplay>();
        display.Setup(card: clickedCard, isExpression: true);

        // calculate cards and show the result on the player score
    }

    public void DisplayExpression(List<Card> expression, Transform container)
    {
        foreach(Card card in expression)
        {
            GameObject obj = Instantiate(cardPrefab, container);
            CardDisplay display = obj.GetComponent<CardDisplay>();
            display.Setup(card: card, isExpression: true);
        }
    }

    public void SpawnHand(List<Card> handCards, System.Action<Card> onCardClicked = null, bool isPlayer = true)
    {
        // it destroies player's cards
        //foreach(Transform child in playerHandContainer)
        //{
        //    Destroy(child.gameObject);
        //}

        foreach(Card card in handCards)
        {
            GameObject obj = Instantiate(cardPrefab, isPlayer ? playerHandContainer : aIHandContainer);
            CardDisplay display = obj.GetComponent<CardDisplay>();
            display.Setup(card: card, onClick: onCardClicked, isPlayer);
        }
    }

    private void AddOnClickCalculator()
    {
        calculatorBtn.onClick.RemoveAllListeners();
        calculatorBtn.onClick.AddListener(() => OnCalculatePressed(player, ai));        
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

    // Calculate button method
    public void OnCalculatePressed(HumanPlayer player, AIPlayer ai)
    {
        int playerResult = player.CalculateExpression();

        //ai.MakeMove();
        //int aiResult = ai.CalculateExpression();       

        // compare
        string msg = $"Player: {playerResult} vs AI: {aiResult}\n";
        if (playerResult > aiResult) msg += "Player Win!";
        else if (playerResult < aiResult) msg += "AI Win!";
        else { msg += "Draw!"; }

        UpdateResult(msg);

        restartBtn.transform.localScale = new Vector2(1, 1);
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnResetPressed()
    {
        // Question: how to get playerExpressionContainer's children gameobjects? It has Card objects
        // I know the playerExpressionContainer[0] is itself. So it should start from [1]
        // Answer: do this
        foreach (Transform child in playerExpressionContainer)
        {
            Destroy(child.gameObject);
        }
        
        player.ClearExpression();

        playerResult = 0;
        DisplayResult(textContainer:playerResultText, result: playerResult);
    }

    public void DisplayResult(TMP_Text textContainer, int result)
    {
        textContainer.text = result.ToString();
    }

    public void UpdateResult(string text)
    {
        resultText.text = text;
    }
}
