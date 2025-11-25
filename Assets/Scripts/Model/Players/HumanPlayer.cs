using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer:PlayerBase
{
    // To compare current expression
    public List<Card> sampleExpression =
        new List<Card> {
            new Card(number: 0),
            new Card(oprtr: OperatorEnum.Plus),
            new Card(number: 0),
            new Card(oprtr: OperatorEnum.Minus),
            new Card(number: 0),
            };


    // pick cards from button clickable cards
    public void PickCards()
    {

    }

    // get sum of expression until current cards clicked on expression
    public int CalculateCurrentExpression(List<Card> myExpression)
    {
        int result = 0;

        // get current cards from myExpression
        // sequence: num + ops + num + ops + num
        // first place should num. next should be ops

        // do this later: validation 
        // UI: do not display the error card
        // logic: if the card sequence has an error, pop it either number or operator cards.

        // Validation: if the card is in the wrong place,
        //if card.type is CardType.Operator or card.type is CardTyep.Number
        //{
        //    Debug.LogWarning("Pick Number Card!");
        //    // pop the operator card 
        //    myExpression.Remove(card);
        //    break;
        //}                       

        /*
         * ignore the validation for now
         * 
         * 1. iterate the myExpression loop
         * 2. in 1st loop, if result is zero, and it's number value, store it into result. Continue
         *  no, result could be zero at any calculation.
         *  do it by preNum
         *  2.1 if result is non-zero, which means it's not the first card.
         *  2.2. if it's operator value
         * 3. in 2nd loop, if it's operator value, store it into preOpCard. Continue
         * 4. in 3rd loop, if it's number value, 
         *  check if result and preOpCard are not null.
         *  if it's true, check what the preOpCard is: plus, minus, etc.
         *  if it's plus, result += card
         *  if it's minus, result -=  card
         *  make preOpCard null. 
         */
        Card preNumCard = null;
        Card preOpCard = null;

        foreach (Card card in myExpression)
        {
            // at the last digit, it goes here, returning wrong answer
            if (card.type == CardType.Number && preNumCard is null)
            {
                result += card.numberValue;
                preNumCard = card;
            }
            else if(card.type == CardType.Operator)
            {
                preOpCard = card;
            }

            if (card.type is CardType.Number && preOpCard is not null)
            {
                if (preOpCard.operatorValue == OperatorEnum.Plus)
                {
                    result += card.numberValue;
                }
                else if (preOpCard.operatorValue == OperatorEnum.Minus)
                {
                    result -= card.numberValue;
                }
                preNumCard = null; 
                preOpCard = null;
            }
        }

        Debug.Log($"Result: {result}");
        return result;
    }
}
