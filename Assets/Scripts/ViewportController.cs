using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewportController : MonoBehaviour
{
    [System.Serializable]
    public class CardData
    {
        public string name;
        public Sprite image;
    }

    [SerializeField]
    private ViewportDAOServer dao;

    [SerializeField]
    private Image viewportDisplay;

    [SerializeField]
    private List<CardData> cardInitializeList;

    private Dictionary<string, Sprite> cards;
    // Start is called before the first frame update
    void Start()
    {
        if (cards == null)
        {
            cards = new Dictionary<string, Sprite>();
        }
        foreach (CardData card in cardInitializeList)
        {
            if (!cards.ContainsKey(card.name))
            {
                cards.Add(card.name.ToLower(), card.image);
            }
        }
        StartCoroutine(SetServerCards());
    }

    private void Update()
    {
        SetCard(dao.GetCurrentImage());
    }

    public void SetCard(string name)
    {
        name = name.ToLower();
        if (cards.ContainsKey(name))
        {
            viewportDisplay.sprite = cards[name];
        }
        else
        {
            Debug.LogWarning("Card with name " + name + " not found in the card list.");
        }
    }

    private IEnumerator SetServerCards()
    {
        yield return new WaitUntil(()=> dao != null && dao.IsReady);
        string[] cardNames = new string[cards.Count];
        cards.Keys.CopyTo(cardNames, 0);
        dao.SetCards(cardNames);
    }
}
