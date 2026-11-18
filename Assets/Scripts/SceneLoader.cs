using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private SceneRecorder sceneRecorder;
    [SerializeField] private int currLevelNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneRecorder = FindAnyObjectByType<SceneRecorder>();
        if(currLevelNum != -1)
        {
            sceneRecorder.currentSceneNumber = currLevelNum;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level0Dialogue");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Level" + sceneRecorder.currentSceneNumber);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
