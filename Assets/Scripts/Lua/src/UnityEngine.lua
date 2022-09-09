local TAG = "UnityEngine"

GameObject = CS.UnityEngine.GameObject
WWW = CS.UnityEngine.WWW
Instantiate = CS.UnityEngine.Object.Instantiate
Destroy=CS.UnityEngine.Object.Destroy


Application = {}

--退出应用
function Application.Quit()
    CS.UnityEngine.Application.Quit()
end