
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Nuntium
{
    public class ResizingAdorner : Adorner
    {
        //use thumb for resizing elements
        Thumb mTopLeft, mTopRight, mBottomLeft, mBottomRight, mTop, mBottom, mRight, mLeft;
        //visual child collection for adorner
        VisualCollection mVisualChildren;

        public ResizingAdorner(UIElement element) : base(element)
        {
            mVisualChildren = new VisualCollection(this);

            //adding thumbs for drawing adorner rectangle and setting cursor
            BuildAdornerCorners(ref mTopLeft, Cursors.SizeNWSE);
            BuildAdornerCorners(ref mTopRight, Cursors.SizeNESW);
            BuildAdornerCorners(ref mBottomLeft, Cursors.SizeNESW);
            BuildAdornerCorners(ref mBottomRight, Cursors.SizeNWSE);

            BuildAdornerCorners(ref mLeft, Cursors.SizeWE);
            BuildAdornerCorners(ref mTop, Cursors.SizeNS);
            BuildAdornerCorners(ref mRight, Cursors.SizeWE);
            BuildAdornerCorners(ref mBottom, Cursors.SizeNS);

            //registering drag delta events for thumb drag movement
            mTopLeft.DragDelta += HandleTopLeft;
            mTopRight.DragDelta += HandleTopRight;
            mBottomLeft.DragDelta += HandleBottomLeft;
            mBottomRight.DragDelta += HandleBottomRight;

            mLeft.DragDelta += Left_DragDelta;
            mTop.DragDelta += TopBottom_DragDelta;
            mBottom.DragDelta += TopBottom_DragDelta;
            mRight.DragDelta += Right_DragDelta;

        }

        private void TopBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb thumb = sender as Thumb;

            //setting new height, width and canvas left after drag
            if (adornedElement != null && thumb != null)
            {
                double newHeight = adornedElement.Height + e.VerticalChange;

                if (newHeight < 0)
                    return;

                EnforceSize(adornedElement);

                adornedElement.Height = newHeight;
            }
        }

        private void Right_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width + e.HorizontalChange, hitThumb.DesiredSize.Width);
        }

        private void Left_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;

            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width - e.HorizontalChange, hitThumb.DesiredSize.Width);
        }

        // Handler for resizing from the bottom-right.
        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;
            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        }

        // Handler for resizing from the top-right.
        void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;
            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            //adornedElement.Height = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);

            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
        }

        // Handler for resizing from the top-left.
        void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            //adornedElement.Width = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            //adornedElement.Height = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;
            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));

            double height_old = adornedElement.Height;
            double height_new = Math.Max(adornedElement.Height - args.VerticalChange, hitThumb.DesiredSize.Height);
            double top_old = Canvas.GetTop(adornedElement);
            adornedElement.Height = height_new;
            Canvas.SetTop(adornedElement, top_old - (height_new - height_old));
        }

        // Handler for resizing from the bottom-left.
        void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            //adornedElement.Width = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);

            double width_old = adornedElement.Width;
            double width_new = Math.Max(adornedElement.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            double left_old = Canvas.GetLeft(adornedElement);
            adornedElement.Width = width_new;
            Canvas.SetLeft(adornedElement, left_old - (width_new - width_old));
        }

        private void BuildAdornerCorners(ref Thumb cornerThumb, Cursor customizedCursors)
        {
            //adding new thumbs for adorner to visual childern collection
            if (cornerThumb != null) return;
            cornerThumb = new Thumb();
            cornerThumb.Style = cornerThumb.FindResource("RoundThumbStyle") as Style;
            mVisualChildren.Add(cornerThumb);
        }

        public void EnforceSize(FrameworkElement element)
        {
            if (element.Width.Equals(Double.NaN))
                element.Width = element.DesiredSize.Width;
            if (element.Height.Equals(Double.NaN))
                element.Height = element.DesiredSize.Height;

            if (element is Image image)
                image.Stretch = Stretch.Fill;

            //enforce size of element not exceeding to it's parent element size
            FrameworkElement parent = element.Parent as FrameworkElement;

            if (parent != null)
            {
                element.MaxHeight = parent.ActualHeight;
                element.MaxWidth = parent.ActualWidth;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);

            //double desireWidth = AdornedElement.DesiredSize.Width;
            //double desireHeight = AdornedElement.DesiredSize.Height;

            //double adornerWidth = this.DesiredSize.Width;
            //double adornerHeight = this.DesiredSize.Height;

            double desireWidth = (AdornedElement as FrameworkElement).ActualWidth;
            double desireHeight = (AdornedElement as FrameworkElement).ActualHeight;

            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            //arranging thumbs
            mTopLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            mTopRight.Arrange(new Rect(desireWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            mBottomLeft.Arrange(new Rect(-adornerWidth / 2, desireHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            mBottomRight.Arrange(new Rect(desireWidth - adornerWidth / 2, desireHeight - adornerHeight / 2, adornerWidth, adornerHeight));

            //TODO: add this
            mLeft.Arrange(new Rect(-adornerWidth / 2, desireHeight - adornerHeight, adornerWidth, adornerHeight));
            mTop.Arrange(new Rect(desireWidth - adornerWidth, -adornerHeight / 2, adornerWidth, adornerHeight));
            mRight.Arrange(new Rect(desireWidth - adornerWidth / 2, desireHeight - adornerHeight, adornerWidth, adornerHeight));
            mBottom.Arrange(new Rect(desireWidth - adornerWidth, desireHeight - adornerHeight / 2, adornerWidth, adornerHeight));

            return finalSize;
        }
        protected override int VisualChildrenCount { get { return mVisualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return mVisualChildren[index]; }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

    }

}
