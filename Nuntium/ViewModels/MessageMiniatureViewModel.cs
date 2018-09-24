﻿using Nuntium.Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace Nuntium
{
    public class MessageMiniatureViewModel : BaseViewModel
    {
        #region Public properties

        public string Initials { get; set; }

        public string AvatarBackground { get; set; }

        public string ImagePath { get; set; }

        public string SenderName { get; set; }

        public string Title { get; set; }

        public DateTime SendDate { get; set; }

        public bool HasAttachments { get; set; }

        public bool WasRead { get; set; }

        public bool IsStared { get; set; }

        public bool AnimateOut { get; set; }

        public InboxCategoryType Placement { get; set; }

        public InboxCategoryType PrevPlacement { get; set; }


        public string MessageSnipit { get; set; }

        public TimeSpan AnimateOutTimeSpan { get; set; } = new TimeSpan(0, 0, 0, 0, 500);

        #endregion

        public MessageMiniatureViewModel()
        {
            DeleteCommand = new RelayCommandWithParameter((parameter) =>  Delete(parameter));
        }

        #region EventHandlers

        public event EventHandler OnItemDeleted;

        #endregion

        #region Protected Members

        protected virtual void RaiseOnItemDeletedEvent()
        {
            OnItemDeleted?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Public Commands

        public ICommand DeleteCommand { get; set; }

        #endregion

        #region Command Methods

        private async void Delete(object param)
        {
            if (!(param is FrameworkElement element))
                return;

            await FrameworkElementAnimation.AnimateOut(element, AnimationDirection.Left, new Duration(AnimateOutTimeSpan), true, 0.4);

            RaiseOnItemDeletedEvent();
        }

        #endregion
    }
}
