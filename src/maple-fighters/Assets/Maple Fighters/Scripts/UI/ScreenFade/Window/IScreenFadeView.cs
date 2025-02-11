﻿using System;
using UI.Manager;

namespace Scripts.UI.ScreenFade
{
    public interface IScreenFadeView : IView
    {
        event Action FadeInCompleted;

        event Action FadeOutCompleted;
    }
}