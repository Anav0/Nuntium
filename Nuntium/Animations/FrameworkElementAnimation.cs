using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Nuntium
{
    public static class FrameworkElementAnimation
    {
        public static async Task AnimateIn(this FrameworkElement element, AnimationDirection direction, Duration duration, bool fadeIn = false, double deacceleration = 0.5f)
        {
            var sb = new Storyboard();

            //Show element before animation starts
            element.Visibility = Visibility.Visible;

            switch (direction)
            {
                case AnimationDirection.Left:
                    sb.AddFromLeftAnimation(duration, element, deacceleration);
                    break;
                case AnimationDirection.Right:
                    sb.AddFromRightAnimation(duration, element, deacceleration);
                    break;
                case AnimationDirection.Top:
                    sb.AddFromTopAnimation(duration, element, deacceleration);
                    break;
                case AnimationDirection.Bottom:
                    sb.AddFromBottomAnimation(duration, element, deacceleration);
                    break;
                case AnimationDirection.Fade:
                    sb.AddFadeInAnimation(duration, element, deacceleration);
                    break;
            }

            if (fadeIn && direction != AnimationDirection.Fade)
                sb.AddFadeInAnimation(duration, element, deacceleration);

            sb.Begin(element);

            await Task.Delay(duration.TimeSpan);
        }

        public static async Task AnimateOut(this FrameworkElement element, AnimationDirection direction, Duration duration, bool fadeOut = false, double acceleration = 0.5f)
        {
            var sb = new Storyboard();

            switch (direction)
            {
                case AnimationDirection.Left:
                    sb.AddToLeftAnimation(duration, element, acceleration);
                    break;
                case AnimationDirection.Right:
                    sb.AddToRightAnimation(duration, element, acceleration);
                    break;
                case AnimationDirection.Top:
                    sb.AddToTopAnimation(duration, element, acceleration);
                    break;
                case AnimationDirection.Bottom:
                    sb.AddToBottomAnimation(duration, element, acceleration);
                    break;
                case AnimationDirection.Fade:
                    sb.AddFadeOutAnimation(duration, element, acceleration);
                    break;
            }

            if (fadeOut && direction != AnimationDirection.Fade)
                sb.AddFadeOutAnimation(duration, element, acceleration);

            sb.Begin(element);

            await Task.Delay(duration.TimeSpan);

            //Collapse element after animation is finished
            element.Visibility = Visibility.Collapsed;
        }

    }
}
