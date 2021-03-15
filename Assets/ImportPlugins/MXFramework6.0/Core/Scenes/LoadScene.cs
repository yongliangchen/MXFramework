using System.Collections;
using Mx.UI;
using UnityEngine;

namespace Mx.Scene
{
    /// <summary> 加载场景管理 </summary>
    public class LoadScene : MonoBehaviour
    {
        private AsyncOperation m_Async = null;
        private float m_progress=0;

        private void Awake()
        {
            StartCoroutine(loadSceneAsyn(LoadSceneData.NextSceneName));
        }

        private void OnDestroy()
        {
            
        }

        /// <summary>异步加载场景</summary>
        private IEnumerator loadSceneAsyn(string sceneName)
        {
            m_Async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            m_Async.allowSceneActivation = false;

            while (!m_Async.isDone)
            {
                m_progress = (m_Async.progress < 0.9f) ? m_Async.progress : 1;
                refreshLoadSceneProgress();

                //场景加载完成
                if (m_progress >= 0.9)
                {
                    loadSceneFinish();
                    m_Async.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        /// <summary>刷新加载场景进度</summary>
        private void refreshLoadSceneProgress()
        {
            if(!string.IsNullOrEmpty(LoadSceneData.LoadSceneUIName))
            {
                //UIManager.Instance.SendMessageToUIForm(LoadSceneData.LoadSceneUIName, "RefreshLoadSceneProgress", m_progress);
            }
        }

        /// <summary>场景加载完成</summary>
        private void loadSceneFinish()
        {
            if (!string.IsNullOrEmpty(LoadSceneData.LoadSceneUIName))
            {
                //UIManager.Instance.CloseUIForms(LoadSceneData.LoadSceneUIName);
            }

            LoadSceneData.LoadSceneUIName = null;
            LoadSceneData.NextSceneName = null;
        }

    }
}