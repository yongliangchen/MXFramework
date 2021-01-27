using DG.Tweening;
using UnityEngine;
using System;

namespace Mx.UI
{
    /// <summary>UI动画</summary>
    public class UIAnimation : MonoBehaviour
    {
        #region 数据申明

        private Tween m_Tween;
        public float Duration { get; set; }
        public Ease Ease { get; set; }
        public EnumUIAnimationStyle AnimationStyle { get; private set; }

        #endregion

        #region 公开函数

        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="animationStyle">动画类型</param>
        /// <param name="Duration">持续时间</param>
        /// <param name="ease">动画曲线</param>
        /// <param name="finish">完成后回调</param>
        public void OpenUIAnimation(EnumUIAnimationStyle animationStyle, float Duration, Ease ease, Action finish)
        {
            this.AnimationStyle = animationStyle;
            this.Duration = Duration;
            this.Ease = ease;

            switch (animationStyle)
            {
                case EnumUIAnimationStyle.CenterScaleBigNomal:

                    transform.localScale = Vector3.zero;
                    Scale(Vector3.one, Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.TopToSlide:

                    transform.localPosition = new Vector3(transform.localPosition.x, 1000, transform.localPosition.z);
                    Move(new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.DoweToSlide:

                    transform.localPosition = new Vector3(transform.localPosition.x, -1000, transform.localPosition.z);
                    Move(new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.LeftToSlide:

                    transform.localPosition = new Vector3(-1000, transform.localPosition.y, transform.localPosition.z);
                    Move(new Vector3(0, transform.localPosition.y, transform.localPosition.z), Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.RightToSlide:

                    transform.localPosition = new Vector3(1000, transform.localPosition.y, transform.localPosition.z);
                    Move(new Vector3(0, transform.localPosition.y, transform.localPosition.z), Duration, Ease, finish);
                    break;
            }
        }

        /// <summary>
        /// 关闭UI动画
        /// </summary>
        /// <param name="finish">完成后回调</param>
        public void CloseUIAnimation(Action finish)
        {
            CloseUIAnimation(AnimationStyle, Duration, Ease, finish);
        }

        /// <summary>
        /// 关闭UI动画(方法重载)
        /// </summary>
        /// <param name="animationStyle">动画类型</param>
        /// <param name="Duration">持续时间</param>
        /// <param name="ease">动画曲线</param>
        /// <param name="finish">完成回调</param>
        public void CloseUIAnimation(EnumUIAnimationStyle animationStyle, float Duration, Ease ease, Action finish)
        {
            this.AnimationStyle = animationStyle;
            this.Duration = Duration;
            this.Ease = ease;

            switch (animationStyle)
            {
                case EnumUIAnimationStyle.CenterScaleBigNomal:

                    Scale(Vector3.zero, Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.TopToSlide:

                    Move(new Vector3(transform.localPosition.x, -1000, transform.localPosition.z), Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.DoweToSlide:

                    Move(new Vector3(transform.localPosition.x, 1000, transform.localPosition.z), Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.LeftToSlide:

                    Move(new Vector3(-1000, transform.localPosition.y, transform.localPosition.z), Duration, Ease, finish);
                    break;

                case EnumUIAnimationStyle.RightToSlide:

                    Move(new Vector3(1000, transform.localPosition.y, transform.localPosition.z), Duration, Ease, finish);
                    break;
            }
        }

        /// <summary>
        /// 缩放动画
        /// </summary>
        /// <param name="endValue">结束值</param>
        /// <param name="duration">持续时间</param>
        /// <param name="ease">动画曲线</param>
        /// <param name="finish">完成后回调</param>
        public void Scale(Vector3 endValue, float duration, Ease ease, Action finish)
        {
            m_Tween = transform.DOScale(endValue, duration).SetEase(ease).OnComplete(() => { if (finish != null) finish(); });
            m_Tween.SetAutoKill(false);
        }

        /// <summary>
        /// 移动动画
        /// </summary>
        /// <param name="endValue">结束值</param>
        /// <param name="duration">持续时间</param>
        /// <param name="ease">动画曲线</param>
        /// <param name="finish">完成后回调</param>
        public void Move(Vector3 endValue, float duration, Ease ease, Action finish)
        {
            m_Tween = transform.DOLocalMove(endValue, duration).SetEase(ease).OnComplete(() => { if (finish != null) finish(); });
            m_Tween.SetAutoKill(false);
        }

        #endregion
    }
}