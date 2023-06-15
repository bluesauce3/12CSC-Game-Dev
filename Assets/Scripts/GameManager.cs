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
    public Text scoreText;
    public float scoreTextSize;
    public GameObject hurtOverlayObject;
    private Image hurtOverlay;
    public GameObject EnemySpawnManager;
    public GameObject Map;
    public GameObject Tree;

    public GameObject[] PlayerHearts = new GameObject[3];

    public bool isGameActive = false;
    public string playerName;

    public PlayerController playerControllerScript;

    public string viewMode;

    private GameObject[] enemies;

    public float startTime;
    public float timeSinceStart = 0f;

    public int enemiesKilledScore;

    private Color hurtOverlayInitialColor;

    // Start is called before the first frame update
    void Start()
    {
        hurtOverlay = hurtOverlayObject.GetComponent<Image>();
        hurtOverlayInitialColor = hurtOverlay.color;
        SpawnTrees(Tree, 600);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive) {timeSinceStart = Time.time - startTime;}
        if (Input.GetKeyDown("1") && isGameActive) {
            viewMode = "1";
        }
        if (Input.GetKeyDown("2") && isGameActive) {
            viewMode = "2";
        }
        if (playerControllerScript.health == 0) {
            EndGame();
        }
        scoreText.text = "Score: " + (Mathf.Round(timeSinceStart) + enemiesKilledScore);
        
    }

    public void StartGame() 
    {
        if (TestNameIsValid(nameInputField.GetComponent<TMP_InputField>().text))
        { 
            isGameActive = true;
            Instantiate(EnemySpawnManager, transform.position, transform.rotation);
            gameTitle.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            nameInputField.SetActive(false);
            nameErrorText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            scoreText.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            scoreText.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
            scoreText.gameObject.GetComponent<Text>().alignment = TextAnchor.LowerRight;
            scoreText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-Screen.width/(2*scoreTextSize), Screen.height/(2*scoreTextSize), 0);
            scoreText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/scoreTextSize, Screen.height/scoreTextSize);
            Cursor.lockState = CursorLockMode.Locked;
            startTime = Time.time;
            enemiesKilledScore = 0;
            foreach (GameObject heart in PlayerHearts)
            {heart.SetActive(true);}
        } else {
            nameErrorText.gameObject.SetActive(true);
        }
    }
    public void EndGame(){
        playerControllerScript.health = 3;
        isGameActive = false;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy);
        }
        viewMode = "1";
        gameTitle.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        nameInputField.GetComponent<TMP_InputField>().text = "";
        nameInputField.SetActive(true);
        scoreText.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        scoreText.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        scoreText.gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        scoreText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -140, 0);
        Cursor.lockState = CursorLockMode.None;
    }
    public bool TestNameIsValid(string testPlayerName)
    {
        return (testPlayerName.Length >= 2 && testPlayerName.Length <= 10 
        && Regex.IsMatch(testPlayerName, "[a-zA-Z]"));
    }
    public void removeHeart(int playerHealth)
    {
        PlayerHearts[playerHealth].SetActive(false);
        StartCoroutine(hurtOverlayCountdown(1.5f));

    }
    public void SpawnTrees(GameObject tree, int numberOfTrees)
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            GameObject lastTree = Instantiate(tree, new Vector3(Random.Range(-Map.transform.localScale.x, Map.transform.localScale.x), 0, Random.Range(-Map.transform.localScale.z, Map.transform.localScale.z)),
            transform.rotation);
            float scaleFactor = Mathf.Pow(10, Random.Range(-1f, 1f));
            lastTree.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        }


    }

    public IEnumerator hurtOverlayCountdown(float fadeDuration)
    {
        hurtOverlay.color = Color.red;
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer/fadeDuration);
            Color currentColor = new Color(hurtOverlayInitialColor.r, hurtOverlayInitialColor.g, hurtOverlayInitialColor.b, alpha);
            hurtOverlay.color = currentColor;
            yield return null;
        }
        hurtOverlay.color = hurtOverlayInitialColor;
    }
}
