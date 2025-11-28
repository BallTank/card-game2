using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class PlayerBase
{
    // this has the common things of the player and ai    
    // calculate the expressions
    // ai picks its own cards

   public int cardCount = 5;
    public List<Card> myHand = new List<Card>();

    // how to get the expression?
    public List<Card> myExpression = new List<Card>();

    // I want to give them 3 random operator cards and 5 diff number cards
    public virtual void GetCards(DeckManager deckManager)
    {
        // provide 2 operator cards and random 5 number cards
        myHand.Clear();
        myHand.AddRange(deckManager.GetNumberDeck(count: cardCount));
        myHand.AddRange(deckManager.GetOperatorDeck());
    }

    /*
     * As calulate button pressed
     * this method calculates both expressions of ai and player
     * 
     * The first algorithm is linear. 
     * As mulplication, division added, 
     * the expression should search for them first.      
     */
    public float CalculateExpression()
    {
        if (myExpression.Count == 0) return 0;

        List<Card> copyExpression = myExpression.ConvertAll((card) => new Card(card));

        float result = 0;

        int multiIdx = int.MinValue;
        int divIdx = int.MinValue;

        // find the index of multiplication, division
        for (int i = 0; i < copyExpression.Count; i++)
        {
            if (copyExpression[i].type == CardType.Operator)
            {
                if(copyExpression[i].operatorValue == OperatorEnum.Multiply)
                {
                    multiIdx = i;
                } else if (copyExpression[i].operatorValue == OperatorEnum.Divide)
                {
                    divIdx = i;
                }
            }
        }

        if (multiIdx > 0)
        {
            int multiResult = copyExpression[multiIdx - 1].numberValue * copyExpression[multiIdx + 1].numberValue;

            // if it has plus or minus
            if (multiIdx-2 > 0)
            {
                if(copyExpression[multiIdx - 2].operatorValue == OperatorEnum.Minus)
                {
                    // apply minus
                    multiResult = -multiResult;
                }
                //copyExpression.RemoveAt(multiIdx - 2);
            }
            //copyExpression.RemoveRange(multiIdx - 1, 3);
            result += multiResult;
        }
        if (divIdx > 0)
        {
            float divResult = copyExpression[divIdx - 1].numberValue / copyExpression[divIdx + 1].numberValue;

            // if it has plus or minus
            if (divIdx - 2 > 0)
            {
                if (copyExpression[divIdx - 2].operatorValue == OperatorEnum.Minus)
                {
                    // apply minus
                    divResult = -divResult;

                    //copyExpression.RemoveAt(divIdx - 2);
                }
            }
            //copyExpression.RemoveRange(divIdx - 1, 3);
            result += divResult;
        }

        // remove those multiplication and division things
        int originListCount = copyExpression.Count;
        int removedListCount = int.MinValue;
        if (multiIdx > 0)
        {
            if (multiIdx - 2 > 0)
            {
                copyExpression.RemoveAt(multiIdx - 2);
            }
            copyExpression.RemoveRange(multiIdx - 1, 3);
        }

        // move divIdx as removed idx
        removedListCount = myExpression.Count - copyExpression.Count;
        divIdx = divIdx - removedListCount;

        if (divIdx > 0)
        {
            // TODO: fix here: remove - 4 / 3 -> count: 4 how to deal with it?
            copyExpression.RemoveRange(divIdx - 2, 4); // remove - 4 / 3 -> count: 4
        }


        for (int i = 0; i < copyExpression.Count; i += 2)
        {
            // number
            if(i % 2 == 0)
            {
                //operator
                if (i - 1 > 0) 
                {
                    if (copyExpression[i-1].operatorValue == OperatorEnum.Minus)
                    {
                        result -= copyExpression[i].numberValue;
                    }
                    else
                    {
                        result += copyExpression[i].numberValue;
                    }
                }
            }
        }

        return result;
    }

    /* Set my hand's best expresison
     * 
     * Logic     
     * Multiply 1st highest and 2nd highest
     * Plus the 3rd highest
     * Subtract from the 1st smallest
     * Divide by the 2nd smallest
     * 
     * Warning
     * No divide by 0
     * Multiply and Divide should go first (this also counts to player expresison)
     * 9 8 7 6 5
     * 9 * 8 + 7 / 6 - 5 = 68.16
     * 9 * 8 + 7 / 5 - 6 = 67.4
     * 9 * 8 / 5 + 7 - 6 = 15.4
     * 9 * 8 + 6 - 5 / 7 = 77.28
     * 9 * 8 + 7 - 5 / 6 = 78.16 (best)
     * 9 * 8 - 5 / 6 + 7 (same above)
     * 7 + 9 * 8 - 5 / 6  (same above)
    */
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

        Card no1highestNumCard = nums[nums.Count - 1];
        Card no2highestNumCard = nums[nums.Count - 2];
        Card no3highestNumCard = nums[nums.Count - 3];
        Card no1LowestNumCard = nums[0];
        // check zero for division
        Card no2LowestNumCard = nums[1].numberValue != 0 ? nums[1] : nums[2];
        Card plusCard = null;
        Card minusCard = null;
        Card multiplyCard = null;
        Card divideCard = null;

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
            else if (card.operatorValue == OperatorEnum.Multiply)
            {
                multiplyCard = card;
            }
            else if (card.operatorValue == OperatorEnum.Divide)
            {
                divideCard = card;
            }
        }

        if (plusCard is not null && minusCard is not null
            && multiplyCard is not null && divideCard is not null)
        {
            myExpression.Add(no1highestNumCard);
            myExpression.Add(multiplyCard);
            myExpression.Add(no2highestNumCard);
            myExpression.Add(plusCard);
            myExpression.Add(no3highestNumCard);
            myExpression.Add(minusCard);
            myExpression.Add(no1LowestNumCard);
            myExpression.Add(divideCard);
            myExpression.Add(no2LowestNumCard);
        }
        //return true;
    }

    // check if the myExpression is in the right place: number and operator
    public bool CheckInputValidation(int expIdx)
    {
        bool result = false;

        //Debug.Log($"CheckOrder(expIdx): {CheckOrder(expIdx)} " +
        //    $"CheckDuplication(expIdx): {CheckDuplication(expIdx)} " +
        //    $"CheckDividedByZero(expIdx): {CheckDividedByZero(expIdx)} ");

        if (CheckOrder(expIdx) && CheckDuplication(expIdx) && CheckDividedByZero(expIdx))
        {
            result = true;
        }
        //if (CheckDuplication(expIdx))
        //{
        //    result = true;
        //}

        return result;
    }

    public bool CheckOrder(int expIdx)
    {
        // if it's Num Opr Num Opr Num pattern, return true
        bool result = false;

        if (expIdx % 2 == 0)
        {
            result = myExpression[expIdx].type == CardType.Number ? true : false;
        }
        else if (expIdx % 2 != 0)
        {
            result = myExpression[expIdx].type == CardType.Operator ? true : false;
        }

        return result;
    }

    public bool CheckDuplication(int expIdx)
    {
        // if it's not duplicate, then return true
        bool result = false;

        result = myExpression.Contains(myExpression[expIdx]);

        return result;
    }

    public bool CheckDividedByZero(int expIdx)
    {
        bool result = true;

        // if it's not zero, return true
        if(myExpression[expIdx].type == CardType.Operator && 
            myExpression[expIdx].operatorValue == OperatorEnum.Divide &&
            expIdx + 1 > myExpression.Count && // to cover the range below
            myExpression[expIdx + 1].numberValue == 0)
        {
           result = false;
        }
        
        return result;
    }

    public float ReturnResult()
    {
        float result = 0;
        SetOptimalExpression();
        result = CalculateExpression();

        return result;
    }

    public void ClearExpression()
    {
        myExpression.Clear();
    }
}
