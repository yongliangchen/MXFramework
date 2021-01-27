using UnityEngine;

namespace Mx.Utils
{
    public delegate void CompleteEvent();
    public delegate void UpdateEvent(float t);

    /// <summary>计时器</summary>
    public class Timer : MonoBehaviour
    {
        private bool m_IsLog = true;
        private UpdateEvent m_UpdateEvent;
        private CompleteEvent m_OnCompleted;
        private float m_TimeTarget;
        private float m_TimeStart;
        private float m_TimeNow;
        private float m_OffsetTime;
        private bool m_IsTimer;
        private bool m_IsDestory = true;
        private bool m_IsEnd;
        private bool m_IsIgnoreTimeScale = true;
        private bool m_IsRepeate;
        private float m_PauseTime;
        private float m_GetTime { get { return m_IsIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time; } }
        private float m_Now;

        private void Update()
        {
            if (m_IsTimer)
            {
                m_TimeNow = m_GetTime - m_OffsetTime;
                m_Now = m_TimeNow - m_TimeStart;
                if (m_UpdateEvent != null) m_UpdateEvent(Mathf.Clamp01(m_Now / m_TimeTarget));
                if (m_Now > m_TimeTarget)
                {
                    if (m_OnCompleted != null) m_OnCompleted();
                    if (!m_IsRepeate) Destory();
                    else ReStartTimer();
                }
            }
        }

        private void OnApplicationPause(bool isPause)
        {
            if (isPause) { PauseTimer(); }
            else { ConnitueTimer(); }
        }

        /// <summary>获取剩余时间</summary>
        public float GetLeftTime()
        {
            return Mathf.Clamp(m_TimeTarget - m_Now, 0, m_TimeTarget);
        }

        /// <summary>计时结束</summary>
        public void Destory()
        {
            m_IsTimer = false;
            m_IsEnd = true;
            if (m_IsDestory) Destroy(gameObject);
        }

        /// <summary>暂停计时</summary>
        public void PauseTimer()
        {
            if (m_IsEnd) { if (m_IsLog) Debug.Log("计时已经结束！"); }

            else
            {
                if (m_IsTimer)
                {
                    m_IsTimer = false;
                    m_PauseTime = m_GetTime;
                }
            }
        }

        /// <summary>继续计时</summary>
        public void ConnitueTimer()
        {
            if (m_IsEnd) { if (m_IsLog) Debug.Log("计时已经结束！请从新计时！"); }

            else
            {
                if (!m_IsTimer)
                {
                    m_OffsetTime += (m_GetTime - m_PauseTime);
                    m_IsTimer = true;
                }
            }
        }

        /// <summary>重新启动定时器</summary>
        public void ReStartTimer()
        {
            m_TimeStart = m_GetTime;
            m_OffsetTime = 0;
        }

        /// <summary>更改目标时间</summary>
        public void ChangeTargetTime(float time)
        {
            m_TimeTarget += time;
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="time">倒计时时间</param>
        /// <param name="onCompleredEvent">计时完成回调</param>
        /// <param name="update">更新事件</param>
        /// <param name="m_IsIgnoreTimeScale">是否忽略时间速率</param>
        /// <param name="m_IsRepeate">是否重复</param>
        /// <param name="m_IsDestory">倒计时完成后是否删除</param>
        public void StartTiming(float time, CompleteEvent onCompleredEvent, UpdateEvent update = null, bool m_IsIgnoreTimeScale = true, bool m_IsRepeate = false, bool m_IsDestory = true)
        {
            m_TimeTarget = time;
            if (onCompleredEvent != null)
                m_OnCompleted = onCompleredEvent;
            if (update != null)
                m_UpdateEvent = update;
            this.m_IsDestory = m_IsDestory;
            this.m_IsIgnoreTimeScale = m_IsIgnoreTimeScale;
            this.m_IsRepeate = m_IsRepeate;

            m_TimeStart = m_GetTime;
            m_OffsetTime = 0;
            m_IsEnd = false;
            m_IsTimer = true;
        }

        /// <summary>创建计时器</summary>
        public static Timer CreateTimer(string gobjName = "Timer")
        {
            GameObject g = new GameObject(gobjName);
            Timer timer = g.AddComponent<Timer>();
            return timer;
        }

    }//class_end
}