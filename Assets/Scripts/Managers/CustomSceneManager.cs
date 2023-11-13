using UnityEngine.SceneManagement;

public class CustomSceneManager : Singleton<CustomSceneManager>
{
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene((int)SceneType.MAIN_MENU, LoadSceneMode.Single);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene((int)SceneType.BATTLE, LoadSceneMode.Single);
    }
}
