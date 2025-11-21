using System.Collections.Generic;

public abstract class PlayerBase
{
    // this has the common things of the player and ai

    // provide 2 operator cards and random 5 number cards
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
        myHand.AddRange(deckManager.GetNumberDeck(5));
        myHand.AddRange(deckManager.GetOperatorDeck());
    }


    // Calculate each expression: player and AI
    // picking the cards are their own classes' job
    public int CalculateExpression()
    {
        if (myExpression.Count == 0) return 0;

        // Assumes format: Num, Opr, Num, Opr...
        int result = myExpression[0].numberValue;

        // TODO: validation for each places should be added later
        for(int i = 1; i < myExpression.Count; i += 2)
        {
            if (i + 1 >= myExpression.Count) break; // prevent out of range

            OperatorEnum op = myExpression[i].operatorValue;
            int nextNum = myExpression[i + 1].numberValue;

            if (op == OperatorEnum.Plus) result += nextNum;
            else if (op == OperatorEnum.Minus) result -= nextNum;
        }
        return result;
    }
}
