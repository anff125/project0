using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace Card
{
    [CreateAssetMenu(fileName = "CardTemplate", menuName = "Card/CardTemplate")]
    public abstract class CardTemplate : ScriptableObject
    {
        protected int CardID;
        protected string CardName;
        [FormerlySerializedAs("Card Cost")] public int cardCost;
        [FormerlySerializedAs("Card Image")] public GameObject cardImage;

        protected enum ECardType
        {
            Weapon,
            Spell
        }

        protected ECardType CardType;
        
        protected CardTemplate(int cardID, string cardName, string cardDescription, int cardCost, GameObject cardImage)
        {
            CardID = cardID;
            CardName = cardName;
            this.cardCost = cardCost;
            this.cardImage = cardImage;
        }

        public virtual void UseCard()
        {
            //Debug.Log(CardName + " is used");
        }
    }
}