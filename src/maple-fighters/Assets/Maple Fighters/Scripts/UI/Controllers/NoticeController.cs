﻿using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class NoticeController : MonoBehaviour
    {
        private INoticeView noticeView;
        private Action onOkButtonClicked;

        private void CreateAndSubscribeToNoticeWindow()
        {
            noticeView = UIElementsCreator.GetInstance()
                .Create<NoticeWindow>(UILayer.Foreground, UIIndex.End);
            noticeView.OkButtonClicked += Hide;
        }

        private void UnsubscribeFromNoticeWindow()
        {
            if (noticeView != null)
            {
                noticeView.OkButtonClicked += Hide;
            }
        }

        public void Show(string message, Action onClicked = null, bool background = true)
        {
            var noticeWindow = CreateOrShowNoticeView();
            if (noticeWindow != null)
            {
                noticeWindow.Message = message;

                if (onClicked != null)
                {
                    onOkButtonClicked = onClicked;

                    noticeWindow.OkButtonClicked += onOkButtonClicked;
                }
                else
                {
                    onOkButtonClicked = null;
                }

                if (background == false)
                {
                    noticeWindow.HideBackground();
                }
            }
        }

        private void Hide()
        {
            SubscribeToUIFadeAnimation();

            HideNoticeWindow();
        }

        private void OnDestroy()
        {
            UnsubscribeFromNoticeWindow();
        }

        private void HideNoticeWindow()
        {
            if (noticeView != null)
            {
                if (onOkButtonClicked != null)
                {
                    noticeView.OkButtonClicked -= onOkButtonClicked;
                }

                noticeView.Hide();
            }
        }

        private void SubscribeToUIFadeAnimation()
        {
            if (noticeView != null)
            {
                noticeView.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            if (noticeView != null)
            {
                noticeView.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void OnFadeOutCompleted()
        {
            UnsubscribeFromUIFadeAnimation();
        }

        private INoticeView CreateOrShowNoticeView()
        {
            if (noticeView == null)
            {
                CreateAndSubscribeToNoticeWindow();
            }

            noticeView.Show();

            return noticeView;
        }
    }
}