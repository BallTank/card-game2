using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer:PlayerBase
{
    // pick cards from button clickable cards
    public void PickCards()
    {

    }

    // get sum of expression until current cards clicked on expression
    public int CalculateCurrentExpression(List<Card> myExpression)
    {
        int result = 0;

        Card preNumCard = null;
        Card preOpCard = null;

        foreach (Card card in myExpression)
        {
            if (card.type == CardType.Operator)
            {
                preOpCard = card;
            }else if(card.type == CardType.Number)
            {
                if(preOpCard is null)
                {
                    result += card.numberValue;
                    preNumCard = card;
                } else if(preOpCard is not null)
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
        return result;
    }
}
