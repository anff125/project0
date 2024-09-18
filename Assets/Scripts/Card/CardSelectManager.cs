using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Card
{
    public class CardSelectManager : MonoBehaviour
    {
        public static CardSelectManager Instance;
        public GameObject[] Cards;
        public GameObject LastSelectedCard;
        public int LastSelectedCardIndex;

        [FormerlySerializedAs("Card List")] public CardTemplate[] CardList;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            InputManager.PlayerInput.actions["CardNavigate"].performed +=
                ctx =>
                {
                    Debug.Log("CardNavigate action performed");
                    HandleNextCardSelection(ctx.ReadValue<Vector2>().x);
                };

            InputManager.PlayerInput.actions["CardUse"].started += ctx =>
            {
                Debug.Log("CardUse action performed");
                UseCard(ctx.ReadValue<float>());
            };
        }

        private void Start()
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                GameObject cardObject = Instantiate(CardList[i].cardImage, gameObject.transform);
                cardObject.transform.localPosition = new Vector3(-400 + i * 200, 0, 0);
                Cards[i] = cardObject;
            }
        }

        private void UseCard(float input)
        {
            Debug.Log("Use Card");
            if (LastSelectedCard != null)
            {
                CardTemplate cardTemplate = CardList[LastSelectedCardIndex];
                cardTemplate.UseCard();
            }
        }

        private void OnEnable()
        {
            StartCoroutine(SetSelectAfterOneframe());
        }

        private IEnumerator SetSelectAfterOneframe()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(Cards[0]);
        }

        private void HandleNextCardSelection(float input)
        {
            int addition = 0;
            if (input > 0)
            {
                addition = 1;
            }
            else if (input < 0)
            {
                addition = -1;
            }

            if (LastSelectedCard != null)
            {
                int newIndex = LastSelectedCardIndex + addition;
                newIndex = Mathf.Clamp(newIndex, 0, Cards.Length - 1);
                EventSystem.current.SetSelectedGameObject(Cards[newIndex]);
            }
        }
    }
}