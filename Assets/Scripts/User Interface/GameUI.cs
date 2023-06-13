using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI;
    public GameObject allbtns;

    private bool btns;

    [Header("PreGame")]
    public Button soundBtn;
    public Sprite soundOnSpr, soundOffSpr;

    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevetImg;
    public Image nextLevelImg;
    public Text currentLevelText, nextLevelText;

    [Header("Finish")]
    public Text finishLevelText;

    [Header("GameOver")]
    public Text gameOverScoreText;
    public Text gameOverBestText;

    private Material playerMat;
    private Player player;
    private bool isGameStarted;

    void Awake()
    {
        isGameStarted = false;
        playerMat = FindObjectOfType<Player>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = FindObjectOfType<Player>();

        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;
        levelSlider.color = playerMat.color;
        currentLevetImg.color = playerMat.color;
        nextLevelImg.color = playerMat.color;

        soundBtn.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
    }

    private void Start()
    {
        currentLevelText.text = FindObjectOfType<LevelSpawner>().level.ToString();
        nextLevelText.text = FindObjectOfType<LevelSpawner>().level + 1 + "";
    }

    void Update()
    {
        if(player.currentPlayerState == Player.PlayerState.Prepare)
        {
            inGameUI.SetActive(false);
            if (SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOnSpr)
                soundBtn.GetComponent<Image>().sprite = soundOnSpr;
            else if (!SoundManager.instance.sound && soundBtn.GetComponent<Image>().sprite != soundOffSpr)
                soundBtn.GetComponent<Image>().sprite = soundOffSpr;
        }

        if (player.currentPlayerState == Player.PlayerState.Playing)
        {
            inGameUI.SetActive(true);
        }

        if(Input.GetMouseButtonDown(0) &&  !IgnoreUI() && player.currentPlayerState == Player.PlayerState.Prepare && !isGameStarted)
        {
                player.currentPlayerState = Player.PlayerState.Prepare;
                homeUI.SetActive(false);
                inGameUI.SetActive(false);
                finishUI.SetActive(false);
                gameOverUI.SetActive(false);
                isGameStarted = true;

        }

        if(player.currentPlayerState == Player.PlayerState.Finish)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level " + FindObjectOfType<LevelSpawner>().level;
        }

        if(player.currentPlayerState == Player.PlayerState.Died)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = ScoreManager.instance.score.ToString();
            gameOverBestText.text = PlayerPrefs.GetInt("HighScore").ToString();

            if (Input.GetMouseButtonDown(0))
            {
                ScoreManager.instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void Settings()
    {
        btns = !btns;
        allbtns.SetActive(btns);
    }
}
