using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Select (string levelName)
    {
        StartCoroutine(MySceneManager.instance.SelectLevel(levelName));
    }
}
