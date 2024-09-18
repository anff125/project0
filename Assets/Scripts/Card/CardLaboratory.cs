using System.Collections;
using System.Collections.Generic;
using Card;
using UnityEngine;

public class CardLaboratory : MonoBehaviour
{
    public static CardLaboratory Instance;

    public CardTemplate[] cardlist;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}