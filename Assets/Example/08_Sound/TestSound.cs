using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Net;
using Mx.Sound;
using UnityEngine.UI;
using System;

namespace Mx.Example
{
    public class TestSound : MonoBehaviour
    {
        public Button playButton;
        public Slider progressSlider;
        public Slider downloadProgressSlider;
        public Text tiemText;
        public Text nameText;

        private BaseSound m_Sound;
        private void Awake()
        {
            tiemText.text = "00/00";
            nameText.text = null;
            progressSlider.value = 0;
            downloadProgressSlider.value = 0;
            m_Sound = gameObject.AddComponent<BaseSound>();
        }

        /// <summary>测试获取Clip(</summary>
        public void GetAudioClip(string uri)
        {
            downloadProgressSlider.value = 0;
            progressSlider.value = 0;
            tiemText.text = "00/00";
            string[] tempArr = uri.Split('/');
            string soundName = tempArr[tempArr.Length - 1];
            nameText.text = soundName;

            WebRequest.Instance.GetAudioClip(uri, AudioType.WAV, (progress)=> { downloadProgressSlider.value = progress; }, (error, clip) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    downloadProgressSlider.value = 1;
                    m_Sound.AddSound(uri, clip);
                    m_Sound.PlaySound2D(uri,false,(playSoundName, state, time, playTime)=> {

                        tiemText.text = FormatTime(playTime) + "/" + FormatTime(time);
                        progressSlider.value = playTime / time;
                    });
                }
                else
                {
                    Debug.LogWarning(GetType() + "/GetAssetBundle()/error!" + error);
                }

            }, 20);
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