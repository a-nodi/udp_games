using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MoveScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
