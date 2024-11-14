using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SpriteRenderer Background;
    public Sprite[] OpenSceneSprites;
    public GameObject OutsideWorldBackGround;
    public GameObject InsideWorldBackGround;
    public Sprite[] Scene1ResultPicsSuccess;
    public Sprite[] Scene1ResultPicsFailed;
    public string OpenSceneText;
    public string SucessText;
    public string FailText;
    public Text TextNode;
    private Vector2 originScale;
    private int index;
    private bool success;
    public enum GAME_STAGE
    {
        SCENE1_OPEN,
        SCENE1_INSIDE_WORLD,
        SCENE1_END,
    }

    public GAME_STAGE CurrentStage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void HandleClicked()
    {
        switch(CurrentStage)
        {
            case GAME_STAGE.SCENE1_OPEN:
                index++;
                if(index>=OpenSceneSprites.Length)
                {
                    CurrentStage++;
                    HandleClicked();
                }
                else
                {
                    Background.sprite = OpenSceneSprites[index];
                }
                break;
            case GAME_STAGE.SCENE1_INSIDE_WORLD:
                OutsideWorldBackGround.SetActive(false);
                InsideWorldBackGround.SetActive(true);
                break;
            case GAME_STAGE.SCENE1_END:
                index++;
                if(index == 1 && (SceneManager.GetActiveScene().buildIndex == 2|| SceneManager.GetActiveScene().buildIndex == 3))
                {
                    Background.gameObject.transform.localScale = new Vector2(0.3f, 0.3f);
                }
                if (success)
                {
                    if (index >= Scene1ResultPicsSuccess.Length)
                    {
                        LoadNextScene();
                        break;
                    }
                    Background.sprite = Scene1ResultPicsSuccess[index];
                }
                else
                {
                    if (index >= Scene1ResultPicsFailed.Length)
                    {
                        Background.gameObject.transform.localScale = originScale;
                        ResetPlayerAndGameStage();
                        break;
                    }
                    Background.sprite = Scene1ResultPicsFailed[index];
                }
                break;
        }
    }

    public void ResetPlayerAndGameStage()
    {
        CurrentStage = GAME_STAGE.SCENE1_INSIDE_WORLD;
        TextNode.text = OpenSceneText;
        HandleClicked();
        if(PlayerController.Instance!=null) PlayerController.Instance.HandleResetPlayer();
    }


    public void HandlePlayerGetToEndInInsideWorld(bool success)
    {
        CurrentStage++;
        OutsideWorldBackGround.SetActive(true);
        InsideWorldBackGround.SetActive(false);
        index = 0;
        Background.sprite = success ? Scene1ResultPicsSuccess[index] : Scene1ResultPicsFailed[index];
        this.success = success;
        TextNode.text = success ? SucessText : FailText;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentStage = GAME_STAGE.SCENE1_OPEN;
        index = 0;
        if(Background!=null) Background.sprite = OpenSceneSprites[index];
        success = false;
        if(TextNode!=null) TextNode.text = OpenSceneText;
        if(Background != null)originScale = Background.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
