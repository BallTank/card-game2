using System.Collections.Generic;

public class AIPlayer : PlayerBase
{
    public void MakeMove()
    {
        myExpression.Clear();

        List<Card> nums = new List<Card>();
        List<Card> ops = new List<Card>();

        foreach(Card card in myHand)
        {
            if (card.type == CardType.Number) nums.Add(card);
            else if (card.type == CardType.Operator) ops.Add(card);
        }

        myExpression.Add(nums[0]);
        myExpression.Add(ops[0]);
        myExpression.Add(nums[1]);
        myExpression.Add(ops[1]);
        myExpression.Add(nums[2]);
    }

    // pick cards itself

    private void PickBestCards()
    {

    }
}
