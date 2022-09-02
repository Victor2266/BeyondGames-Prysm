using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public void Select (string levelName)
    {
        StartCoroutine(MySceneManager.instance.SelectLevel(levelName));
    }
}
