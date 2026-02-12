using UnityEngine;
using UnityEngine.SceneManagement;

public class EscManager : MonoBehaviour
{
    public GameObject menuSet;
    public static bool isPause = false;
    public GameObject player;

    private static bool hasLoadedOnce = false; 

    void Awake()
    {
       
        if (!hasLoadedOnce && PlayerPrefs.HasKey("PlayerX"))
        {
            GameLoad();
            hasLoadedOnce = true;
        }
    }

    public void ResumeGame()
    {
        isPause = false;
        menuSet.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    public void SaveGame()
    {
        Debug.Log("게임 저장");
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.Save();

        ResumeGame();
    }

    public void GameLoad()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        player.transform.position = new Vector3(x, y, -1);
        Debug.Log($"로드됨: {x}, {y}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            menuSet.SetActive(isPause);
            Time.timeScale = isPause ? 0f : 1f;
        }
    }
}