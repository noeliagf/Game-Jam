using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneLoader : MonoBehaviour
{
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title");  // Cargar la escena de título
    }
}