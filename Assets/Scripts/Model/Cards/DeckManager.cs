using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// have cards for numbers, operator, special ones(later)

public enum OperatorEnum { Plus, Minus, Multiply, Divide }
public enum CardType { Number, Operator }

public class Card
{
    public CardType type;
    public int numberValue;             // in case the type is Number
    public OperatorEnum operatorValue;  // in case the type is Operator

    // Constructor for Number Cards
    public Card(int number)
    {
        type = CardType.Number;
        numberValue = number;
    }

    // Constructor for Operator Cards
    public Card(OperatorEnum oprtr)
    {
        type = CardType.Operator;
        operatorValue = oprtr;
    }
}

public class DeckManager
{
    public List<Card> numberDeck = new List<Card>();
    public List<Card> operatorDeck = new List<Card>();

    public int numberCopy = 1;

    public DeckManager()
    {
        InitializeNumberDeck();
        //SetNumDeck();
        //SetOperatorDeck();

    }
    private void InitializeNumberDeck()
    {
        for (int copy = 0; copy < numberCopy; copy++)
        {
            for (int num = 0; num < 10; num++)
            {
                // 0 to 9, 4 cards each numbers
                numberDeck.Add(new Card(num));
            }
        }
    }

    private void SetNumDeck() {
        for (int copy = 0; copy < numberCopy; copy++)
        {
            for (int num = 0; num < 10; num++)
            {
                // 0 to 9, 4 cards each numbers
                Card newCard = new Card(num);
                numberDeck.Add(newCard);
            }
        }
    }
    private void SetOperatorDeck() {
        // for now, only plus and minus operator cards are provided to player, ai
        //for (int copy = 0; copy < 2; copy++)
        //{
        //    operatorDeck.Add(new Card(OperatorEnum.Plus));
        //    operatorDeck.Add(new Card(OperatorEnum.Minus));
        //    operatorDeck.Add(new Card(OperatorEnum.Multiply));
        //    operatorDeck.Add(new Card(OperatorEnum.Divide));
        //}
    }

    public List<Card> GetNumberDeck(int count)
    {
        Shuffle(list: numberDeck);
        
        // take the top count cards
        List<Card> hand = new List<Card>();
        
        for(int i = 0; i < count; i++)
        {
            if(i < numberDeck.Count)
            {
                hand.Add(numberDeck[i]);
            }
            else // if out of cards in the ops
            {
                Debug.LogError("There's no more cards in the deck");
            }
        }

        return hand;
    }

    public List<Card> GetOperatorDeck()
    {
        List<Card> ops = new List<Card>();
        ops.Add(new Card(OperatorEnum.Plus));
        ops.Add(new Card(OperatorEnum.Minus));

        return ops;
    }

    private void Shuffle(List<Card> list)
    {
        for(int i = list.Count-1; i >= 0; i--)
        {
            int randIdx = UnityEngine.Random.Range(0, i+1);

            Card val = list[randIdx];
            list[randIdx] = list[i];
            list[i] = val;
        }        
    }
}
