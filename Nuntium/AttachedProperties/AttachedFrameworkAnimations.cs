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

        #region AnimationAccelerationRate

        public static readonly DependencyProperty AnimationAccelerationRateProperty = DependencyProperty.RegisterAttached(
        "AnimationAccelerationRate",
        typeof(double),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(0.5)
        );

        public static void SetAnimationAccelerationRate(FrameworkElement element, double value)
        {
            element.SetValue(AnimationAccelerationRateProperty, value);
        }

        public static double GetAnimationAccelerationRate(FrameworkElement element)
        {
            return (double)element.GetValue(AnimationAccelerationRateProperty);
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

        #region AnimateInDirection

        public static readonly DependencyProperty AnimateInDirectionProperty = DependencyProperty.RegisterAttached(
        "AnimateInDirection",
        typeof(AnimationDirection),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(AnimationDirection.Fade)
        );

        public static void SetAnimateInDirection(FrameworkElement element, AnimationDirection value)
        {
            element.SetValue(AnimateInDirectionProperty, value);
        }

        public static AnimationDirection GetAnimateInDirection(FrameworkElement element)
        {
            return (AnimationDirection)element.GetValue(AnimateInDirectionProperty);
        }

        #endregion

        #region AnimateOutDirection

        public static readonly DependencyProperty AnimateOutDirectionProperty = DependencyProperty.RegisterAttached(
        "AnimateOutDirection",
        typeof(AnimationDirection),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(AnimationDirection.Fade)
        );

        public static void SetAnimateOutDirection(FrameworkElement element, AnimationDirection value)
        {
            element.SetValue(AnimateOutDirectionProperty, value);
        }

        public static AnimationDirection GetAnimateOutDirection(FrameworkElement element)
        {
            return (AnimationDirection)element.GetValue(AnimateOutDirectionProperty);
        }

        #endregion

        #region AnimationCondition

        public static readonly DependencyProperty AnimationConditionProperty = DependencyProperty.RegisterAttached(
        "AnimationCondition",
        typeof(bool),
        typeof(AttachedFrameworkElementAnimations),
        new FrameworkPropertyMetadata(false, new PropertyChangedCallback(AnimationConditionChanged))
        );

        private static async void AnimationConditionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                return;

            if (e.OldValue == e.NewValue)
                return;

            await Task.Delay(10);

            if ((bool)e.NewValue == true)
                await element.AnimateIn(GetAnimateInDirection(element), GetAnimationTimeSpan(element), GetAddFadingToAnimation(element), GetAnimationAccelerationRate(element));
            else await element.AnimateOut(GetAnimateOutDirection(element), GetAnimationTimeSpan(element), GetAddFadingToAnimation(element), GetAnimationAccelerationRate(element));
        }

        public static void SetAnimationCondition(FrameworkElement element, bool value)
        {
            element.SetValue(AnimationConditionProperty, value);
        }

        public static bool GetAnimationCondition(FrameworkElement element)
        {
            return (bool)element.GetValue(AnimationConditionProperty);
        }

        #endregion

    }
}
