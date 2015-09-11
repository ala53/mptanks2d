using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Toolkit
{
    public class AdornerCanvas : Canvas
    {
        AdornerLayer aLayer;

        bool _isDown;
        bool _isDragging;
        bool selected = false;
        UIElement selectedElement = null;

        Point _startPoint;
        private double _originalLeft;
        private double _originalTop;

        public override void EndInit()
        {
            base.EndInit();
            PreviewMouseLeftButtonDown += AdornerGrid_PreviewMouseLeftButtonDown;
            PreviewMouseLeftButtonUp += AdornerGrid_PreviewMouseLeftButtonUp;
            PreviewMouseMove += AdornerGrid_PreviewMouseMove;
            MouseLeave += AdornerGrid_MouseLeave;
        }
        // Method for stopping dragging
        private void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;
                _isDragging = false;
            }
        }

        private void AdornerGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            StopDragging();
        }

        private void AdornerGrid_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(this).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(this).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    _isDragging = true;

                if (_isDragging)
                {
                    Point position = Mouse.GetPosition(this);
                    SetTop(selectedElement, position.Y - (_startPoint.Y - _originalTop));
                    SetLeft(selectedElement, position.X - (_startPoint.X - _originalLeft));
                }
            }
        }

        private void AdornerGrid_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        private void AdornerGrid_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Remove selection on clicking anywhere the window
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    // Remove the adorner from the selected element
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }

            // If any element except canvas is clicked, 
            // assign the selected element and add the adorner
            if (e.Source != this)
            {
                _isDown = true;
                _startPoint = e.GetPosition(this);

                selectedElement = e.Source as UIElement;

                _originalLeft = Canvas.GetLeft(selectedElement);
                _originalTop = Canvas.GetTop(selectedElement);

                aLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                aLayer.Add(new ResizingAdorner(selectedElement));
                selected = true;
                e.Handled = true;
            }
        }
    }
}
