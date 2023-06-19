using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    //UI elements
    public Button playButton;
    public TextMeshProUGUI gameTitle;
    public GameObject nameInputField;
    public TextMeshProUGUI nameErrorText;
    public Text scoreText;
    public float scoreTextSize;
    public GameObject tutorialText;

    //hurt overlay elements
    public GameObject hurtOverlayObject;
    private Image hurtOverlay;
    private Color hurtOverlayInitialColor;

    //prefabs instantiated
    public GameObject EnemySpawnManager;
    public GameObject Map;
    public GameObject[] Trees;
    public int numberOfTrees;

    //player-related variables
    public GameObject[] PlayerHearts = new GameObject[3];
    public string playerName;
    public PlayerController playerControllerScript;
    public string viewMode;

    //game-related variables
    public bool isGameActive = false;
    public float startTime;
    public float timeSinceStart = 0f;

    //enemy-related variables
    private GameObject[] enemies;
    public int enemiesKilledScore;


    // Start is called before the first frame update
    void Start()
    {
        hurtOverlay = hurtOverlayObject.GetComponent<Image>(); //get hurtoverlay components
        hurtOverlayInitialColor = hurtOverlay.color;
        foreach (GameObject Tree in Trees)
        {
            SpawnTrees(Tree, numberOfTrees); //spawn each type of tree
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive) {timeSinceStart = Time.time - startTime;} //update relative time variable
        if (Input.GetKeyDown("1") && isGameActive) {
            viewMode = "1"; //first person if 1 pressed
        }
        if (Input.GetKeyDown("2") && isGameActive) {
            viewMode = "2"; //thord person if 2 pressed
        }
        if (playerControllerScript.health == 0) {
            EndGame(); //if player health is 0, end game
        }
        scoreText.text = "Score: " + (Mathf.Round(timeSinceStart) + enemiesKilledScore); //update score text UI
        
    }

    public void StartGame() 
    {
        if (TestNameIsValid(nameInputField.GetComponent<TMP_InputField>().text)) //if name is valid
        { 
            isGameActive = true; //start game mechanics
            Instantiate(EnemySpawnManager, transform.position, transform.rotation); //start enemies spawning

            gameTitle.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false); //hide start menu UI 
            nameInputField.SetActive(false);
            nameErrorText.gameObject.SetActive(false);
            tutorialText.SetActive(true);
            StartCoroutine(removeTutorialText());

            scoreText.gameObject.SetActive(true); //show score UI and move to new place
            scoreText.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
            scoreText.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
            scoreText.gameObject.GetComponent<Text>().alignment = TextAnchor.LowerRight;
            scoreText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-Screen.width/(2*scoreTextSize), Screen.height/(2*scoreTextSize), 0);
            scoreText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/scoreTextSize, Screen.height/scoreTextSize);

            Cursor.lockState = CursorLockMode.Locked; //lock cursor for better gameplay
            startTime = Time.time; //start time
            enemiesKilledScore = 0; //set enemies killed to 0

            foreach (GameObject heart in PlayerHearts)
            {heart.SetActive(true);} // show all player hearts
        } else {
            nameErrorText.gameObject.SetActive(true); //if name is invalid, show error text
        }
    }
    public void EndGame(){ //at end of game
        playerControllerScript.health = 3; //reset helath
        isGameActive = false; //stop game mechanics

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy); //destroy all enemies
        }
        viewMode = "1"; //back to first person

        gameTitle.gameObject.SetActive(true); //show all UI elements
        playButton.gameObject.SetActive(true);
        nameInputField.GetComponent<TMP_InputField>().text = "";
        nameInputField.SetActive(true);

        scoreText.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        scoreText.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); //move score
        scoreText.gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        scoreText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -140, 0);

        Cursor.lockState = CursorLockMode.None; //let mouse be free
    }
    public bool TestNameIsValid(string testPlayerName) //test name is between 2-10 characters inclusive and contains at least 1 letter
    {
        return (testPlayerName.Length >= 2 && testPlayerName.Length <= 10 
        && Regex.IsMatch(testPlayerName, "[a-zA-Z]"));
    }
    public void removeHeart(int playerHealth) //remove 1 heart
    {
        PlayerHearts[playerHealth].SetActive(false);
        StartCoroutine(hurtOverlayCountdown(1.5f));

    }
    public void SpawnTrees(GameObject tree, int numberOfTrees) //spawn a bunch of trees; random rotation position and size
    {
        for (int i = 0; i < numberOfTrees; i++) //FOR LOOP
        {
            float scaleFactor = Mathf.Pow(10, Random.Range(-1f, 1f));//scale factor uses exponentiation
            float rotationFactor = Random.Range(0, 359);//rotation uses linear scale
            GameObject lastTree = Instantiate(tree, new Vector3(Random.Range(-Map.transform.localScale.x, Map.transform.localScale.x), 0, Random.Range(-Map.transform.localScale.z, Map.transform.localScale.z))*5,
            transform.rotation);
            lastTree.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor); //change scale and rotation
            lastTree.transform.Rotate(new Vector3(0, rotationFactor, 0));

        }


    }

    public IEnumerator hurtOverlayCountdown(float fadeDuration) //hurt overlay function to run in background when hurt
    {
        hurtOverlay.color = Color.red; //color of overlay
        
        float timer = 0f;
        while (timer < fadeDuration) //WHILE LOOP
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer/fadeDuration); //change alpha value (transparency) as time increases
            Color currentColor = new Color(hurtOverlayInitialColor.r, hurtOverlayInitialColor.g, hurtOverlayInitialColor.b, alpha);
            hurtOverlay.color = currentColor;
            yield return null;
        }
        hurtOverlay.color = hurtOverlayInitialColor; //reset color
    }

    public IEnumerator removeTutorialText()
    {
        yield return new WaitForSeconds(10f);
        Destroy(tutorialText);
    }
}
