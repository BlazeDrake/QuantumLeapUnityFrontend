using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
    public Button loadButton;
    public TextMeshProUGUI errorText;

    private HttpController httpController;

    private void Start()
    {
        StartCoroutine(SetupRoutine());
    }
    public void SetPort(string input)
    {
        if (httpController.SetURI(input))
        {

            errorText.text = "Connecting...";
            httpController.Connect();
        }

    }

    public void Connect()
    {
        loadButton.gameObject.SetActive(true);
        errorText.text = "";
    }

    public void FailConnect(string error)
    {
        loadButton.gameObject.SetActive(false);
        errorText.text = "Failed to connect to server: " + error;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private IEnumerator SetupRoutine()
    {
        SceneManager.LoadSceneAsync("Server",LoadSceneMode.Additive);

        yield return new WaitUntil(() => SceneManager.GetSceneByName("Server").isLoaded);
        httpController= FindObjectOfType<HttpController>();

        DontDestroyOnLoad(httpController.gameObject);
        httpController.OnConnect.AddListener(Connect);
        httpController.OnFail.AddListener(FailConnect);
    }
}
