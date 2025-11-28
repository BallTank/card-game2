using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HumanPlayer:PlayerBase
{
    // pick cards from button clickable cards
    public void PickCards()
    {

    }

    /*
     * get sum of expression until current cards clicked on expression
     * step
     * 1. make a deep copy list of the current expression
     * 2. find if there're multiplication or division
     * 3. calculate them
     * 4. remove those index at the deep copy list
     * 5. calculate addition, and subtraction
     * 6. return result
     * 
     * this only works when num opr num format.
     */
    public (float, bool) CalculateCurrentExpression(List<Card> myExpression, float playerOldResult)
    {
        if (myExpression.Count == 0) return (0, true);

        // validation check
        for (int i = 0; i < myExpression.Count; i++)
        {
            if (!CheckInputValidation(i))
            {
                Debug.LogWarning("Wrong Card!");
                myExpression.RemoveAt(i);
                return (0, false);
            }            
        }
 

        float result = 0;
        Card preNumCard = null;
        Card preOpCard = null;

        int multiIdx = int.MinValue;
        int divIdx = int.MinValue;

        // 1.make a deep copy list of the current expression
        List<Card> copyExpression = myExpression.ConvertAll((card) => new Card(card));

        // 2. find if there're multiplication or division
        // find the index of multiplication, division
        for (int i = 0; i < copyExpression.Count; i++)
        {
            if (copyExpression[i].type == CardType.Operator)
            {
                if (copyExpression[i].operatorValue == OperatorEnum.Multiply)
                {
                    multiIdx = i;
                }
                else if (copyExpression[i].operatorValue == OperatorEnum.Divide)
                {
                    divIdx = i;
                }
            }
        }

        // 3.calculate them
        // if mutiIdx + 1 is existing
        if (multiIdx > 0 && multiIdx + 1 <= copyExpression.Count -1)
        {
            int multiResult = copyExpression[multiIdx - 1].numberValue * copyExpression[multiIdx + 1].numberValue;

            // if it has plus or minus
            if (multiIdx - 2 > 0)
            {
                if (copyExpression[multiIdx - 2].operatorValue == OperatorEnum.Minus)
                {
                    // apply minus
                    multiResult = -multiResult;
                }
            }
            result += multiResult;
        }
        if (divIdx > 0 && divIdx + 1 <= copyExpression.Count - 1)
        {
            float divResult = copyExpression[divIdx - 1].numberValue / copyExpression[divIdx + 1].numberValue;

            // if it has plus or minus
            if (divIdx - 2 > 0)
            {
                if (copyExpression[divIdx - 2].operatorValue == OperatorEnum.Minus)
                {
                    // apply minus
                    divResult = -divResult;
                }
            }
            result += divResult;
        }

        Debug.Log($"After */ result: {result}");

        // 4.remove those index at the deep copy list
        // remove those multiplication and division things
        int originListCount = copyExpression.Count;
        int removedListCount = int.MinValue;
        // 2 * 3 or - 2 * 3 
        if (multiIdx > 0 && multiIdx + 1 <= copyExpression.Count - 1)
        {
            //  (2 * 3 -) or (- 2 * 3)
            // - 2 * 3
            // 8 * 5 occuring error    
            if (copyExpression[multiIdx-2].type == CardType.Operator)
                {
                    copyExpression.RemoveRange(multiIdx - 2, 4);
                }
            //}
            // 2 * 3
            else
            {
                copyExpression.RemoveRange(multiIdx - 1, 3); // remove 4 / 3 -> count: 4
            }
        }
        // move divIdx as removed idx
        removedListCount = myExpression.Count - copyExpression.Count;
        divIdx = divIdx - removedListCount;



        if (divIdx > 0 && divIdx + 1 <= copyExpression.Count - 1)
        {
            //  (2 / 3 -) or (- 2 / 3)
            if (copyExpression.Count >= 4)
            {
                // 2 / 3 -
                if (copyExpression[divIdx + 2].type == CardType.Operator)
                {
                    copyExpression.RemoveRange(divIdx - 1, 3);
                }
                // - 2 / 3
                else if (copyExpression[divIdx - 2].type == CardType.Operator)
                {
                    copyExpression.RemoveRange(divIdx - 2, 4);
                }
            }
            // 2 / 3
            else
            {
                copyExpression.RemoveRange(divIdx - 1, 3); // remove 4 / 3 -> count: 4
            }
        }

        // 5. calculate addition, and subtraction
        for (int i = 0; i < copyExpression.Count; i += 2)
        {
            // number
            if ( copyExpression[i].type == CardType.Operator)
            {
                // - 1, opr num
                if(i + 1 < copyExpression.Count && 
                    copyExpression[i].type == CardType.Operator)
                {
                    if (copyExpression[i].operatorValue == OperatorEnum.Minus)
                    {
                        result -= copyExpression[i+1].numberValue;
                    }
                    else if (copyExpression[i].operatorValue == OperatorEnum.Plus)
                    {
                        result += copyExpression[i+1].numberValue;
                    }
                }
            }
           // (9), the first number
            else if (copyExpression[i].type == CardType.Number)
            {
                result += copyExpression[i].numberValue;
            }
                //}
                //else
                //{   // first number
                //    result += copyExpression[i].numberValue;
                //}
        }
        return (result, true);

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
