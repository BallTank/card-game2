using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer:PlayerBase
{
    // pick cards from button clickable cards
    public void PickCards()
    {

    }

    // get sum of expression until current cards clicked on expression
    public (int, bool) CalculateCurrentExpression(List<Card> myExpression)
    {
        int result = 0;
        Card preNumCard = null;
        Card preOpCard = null;

        string msg = "";
        foreach(Card card in myExpression)
        {
            if(card.type == CardType.Number)
                msg += $"{card.numberValue} ";
            else
            {
                msg += $"{card.operatorValue} ";
            } 
        }
        Debug.Log(msg);

        for (int i = 0; i < myExpression.Count; i++)
        {
            if (!CheckInputValidation(i))
            {
                Debug.LogWarning("Wrong Card!");
                myExpression.RemoveAt(i);
                return (0, false);
            }

            Card card = myExpression[i];

            if (card.type == CardType.Operator)
            {
                preOpCard = card;
            }
            else if (card.type == CardType.Number)
            {
                if (preOpCard is null)
                {
                    result += card.numberValue;
                    preNumCard = card;
                }
                else if (preOpCard is not null)
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
        }

        return (result, true);
    }
}
