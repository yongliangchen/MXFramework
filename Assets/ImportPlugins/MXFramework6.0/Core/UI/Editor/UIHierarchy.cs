using UnityEditor;
using UnityEngine;

namespace Mx.UI
{
    public class UIHierarchy
    {
        [MenuItem("GameObject/MXFramework/UI/UIRoot", false, 0)]
        private static void createUIRoot()
        {
            if (GameObject.Find(UIDefine.NAME_UIROOT) == null)
            {
                GameObject obj = new GameObject(UIDefine.NAME_UIROOT);
                UIRoot uIRoot= obj.AddComponent<UIRoot>();
                uIRoot.CreateUIRoot();
            }
            else
            {
                Debug.LogWarning("Mx.UI.UIHierarchy/createUIRoot()/ uiRoot exist！");
            }
        }
    }
}