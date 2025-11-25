using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameView gameView;
    public HumanPlayer player;
    public AIPlayer ai;

    private DeckManager deckManager;
    private int playerResult;
    private int aiResult;

    private void Awake()
    {
        player = new HumanPlayer();
        ai = new AIPlayer();
        deckManager = new DeckManager();


    }

    private void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        // 1. deal cards
        player.GetCards(deckManager);
        ai.GetCards(deckManager);

        // 2. show on UI
        UpdateGameUI();
    }

    private void OnCardClicked(Card clickedCard)
    {
        // Add to the player expression
        player.myExpression.Add(clickedCard);

        int currentResult = player.CalculateCurrentExpression(player.myExpression);
        gameView.DisplayResult(result: currentResult, isPlayer: true);
        gameView.DisplaySingleCard(card: clickedCard); // this part give unclickable card
    }

    // Calculate button method
    public string OnCalculatePressed(HumanPlayer player, AIPlayer ai)
    {
        int playerResult = player.CalculateExpression();
        int aiResult = ai.CalculateExpression();

        //ai.MakeMove();
        //int aiResult = ai.CalculateExpression();       

        // compare
        string msg = $"Player: {playerResult} vs AI: {aiResult}\n";
        if (playerResult > aiResult) msg += "Player Win!";
        else if (playerResult < aiResult) msg += "AI Win!";
        else { msg += "Draw!"; }

        return msg;
    }

    private void UpdateGameUI()
    {
        gameView.SpawnHand(handCards: player.myHand, isPlayer: true, onCardClicked: OnCardClicked);
        gameView.SpawnHand(handCards: ai.myHand, isPlayer: false);

        playerResult = player.ReturnResult();
        aiResult = ai.ReturnResult();

        gameView.DisplayExpression(player.myExpression, isPlayer: true);
        gameView.DisplayExpression(ai.myExpression, isPlayer: false);

        gameView.DisplayResult(result: playerResult, isPlayer: true);
        gameView.DisplayResult(result: aiResult, isPlayer: false);
    }

    public void ResetExpression()
    {
        player.ClearExpression();
    }

    private void OnEnable()
    {
        // should I make a class that handle a card clicked or not?    
    }
    private void OnDisable()
    {
        // if a method subscribed, unsubscribe it
    }
    private void Update()
    {      
        // player makes an expression.
        // how to get the player's expression?
    }
}
