using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    [CreateAssetMenu(fileName = "WeaponCard", menuName = "Card/WeaponCard")]
    public class CardType_Weapon : CardTemplate
    {
        public int Durability;
        public int AttackDamage;
        public int WeaponIndex;

        protected CardType_Weapon(int cardID, string cardName, string cardDescription, int cardCost,
            GameObject cardImage,
            int attackDamage, int durability)
            : base(cardID, cardName, cardDescription, cardCost, cardImage)
        {
            Durability = durability;
            AttackDamage = attackDamage;
            CardType = ECardType.Weapon;
        }

        public override void UseCard()
        {
            base.UseCard();
            WeaponManager.Instance.ActivateWeaponAtIndex(WeaponIndex);
        }
        
        
    }
}