using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRecorder : MonoBehaviour
{
    public static SceneRecorder Instance {get; private set; }
    public int currentSceneNumber = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.currentSceneNumber = 0;
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
