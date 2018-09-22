using System;
using System.Threading.Tasks;
using System.Windows;

namespace Nuntium
{
    public static class AttachedFrameworkElementAnimations
    {

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
                await element.AnimateIn(AnimationDirection.Left, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element));
            else await element.AnimateOut(AnimationDirection.Right, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element));
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
                await element.AnimateIn(AnimationDirection.Right, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element));
            else await element.AnimateOut(AnimationDirection.Left, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element));
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
                await element.AnimateIn(AnimationDirection.Top, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element));
            else await element.AnimateOut(AnimationDirection.Bottom, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element));
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
                await element.AnimateIn(AnimationDirection.Fade, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element), 0.5);
            else await element.AnimateOut(AnimationDirection.Fade, GetAnimationTimeSpan(element), GetAddFadingToAnimation(element), 0.5);
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

        #region AddFadingToAnimation

        public static readonly DependencyProperty AddFadingToAnimationProperty = DependencyProperty.RegisterAttached(
        "AddFadingToAnimation",
        typeof(bool),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(false)
        );

        public static void SetAddFadingToAnimation(FrameworkElement element, bool value)
        {
            element.SetValue(AddFadingToAnimationProperty, value);
        }

        public static bool GetAddFadingToAnimation(FrameworkElement element)
        {
            return (bool)element.GetValue(AddFadingToAnimationProperty);
        }

        #endregion
    }
}
