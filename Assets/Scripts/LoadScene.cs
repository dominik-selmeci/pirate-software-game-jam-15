using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string _sceneName;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadSceneByTitle);
    }

    void LoadSceneByTitle() => SceneManager.LoadScene(_sceneName);
}
