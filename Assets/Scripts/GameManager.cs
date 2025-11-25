using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameView gameView;
    public HumanPlayer player;
    public AIPlayer ai;

    private DeckManager deckManager;



    private void Awake()
    {
        player = new HumanPlayer();
        ai = new AIPlayer();
        deckManager = new DeckManager();


        StartRound();
    }

    public void StartRound()
    {
        // 1. deal cards
        player.GetCards(deckManager);
        ai.GetCards(deckManager);

        // give cards to player and ai
        // 2. show human hand on UI
        gameView.SpawnHand(handCards: player.myHand, onCardClicked: OnCardClicked);

        // show ai hand on UI
        gameView.SpawnHand(handCards: ai.myHand,isPlayer:false);
    }

    private void OnCardClicked(Card clickedCard)
    {
        // Add to the player expression
        player.myExpression.Add(clickedCard);
        if (clickedCard.type == CardType.Number)
            Debug.Log($"card: {clickedCard.numberValue}");
        else if (clickedCard.type == CardType.Operator)
            Debug.Log($"card: {clickedCard.operatorValue}");

        int currentResult = player.CalculateCurrentExpression(player.myExpression);
        gameView.DisplayResult(textContainer: gameView.playerResultText, result: currentResult);
        gameView.DisplayPlayerExpression(clickedCard);
    }

    private void Start()
    {
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
