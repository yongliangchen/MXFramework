/***
 * 
 *    Title: MXFramework
 *           主题:声音管理的基类
 *    Description: 
 *           功能：1.封装了声音播放、暂停、继续、停止、切换播放进度、静音、设置音量 功能
 *                2.封装了2D声音和3D声音处理
 *                3.封装了声音状态监测
 *                4.封装了声音缓存功能
 *                
 *    Date: 2020
 *    Version: v5.0版本
 *    Modify Recoder:     
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mx.Sound
{
    public class BaseSound : MonoBehaviour
    {
        /// <summary>所有声音缓存库</summary>
        private Dictionary<string, AudioClip> m_DicAllAudioClip = new Dictionary<string, AudioClip>();
        /// <summary>所有播放声音的缓存</summary>
        private Dictionary<string, SoundItem> m_DicAllSound = new Dictionary<string, SoundItem>();

        private bool mute = false;
        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;
                muteToggle(GetAllSoundName());
            }
        }

        private float volume = 1;
        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                setVolume(GetAllSoundName());
            }
        }

        /// <summary>播放一个2D声音</summary>
        public void PlaySound2D(string soundName, bool isLoop = false, DelSoundCallback callback = null)
        {
            playSound(soundName, null, isLoop, callback);
        }

        /// <summary>播放一个3D声音</summary>
        public void PlaySound3D(string soundName, Transform target, bool isLoop = false, DelSoundCallback callback = null)
        {
            playSound(soundName, target, isLoop, callback);
        }

        /// <summary>设置循环播放</summary>
        public void SetLoop(string soundName, bool isLoop)
        {
            DeleteNullSound(soundName);
            if (m_DicAllSound.ContainsKey(soundName)) m_DicAllSound[soundName].audioSource.loop = isLoop;
        }

        /// <summary>关闭声音播放</summary>
        public void CloseSound(params string[] soundNames)
        {
            for(int i=0;i< soundNames.Length;i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                if (m_DicAllSound.ContainsKey(soundName))
                {
                  if(m_DicAllSound[soundName].state== SoundState.Play|| m_DicAllSound[soundName].state == SoundState.Pause)  m_DicAllSound[soundName].Stop();
                }
            }
        }

        /// <summary>关闭所有声音播放</summary>
        public void CloseAllSound()
        {
            CloseSound(GetAllSoundName());
        }

        /// <summary>继续播放声音（从暂停状态进度继续播放）</summary>
        public void ContinuePlay(params string[] soundNames)
        {
            for (int i = 0; i < soundNames.Length; i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                if (m_DicAllSound.ContainsKey(soundName))
                {
                    if (m_DicAllSound[soundName].state == SoundState.Pause) m_DicAllSound[soundName].ContinuePlay();
                }
            }
        }

        /// <summary>继续播放全部声音（从暂停状态进度继续播放）</summary>
        public void ContinuePlayAll()
        {
            ContinuePlay(GetAllSoundName());
        }

        /// <summary>暂停声音播放</summary>
        public void PauseSound(params string[] soundNames)
        {
            for (int i = 0; i < soundNames.Length; i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                if (m_DicAllSound.ContainsKey(soundName))
                {
                    if (m_DicAllSound[soundName].state == SoundState.Play) m_DicAllSound[soundName].Pause();
                }
            }
        }

        /// <summary>暂停所有声音播放</summary>
        public void PauseAllSounds()
        {
            PauseSound(GetAllSoundName());
        }

        /// <summary>停止播放声音</summary>
        public void StopSound(params string[] soundNames)
        {
            for (int i = 0; i < soundNames.Length; i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                if (m_DicAllSound.ContainsKey(soundName))
                {
                    if (m_DicAllSound[soundName].state != SoundState.Stop) m_DicAllSound[soundName].Stop();
                }
            }
        }

        /// <summary>停止播放所有声音</summary>
        public void StopAllSounds()
        {
            StopSound(GetAllSoundName());
        }

        /// <summary>重新播放声音（进度从0开始）</summary>
        public void ReplaySounds(params string[] soundNames)
        {
            for (int i = 0; i < soundNames.Length; i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                if (m_DicAllSound.ContainsKey(soundName))
                {
                    m_DicAllSound[soundName].Replay();
                }
            }
        }

        /// <summary>重新播放所有声音（进度从0开始）</summary>
        public void ReplayAllSounds()
        {
            ReplaySounds(GetAllSoundName());
        }

        /// <summary>添加2D声音到管理</summary>
        public void AddSound(string soundName, AudioClip Clip)
        {
            DeleteNullSound(soundName);
            if (!m_DicAllAudioClip.ContainsKey(soundName)) m_DicAllAudioClip.Add(soundName, Clip);
        }

        /// <summary>移除声音及相关声音组件</summary>
        public void RemoveSound(params string[] soundNames)
        {
            for(int i=0;i< soundNames.Length;i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                if (m_DicAllSound.ContainsKey(soundName))
                {
                    m_DicAllSound[soundName].Stop();
                    Destroy(m_DicAllSound[soundName].gameObject);
                    m_DicAllSound.Remove(soundName);
                }

                if (m_DicAllAudioClip.ContainsKey(soundName)) m_DicAllAudioClip.Remove(soundName);
            }
        }

        /// <summary>移除所有声音及相关声音组件</summary>
        public void RemoveAllSound()
        {
            RemoveSound(GetAllSoundName());
            m_DicAllSound.Clear();
            m_DicAllAudioClip.Clear();
        }

        /// <summary>获取所有AudioClip名称</summary>
        public string[] GetAllAudioClipName()
        {
            string[] soundNameArr = m_DicAllAudioClip.Keys.ToArray<string>();
            return soundNameArr;
        }

        /// <summary>获取所有Sound名称</summary>
        public string[] GetAllSoundName()
        {
            string[] soundNameArr = m_DicAllSound.Keys.ToArray<string>();
            return soundNameArr;
        }

        /// <summary>通过名称获取Itme</summary>
        public SoundItem GetSoundItemByName(string soundName)
        {
            SoundItem soundItem=null;
            m_DicAllSound.TryGetValue(soundName, out soundItem);
            return soundItem;
        }

        /// <summary>判断库存中是否有这个声音</summary>
        public bool Contains(string soundName)
        {
            DeleteNullSound(soundName);
            return m_DicAllAudioClip.ContainsKey(soundName);
        }

        /// <summary>获取声音播放进度（取值范围：0-1）</summary>
        public float GetProgressByName(string soundName)
        {
            DeleteNullSound(soundName);
            float Progress = 0;
            if (!m_DicAllSound.ContainsKey(soundName)) Progress = m_DicAllSound[soundName].GetPlayProgress();
            return Progress;
        }

        /// <summary>获取声音播放时间长度（单位：秒）</summary>
        public float GetPlayTimeByName(string soundName)
        {
            DeleteNullSound(soundName);
            float time=0;
            if (!m_DicAllSound.ContainsKey(soundName)) time = m_DicAllSound[soundName].playTime;
            return time;
        }

        /// <summary>获取声音时间长度（单位：秒）</summary>
        public float GetTimeByName(string soundName)
        {
            DeleteNullSound(soundName);
            float length = 0;
            if (!m_DicAllSound.ContainsKey(soundName)) length = m_DicAllSound[soundName].time;
            return length;
        }

        /// <summary>设置播放进度（0-1）</summary>
        public void SetProgress(string soundName, float progress)
        {
            DeleteNullSound(soundName);
            if (!m_DicAllSound.ContainsKey(soundName)) m_DicAllSound[soundName].ChangeProgress(progress);
        }

        /// <summary>播放声音</summary>
        private void playSound(string soundName, Transform target, bool isLoop, DelSoundCallback callback)
        {
            DeleteNullSound(soundName);

            if (!m_DicAllAudioClip.ContainsKey(soundName)|| m_DicAllAudioClip[soundName]==null) return;

            if (m_DicAllSound.ContainsKey(soundName) && m_DicAllSound[soundName] != null)
            {
                m_DicAllSound[soundName].target = target;
                m_DicAllSound[soundName].audioSource.loop = isLoop;
                m_DicAllSound[soundName].onCallback = callback;

                m_DicAllSound[soundName].Play(Mute, Volume);
            }
            else
            {
                if (m_DicAllSound.ContainsKey(soundName)) m_DicAllSound.Remove(soundName);

                GameObject item = new GameObject(soundName);
                AudioSource audioSource = item.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = isLoop;
                SoundItem soundItem = item.AddComponent<SoundItem>();
                soundItem.soundName = soundName;
                soundItem.onCallback = callback;
                soundItem.target = target;
                soundItem.audioSource = audioSource;
                soundItem.clip = m_DicAllAudioClip[soundName];
                soundItem.Play(Mute, Volume);
                m_DicAllSound.Add(soundName, soundItem);
            }
        }

        /// <summary>移除空的声音</summary>
        private void DeleteNullSound(string soundName)
        {
            if (m_DicAllSound.ContainsKey(soundName) && m_DicAllSound[soundName] == null) m_DicAllSound.Remove(soundName);
            if (m_DicAllAudioClip.ContainsKey(soundName) && m_DicAllAudioClip[soundName] == null) m_DicAllAudioClip.Remove(soundName);
        }

        /// <summary>设置静音</summary>
        private void muteToggle(params string[] soundNames)
        {
            for(int i=0;i< soundNames.Length;i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                SoundItem soundItem = m_DicAllSound[soundName];
                if (soundItem != null) soundItem.audioSource.mute = Mute;
            }
        }

        /// <summary>设置音量</summary>
        private void setVolume(params string[] soundNames)
        {
            for (int i = 0; i < soundNames.Length; i++)
            {
                string soundName = soundNames[i];
                DeleteNullSound(soundName);
                SoundItem soundItem = m_DicAllSound[soundName];
                if (soundItem != null) soundItem.audioSource.volume = Volume;
            }
        }
    }
}