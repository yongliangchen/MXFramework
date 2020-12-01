namespace Mx.Sound
{
    public class SoundDefine 
    {

    }

    /// <summary>UI窗体状态</summary>
    public enum SoundState
    {
        /// <summary>未知状态</summary>
        None,
        /// <summary>加载当中</summary>
        //Loading,
        /// <summary>播放状态</summary>
        Play,
        /// <summary>暂停状态</summary>
        Pause,
        /// <summary>停止状态</summary>
        Stop,
        /// <summary>发生错误</summary>
        Error,
    }

    /// <summary>
    /// 声音回调
    /// </summary>
    /// <param name="soundName">声音名字</param>
    /// <param name="state">状态</param>
    /// <param name="time">声音的长度</param>
    /// <param name="playTime">播放时间</param>
    public delegate void DelSoundCallback(string soundName, SoundState state, float time, float playTime);
}