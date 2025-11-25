using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay: MonoBehaviour
{
    public TMP_Text lable;
    private Button button;
    public Card cardData;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // question syntax, is it like an event?
    // if it is, I only saw Action not Action<T>
    private System.Action<Card> onClickCallback; 

    public void Setup(Card card, System.Action<Card> onClick = null, bool isPlayer = true, bool isExpression = false)
    {
        cardData = card;
        onClickCallback = onClick;

        if(card.type == CardType.Number)
        {
            lable.text = card.numberValue.ToString();
        }else if(card.type == CardType.Operator)
        {
            lable.text = card.operatorValue.ToString();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickCallback(card));

        if(!isPlayer || isExpression)
        {
            button.interactable = false;
        }
    }
}
