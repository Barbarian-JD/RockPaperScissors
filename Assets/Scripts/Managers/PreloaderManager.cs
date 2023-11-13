using UnityEngine;

public class PreloaderManager : MonoBehaviour
{
    void Start()
    {
        PreloadSaveData();

        CustomSceneManager.Instance.LoadMainMenuScene();
    }

    private void PreloadSaveData()
    {
        PlayerManager.Instance.LoadPlayerData();
    }
}
