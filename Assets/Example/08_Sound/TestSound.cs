using System;
using Mx.Net;
using Mx.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.Example
{
    public class TestSound : MonoBehaviour
    {
        /// <summary>测试时候使用的声音库路径</summary>
        private string[] soundPathArr = {"https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/TestAudio.wav", "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/Test/Earth.wav",
            "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/Test/Jupiter.wav" ,
            "https://hisceneapp.oss-cn-shenzhen.aliyuncs.com/Test/Mars.wav"};

        private int index = 0;

        public Button playButton;
        public Slider progressSlider;
        public Slider downloadProgressSlider;
        public Slider volumeSlider;
        public Text tiemText;
        public Text nameText;

        public Toggle muteToggle;
        public Toggle loopToggle;
        private bool isPlay = false;

        private SoundItem m_CurrentSundItem;
        private string m_CurrentSoundName;

        private BaseSound m_Sound;
        private void Awake()
        {
            m_Sound = gameObject.AddComponent<BaseSound>();

            tiemText.text = "00/00";
            nameText.text = null;
            progressSlider.value = 0;
            progressSlider.interactable = false;
            downloadProgressSlider.value = 0;

            playButton.transform.Find("Text").GetComponent<Text>().text = "播放";

            volumeSlider.value = m_Sound.Volume;
            volumeSlider.onValueChanged.AddListener(volumeChanged);
            muteToggle.isOn = m_Sound.Mute;
            muteToggle.onValueChanged.AddListener(muteToggleChanged);
            loopToggle.onValueChanged.AddListener(loopToggleChanged);
        }

        /// <summary>音量发生改变</summary>
        private void volumeChanged(float value)
        {
            m_Sound.Volume = value;
        }

        /// <summary>静音Toggle发生改变</summary>
        private void muteToggleChanged(bool value)
        {
            m_Sound.Mute = value;
        }

        /// <summary>循环声音Toggle发生改变</summary>
        private void loopToggleChanged(bool value)
        {
            if (!string.IsNullOrEmpty(m_CurrentSoundName)) m_Sound.SetLoop(m_CurrentSoundName, value);
        }

        /// <summary>点击播放或者暂停按钮</summary>
        public void OnClickPlayButton()
        {
            isPlay = !isPlay;
            if (isPlay) playSound(soundPathArr[index]);
            else pauseSound();
        }

        /// <summary>点击上一个曲目</summary>
        public void OnClickFirstTrackButton()
        {
            if (!string.IsNullOrEmpty(m_CurrentSoundName))
            {
                m_Sound.StopSound(m_CurrentSoundName);
                WebRequest.Instance.Dispose(m_CurrentSoundName);
            }

            index--;
            if (index < 0) index = soundPathArr.Length - 1;
            playSound(soundPathArr[index]);
        }

        /// <summary>点击下一首歌</summary>
        public void OnClicnNextTrackButton()
        {
            if (!string.IsNullOrEmpty(m_CurrentSoundName))
            {
                m_Sound.StopSound(m_CurrentSoundName);
                WebRequest.Instance.Dispose(m_CurrentSoundName);
            }

            index++;
            if (index > soundPathArr.Length - 1) index =0;

            playSound(soundPathArr[index]);
        }

        /// <summary>播放声音</summary>
        private void playSound(string soundName)
        {
            isPlay = true;
            m_CurrentSoundName = soundName;

            progressSlider.interactable = false;

            downloadProgressSlider.value = 0;
            progressSlider.value = 0;
            tiemText.text = "00/00";

            string[] tempArr = m_CurrentSoundName.Split('/');
            string tempSoundName = tempArr[tempArr.Length - 1];
            nameText.text = tempSoundName;

            if (m_Sound.Contains(m_CurrentSoundName))
            {
                downloadProgressSlider.value = 1;
                m_CurrentSundItem = m_Sound.GetSoundItemByName(m_CurrentSoundName);
                if (m_CurrentSundItem != null && m_CurrentSundItem.state == SoundState.Pause) m_CurrentSundItem.ContinuePlay();
                else m_Sound.PlaySound2D(m_CurrentSoundName, loopToggle.isOn, playProgress);
           
                progressSlider.interactable = true;
                if (!string.IsNullOrEmpty(m_CurrentSoundName)) m_Sound.SetLoop(m_CurrentSoundName, loopToggle.isOn);
            }
            else getAudioClip(m_CurrentSoundName);

            updatePlayState();
        }

        /// <summary>暂停声音</summary>
        private void pauseSound()
        {
            m_Sound.PauseSound(m_CurrentSoundName);
            updatePlayState();
        }

        /// <summary>更新播放状态</summary>
        private void updatePlayState()
        {
            playButton.transform.Find("Text").GetComponent<Text>().text = (isPlay) ? "暂停" : "播放";
        }

        /// <summary>获取网络Clip(</summary>
        private void getAudioClip(string uri)
        {
            WebRequest.Instance.GetAudioClip(uri, AudioType.WAV, (progress) => {downloadProgressSlider.value = progress; }, (error, clip) =>
             {
                 if (string.IsNullOrEmpty(error))
                 {
                     downloadProgressSlider.value = 1;
                     m_Sound.AddSound(uri, clip);

                     //资源加载完成后首先判断当前是否是播放状态，然后判断当前播放的曲目是不是加载完成的这首
                     if (isPlay && uri.Equals(m_CurrentSoundName)) m_Sound.PlaySound2D(uri, false, playProgress);

                     progressSlider.interactable = true;
                     if (!string.IsNullOrEmpty(m_CurrentSoundName)) m_Sound.SetLoop(m_CurrentSoundName, loopToggle.isOn);
                 }
                 else
                 {
                     isPlay = false;
                     updatePlayState();
                     Debug.LogWarning(GetType() + "/GetAssetBundle()/error!" + error);
                 }

            }, 60);
        }

        /// <summary>播放进度回调</summary>
        private void playProgress(string playSoundName, SoundState state, float time, float playTime)
        {
            Debug.Log("state:" + state);
            tiemText.text = FormatTime(playTime) + "/" + FormatTime(time);
            progressSlider.value = playTime / time;

            isPlay = (state == SoundState.Play);
            updatePlayState();

            if (state == SoundState.PlayFinish && !loopToggle.isOn) OnClicnNextTrackButton();
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public string FormatTime(float seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }

    }
}