using UnityEngine;

namespace Mx.Sound
{
    public class SoundItem : MonoBehaviour
    {
        private Transform audioRoot;
        public SoundState state { get; set; }
        public string soundName { get; set; }
        public AudioSource audioSource { get; set; }
        public AudioClip clip { get; set; }
        /// <summary>声音的长度</summary>
        public float time { get; private set; }
        /// <summary>当前播放的时间</summary>
        public float playTime { get; private set; }
        /// <summary>是否正在播放</summary>
        private bool m_IsPlay = false;
        /// <summary>3D声音的播放位置</summary>
        public Transform target { get; set; }

        /// <summary>声音回调</summary>
        public DelSoundCallback onCallback { get; set; }

        private void Awake()
        {
            if (audioRoot == null)
            {
                if (GameObject.Find("SoundRoot") != null) audioRoot = GameObject.Find("SoundRoot").transform;
                else audioRoot = new GameObject("SoundRoot").transform;
            }
        }

        private void Update()
        {
            if (m_IsPlay)
            {
                playTime = audioSource.time;
                if (time <= playTime)
                {
                    playTime = time;
                    m_IsPlay = false;

                    state = SoundState.Stop;

                    if (onCallback != null) onCallback(soundName, state, time, playTime);
                }

                if (onCallback != null) onCallback(soundName, state, time, playTime);
            }
        }

        /// <summary>播放声音</summary>
        public void Play(bool mute, float volume)
        {
            state = SoundState.Play;
            if (onCallback != null) onCallback(soundName, state, time, playTime);

            audioSource.clip = clip;
            time = clip.length;

            if (target == null) { transform.SetParent(audioRoot); }
            else
            {
                transform.SetParent(target);
                audioSource.spatialBlend = 1;
            }

            audioSource.mute = mute;
            audioSource.volume = volume;
            ChangeProgress(0);
        }

        /// <summary>获取播放进度</summary>
        public float GetPlayProgress()
        {
            if (clip == null) return 0;
            return playTime / time;
        }

        /// <summary>重新播放</summary>
        public void Replay()
        {
            m_IsPlay = true;
            audioSource.Play();
            state = SoundState.Play;

            if (onCallback != null) onCallback(soundName, state, time, playTime);

            ChangeProgress(0);
        }

        /// <summary>继续播放声音</summary>
        public void ContinuePlay()
        {
            m_IsPlay = true;
            audioSource.Play();
            state = SoundState.Play;

            if (onCallback != null) onCallback(soundName, state, time, playTime);

            ChangeProgress(playTime / time);
        }

        /// <summary>暂停播放</summary>
        public void Pause()
        {
            audioSource.Pause();
            m_IsPlay = false;

            if (onCallback != null) onCallback(soundName, state, time, playTime);
        }

        /// <summary>停止播放</summary>
        public void Stop()
        {
            audioSource.Stop();
            m_IsPlay = false;

            if (onCallback != null) onCallback(soundName, state, time, playTime);
        }

        /// <summary>
        /// 切换进度
        /// </summary>
        /// <param name="progress">0-1</param>
        public void ChangeProgress(float progress)
        {
            if (progress >= 1)
            {
                if (audioSource.loop)
                {
                    m_IsPlay = true;
                    audioSource.time = 0;
                    audioSource.Play();
                }
            }
            else
            {
                playTime = time * Mathf.Clamp01(progress);
                audioSource.time = playTime;
                m_IsPlay = true;
                audioSource.Play();
            }
        }

    }
}
