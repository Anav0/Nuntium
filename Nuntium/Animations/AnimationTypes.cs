using System.Windows;
using System.Windows.Media.Animation;

namespace Nuntium
{
    public static class AnimationTypes
    {

        public static void AddFromLeftAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double deacceleration)
        {

            var animation = new ThicknessAnimation
            {
                DecelerationRatio = deacceleration,
                Duration = duration,
                From = new Thickness(-element.ActualWidth, 0, element.ActualWidth, 0),
                To = new Thickness(0),
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddToLeftAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {

            var animation = new ThicknessAnimation
            {
                AccelerationRatio = acceleration,
                Duration = duration,
                From = new Thickness(0),
                To = new Thickness(-element.ActualWidth, 0, element.ActualWidth, 0)
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddFromRightAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double deacceleration)
        {

            var animation = new ThicknessAnimation
            {
                DecelerationRatio = deacceleration,
                Duration = duration,
                From = new Thickness(-element.ActualWidth, 0, element.ActualWidth, 0),
                To = new Thickness(0)
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddToRightAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {

            var animation = new ThicknessAnimation
            {
                AccelerationRatio = acceleration,
                Duration = duration,
                From = new Thickness(0),
                To = new Thickness(element.ActualWidth, 0, -element.ActualWidth, 0)
            };


            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddToTopAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {

            var animation = new ThicknessAnimation
            {
                AccelerationRatio = acceleration,
                Duration = duration,
                From = new Thickness(0),
                To = new Thickness(0, -element.ActualHeight, 0, element.ActualHeight)
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddFromTopAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {

            var animation = new ThicknessAnimation
            {
                AccelerationRatio = acceleration,
                Duration = duration,
                From = new Thickness(0, -element.ActualHeight, 0, element.ActualHeight),
                To = new Thickness(0)
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddToBottomAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {

            var animation = new ThicknessAnimation
            {
                AccelerationRatio = acceleration,
                Duration = duration,
                From = new Thickness(0),
                To = new Thickness(0, element.ActualHeight, 0, -element.ActualHeight)
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddFromBottomAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {

            var animation = new ThicknessAnimation
            {
                AccelerationRatio = acceleration,
                Duration = duration,
                From = new Thickness(0, element.ActualHeight, 0, -element.ActualHeight),
                To = new Thickness(0)
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            sb.Children.Add(animation);
        }

        public static void AddFadeInAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {
            var animation = new DoubleAnimation
            {
                Duration = duration,
                To = 1,
                From = 0
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            sb.Children.Add(animation);
        }

        public static void AddFadeOutAnimation(this Storyboard sb, Duration duration, FrameworkElement element, double acceleration)
        {
            var animation = new DoubleAnimation
            {
                Duration = duration,
                To = 0,
                From = 1
            };

            
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            sb.Children.Add(animation);
        }
    }
}
