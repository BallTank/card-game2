using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase
{
    // this has the common things of the player and ai    
    // calculate the expressions
    // ai picks its own cards

    public List<Card> myHand = new List<Card>();

    // how to get the expression?
    public List<Card> myExpression = new List<Card>();

    // I want to give them 3 random operator cards and 5 diff number cards
    public virtual void GetCards(DeckManager deckManager)
    {
        // provide 2 operator cards and random 5 number cards
        myHand.Clear();
        myHand.AddRange(deckManager.GetNumberDeck(count: 5));
        myHand.AddRange(deckManager.GetOperatorDeck());
    }

    // As calulate button pressed
    // this method calculates both expressions of ai and player    
    // TODO: Multiply and divide are not consiered
    public int CalculateExpression()
    {
        if (myExpression.Count == 0) return 0;

        int result = 0;
        Card preOprtCard = null;

        for (int i = 0; i < myExpression.Count; i++)
        {
            if (i % 2 == 0) // number
            {
                if (myExpression[i].type == CardType.Operator)  // validation check
                {
                    break;
                }

                if (preOprtCard is null)
                { // first number                

                    result += myExpression[i].numberValue;
                }
                else
                {
                    if (preOprtCard.operatorValue == OperatorEnum.Plus)
                    {
                        result += myExpression[i].numberValue;
                    }
                    else if (preOprtCard.operatorValue == OperatorEnum.Minus)
                    {
                        result -= myExpression[i].numberValue;
                    }
                    preOprtCard = null;
                }
            }
            else // operator
            {
                if (myExpression[i].type == CardType.Number)  // validation check
                {
                    break;
                }
                preOprtCard = myExpression[i];
            }
        }
        return result;
    }

    // Set my hand's best expresison
    // TODO: Validation
    //public bool SetOptimalExpression()
    public void SetOptimalExpression()
    {
        myExpression.Clear();

        List<Card> nums = new List<Card>();
        List<Card> ops = new List<Card>();

        for (int i = 0; i < myHand.Count; i++)
        {
            if (myHand[i].type == CardType.Number)
            {
                nums.Add(myHand[i]);
                //if (CheckInputValidation(i))
                //{                    
                //    return false;   // it's not valid!
                //}
            }
            else if (myHand[i].type == CardType.Operator) ops.Add(myHand[i]);
        }

        nums.Sort((a, b) => a.numberValue.CompareTo(b.numberValue));

        Card highestNum1Card = nums[nums.Count - 1];
        Card highestNum2Card = nums[nums.Count - 2];
        Card lowestNumCard = nums[0];
        Card plusCard = null;
        Card minusCard = null;

        foreach (Card card in ops)
        {
            if (card.operatorValue == OperatorEnum.Plus)
            {
                plusCard = card;
            }
            else if (card.operatorValue == OperatorEnum.Minus)
            {
                minusCard = card;
            }
        }

        if (plusCard is not null && minusCard is not null)
        {
            myExpression.Add(highestNum1Card);
            myExpression.Add(plusCard);
            myExpression.Add(highestNum2Card);
            myExpression.Add(minusCard);
            myExpression.Add(lowestNumCard);
        }
        //return true;
    }

    // check if the myExpression is in the right place: number and operator
    public bool CheckInputValidation(int expIdx)
    {
        bool result = false;
        if (expIdx % 2 == 0)
        {
            return myExpression[expIdx].type == CardType.Number ? true : false;
        }
        if (expIdx % 2 != 0)
        {
            return myExpression[expIdx].type == CardType.Operator ? true : false;
        }
        return result;
    }

    public int ReturnResult()
    {
        int result = 0;
        SetOptimalExpression();
        result = CalculateExpression();

        return result;
    }

    public void ClearExpression()
    {
        myExpression.Clear();
    }
}
