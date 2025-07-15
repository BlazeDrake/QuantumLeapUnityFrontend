using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class ViewportController : MonoBehaviour
{
    [System.Serializable]
    public class CardData
    {
        public string name;
        public Sprite image;
    }
    [System.Serializable]
    public class UrlData
    {
        public string name;
        public string url;
    }

    [System.Serializable]
    public class VideoCardData
    {
        public string name;
        public string filepath;
    }

    [SerializeField]
    private ViewportDAOServer dao;

    [SerializeField]
    private Image viewportDisplay;
    [SerializeField]
    private RawImage videoDisplay;
    [SerializeField]
    private VideoPlayer videoPlayer;


    [SerializeField]
    [FormerlySerializedAs("cardInitializeList")]
    private List<CardData> downloadedImageCardData;

    [SerializeField]
    private List<UrlData> urlImageCardData;

    [SerializeField]
    [FormerlySerializedAs("videoCards")]
    private List<VideoCardData> videoCardData;

    private Dictionary<string, Sprite> imageCards;
    private Dictionary<string, string> videoCards;


    private string curCard;
    private bool initialized = false;   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeController());
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }
        SetCard(dao.GetCurrentImage());
    }

    public void SetCard(string name)
    {
        name = name.ToLower();
        if (name == curCard)
        {
            return;
        }

        if (imageCards.ContainsKey(name))
        {
            viewportDisplay.sprite = imageCards[name];
            videoPlayer.Stop();
            videoDisplay.gameObject.SetActive(false);
            curCard = name;
        }
        else if (videoCards.ContainsKey(name))
        {
            videoPlayer.url = videoCards[name];
            videoPlayer.Play();
            videoDisplay.gameObject.SetActive(true);
            curCard = name;
        }
        else
        {
            Debug.LogWarning("Card with name " + name + " not found in the card list.");
        }
    }

    private IEnumerator InitializeController()
    {
        yield return StartCoroutine(SetupCards());
        yield return StartCoroutine(SetServerCards());
        initialized = true;
    }

    private IEnumerator SetupCards()
    {
        if (imageCards == null)
        {
            imageCards = new Dictionary<string, Sprite>();
        }
        if (videoCards == null)
        {
            videoCards = new Dictionary<string, string>();
        }
        //load image cards
        foreach (CardData card in downloadedImageCardData)
        {
            if (!imageCards.ContainsKey(card.name))
            {
                imageCards.Add(card.name.ToLower(), card.image);
            }
        }
        foreach(var card in urlImageCardData)
        {
            if (!imageCards.ContainsKey(card.name.ToLower()))
            {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(card.url);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    var cardSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    imageCards.Add(card.name.ToLower(), cardSprite);
                }
            }
        }

        //load video cards
        foreach(var card in videoCardData)
        {
            if (!videoCards.ContainsKey(card.name.ToLower()))
            {
                string filepath = System.IO.Path.Combine(Application.persistentDataPath,card.filepath);
                if(System.IO.File.Exists(filepath))
                {
                    videoCards.Add(card.name.ToLower(), "file://"+filepath);
                }
                else
                {
                    Debug.LogWarning($"Video file {filepath} does not exist for card {card.name}");
                }
            }
        }
    }

    private IEnumerator SetServerCards()
    {
        
        yield return new WaitUntil(()=> dao != null && dao.IsReady);
        string[] cardNames = new string[imageCards.Count+videoCards.Count];
        imageCards.Keys.CopyTo(cardNames, 0);
        videoCards.Keys.CopyTo(cardNames, imageCards.Count);
        dao.SetCards(cardNames);
    }
}
