using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{

    public GameObject LevelPanelPrefab;
    public GameObject ScrollViewContent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < LevelManager.Instance.levels.Count; i++)
        {
            LevelInfo info = LevelManager.Instance.levels[i];
            GameObject go = Instantiate(LevelPanelPrefab, ScrollViewContent.transform);
            go.GetComponent<LevelPanel>().SetLevelInformation(i, info.LevelThumb, info.LevelName);
        }
    }

}
