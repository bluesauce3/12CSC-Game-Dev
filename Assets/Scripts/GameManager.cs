using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    public Button playButton;
    public TextMeshProUGUI gameTitle;
    public GameObject nameInputField;
    public TextMeshProUGUI nameErrorText;

    public bool isGameActive = false;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}

    public void StartGame() 
    {
        if (TestNameIsValid(nameInputField.GetComponent<TMP_InputField>().text))
        { 
            isGameActive = true;
            gameTitle.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            nameInputField.SetActive(false);
            nameErrorText.gameObject.SetActive(false);
        } else {
            nameErrorText.gameObject.SetActive(true);
        }
    }
    public bool TestNameIsValid(string testPlayerName)
    {
        return (testPlayerName.Length >= 2 && testPlayerName.Length <= 10 
        && Regex.IsMatch(testPlayerName, "[a-zA-Z]"));
    }
}
