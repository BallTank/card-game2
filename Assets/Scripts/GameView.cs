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
    public Transform expressionContainer;
    public GameObject cardPrefab;

    public Transform aIHandContainer;

    [Header("Buttons")]
    public Button calculatorBtn;
    public Button restartBtn;

    public TMP_Text resultText;

    private HumanPlayer player;
    private AIPlayer ai;

    public GameManager gameManager;
    private void Awake()
    {
        // why i can't use rectTransform?
        restartBtn.transform.localScale = new Vector2(0, 0);



        AddOnClickCalculator();
        AddOnClickRestart();
    }

    private void Start()
    {
        player = gameManager.player;
        ai = gameManager.ai;
    }

    public void DisplayPlayerExpression()
    {
        // get the player's hand
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

    // Calculate button method
    public void OnCalculatePressed(HumanPlayer player, AIPlayer ai)
    {
        int playerResult = player.CalculateExpression();

        ai.MakeMove();
        int aiResult = ai.CalculateExpression();

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

    public void UpdateResult(string text)
    {
        resultText.text = text;
    }
}
