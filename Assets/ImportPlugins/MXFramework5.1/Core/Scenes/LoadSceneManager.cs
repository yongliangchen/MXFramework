namespace Mx.Scene
{
    public class LoadSceneManager 
    {
        /// <summary>同步加载场景</summary>
        public static void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        /// <summary>异步加载场景</summary>
        public static void LoadSceneAsyn(string sceneName)
        {
            LoadSceneData.NextSceneName = sceneName;
        }

        /// <summary>异步加载场景</summary>
        public static void LoadSceneAsyn(string sceneName,string loadSceneUIName)
        {
            LoadSceneData.NextSceneName = sceneName;
            LoadSceneData.LoadSceneUIName = loadSceneUIName;
        }
    }
}