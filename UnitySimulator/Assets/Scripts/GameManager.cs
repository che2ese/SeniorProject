using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private bool isButtonPressed;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    private void Update()
    {
        if (!isButtonPressed)
            Time.timeScale = 1f;
        else
            Time.timeScale = 2f;
    }
    public void PointerDown()
    {
        isButtonPressed = true;
    }
    public void PointerUp()
    {
        isButtonPressed = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
