using UnityEngine;

namespace Card
{
    [CreateAssetMenu(fileName = "SpellCard", menuName = "Card/SpellCard")]
    public class CardType_Spell : CardTemplate
    {
        public int SpellIndex;
        public int SpellDamage;
        
        protected CardType_Spell(int cardID, string cardName, string cardDescription, int cardCost,
            GameObject cardImage)
            : base(cardID, cardName, cardDescription, cardCost, cardImage)
        {
        }

        public override void UseCard()
        {
            base.UseCard();
            SpellManager.Instance.UseSpell(SpellIndex);
        }
    }
}