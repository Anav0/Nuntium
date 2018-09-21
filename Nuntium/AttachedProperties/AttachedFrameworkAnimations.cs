using System;
using System.Threading.Tasks;
using System.Windows;

namespace Nuntium
{
    public static class AttachedFrameworkElementAnimations
    {

        private static readonly Duration mDefaultAnimDuration = new Duration(new TimeSpan(0, 0, 0, 0, 400));

        #region AnimationTimeSpan

        public static readonly DependencyProperty AnimationTimeSpanProperty = DependencyProperty.RegisterAttached(
        "AnimationTimeSpan",
        typeof(TimeSpan),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(new TimeSpan(0, 0, 0, 0, 400))
        );

        public static void SetAnimationTimeSpan(FrameworkElement element, TimeSpan value)
        {
            element.SetValue(AnimationTimeSpanProperty, value);
        }

        public static TimeSpan GetAnimationTimeSpan(FrameworkElement element)
        {
            return (TimeSpan)element.GetValue(AnimationTimeSpanProperty);
        }

        #endregion



        #region AnimateFromLeftToRight

        public static readonly DependencyProperty AnimateFromLeftToRightProperty = DependencyProperty.RegisterAttached(
        "AnimateFromLeftToRight",
        typeof(bool),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(false, new PropertyChangedCallback(AnimateFromLeftToRightChanged))
        );

        public static void SetAnimateFromLeftToRight(FrameworkElement element, bool value)
        {
            element.SetValue(AnimateFromLeftToRightProperty, value);
        }

        public static bool GetAnimateFromLeftToRight(FrameworkElement element)
        {
            return (bool)element.GetValue(AnimateFromLeftToRightProperty);
        }

        private async static void AnimateFromLeftToRightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                return;

            if (e.OldValue == e.NewValue)
                return;

            await Task.Delay(5);

            if ((bool)e.NewValue)
                await element.AnimateIn(AnimationDirection.Left, mDefaultAnimDuration);
            else await element.AnimateOut(AnimationDirection.Right, mDefaultAnimDuration);
        }

        #endregion

        #region AnimateFromRightToLeft

        public static readonly DependencyProperty AnimateFromRightToLeftProperty = DependencyProperty.RegisterAttached(
        "AnimateFromRightToLeft",
        typeof(bool),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(false, new PropertyChangedCallback(AnimateFromRightToLeftPropertyChanged))
        );

        private static async void AnimateFromRightToLeftPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                return;

            if (e.OldValue == e.NewValue)
                return;

            await Task.Delay(5);

            if ((bool)e.NewValue == true)
                await element.AnimateIn(AnimationDirection.Right, mDefaultAnimDuration);
            else await element.AnimateOut(AnimationDirection.Left, mDefaultAnimDuration);
        }

        public static void SetAnimateFromRightToLeft(FrameworkElement element, bool value)
        {
            element.SetValue(AnimateFromLeftToRightProperty, value);
        }

        public static bool GetAnimateFromRightToLeft(FrameworkElement element)
        {
            return (bool)element.GetValue(AnimateFromLeftToRightProperty);
        }

        #endregion

        #region AnimateFromTopToBottom

        public static readonly DependencyProperty AnimateFromTopToBottomProperty = DependencyProperty.RegisterAttached(
        "AnimateFromTopToBottom",
        typeof(bool),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(false, new PropertyChangedCallback(AnimateFromTopToBottomPropertyChanged))
        );

        private static async void AnimateFromTopToBottomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                return;

            if (e.OldValue == e.NewValue)
                return;

            await Task.Delay(10);

            if ((bool)e.NewValue == true)
                await element.AnimateIn(AnimationDirection.Top, mDefaultAnimDuration);
            else await element.AnimateOut(AnimationDirection.Bottom, mDefaultAnimDuration);
        }

        public static void SetAnimateFromTopToBottom(FrameworkElement element, bool value)
        {
            element.SetValue(AnimateFromTopToBottomProperty, value);
        }

        public static bool GetAnimateFromTopToBottom(FrameworkElement element)
        {
            return (bool)element.GetValue(AnimateFromTopToBottomProperty);
        }

        #endregion

        #region Fading

        public static readonly DependencyProperty FadingProperty = DependencyProperty.RegisterAttached(
        "Fading",
        typeof(bool),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(false, new PropertyChangedCallback(FadingPropertyChanged))
        );

        private static async void FadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                return;

            if (e.OldValue == e.NewValue)
                return;

            await Task.Delay(10);

            if ((bool)e.NewValue == true)
                await element.AnimateIn(AnimationDirection.Fade, GetAnimationTimeSpan(element));
            else await element.AnimateOut(AnimationDirection.Fade, GetAnimationTimeSpan(element));
        }

        public static void SetFading(FrameworkElement element, bool value)
        {
            element.SetValue(AnimateFromTopToBottomProperty, value);
        }

        public static bool GetFading(FrameworkElement element)
        {
            return (bool)element.GetValue(FadingProperty);
        }

        #endregion

    }
}
