﻿// ==========================================================================
// NodeMovingBehavior.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GP.Utils.UI;
using GP.Utils.UI.Interactivity;

// ReSharper disable InvertIf
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable LoopCanBePartlyConvertedToQuery

namespace Hercules.App.Controls
{
    public sealed class NodeMovingBehavior : Behavior<FrameworkElement>
    {
        private ScrollViewer scrollViewer;
        private NodeMovingOperation movingOperation;

        protected override void OnAttached()
        {
            AssociatedElement.PointerPressed += AssociatedElement_PointerPressed;
            AssociatedElement.PointerReleased += AssociatedElement_PointerReleased;
            AssociatedElement.ManipulationStarted += AssociatedElement_ManipulationStarted;
            AssociatedElement.ManipulationDelta += AssociatedElement_ManipulationDelta;
            AssociatedElement.ManipulationCompleted += AssociatedElement_ManipulationCompleted;
        }

        protected override void OnDetaching()
        {
            AssociatedElement.PointerPressed -= AssociatedElement_PointerPressed;
            AssociatedElement.PointerReleased -= AssociatedElement_PointerReleased;
            AssociatedElement.ManipulationStarted -= AssociatedElement_ManipulationStarted;
            AssociatedElement.ManipulationDelta -= AssociatedElement_ManipulationDelta;
            AssociatedElement.ManipulationCompleted -= AssociatedElement_ManipulationCompleted;
        }

        private void AssociatedElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (AcquireOperation(e.GetCurrentPoint(AssociatedElement).Position.ToVector2()) != null)
            {
                AssociatedElement.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            }
        }

        private void AssociatedElement_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            AssociatedElement.ManipulationMode = ManipulationModes.System;
        }

        private void AssociatedElement_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            scrollViewer = AssociatedElement.FindParent<ScrollViewer>();

            if (movingOperation != null)
            {
                movingOperation.Cancel();
                movingOperation = null;
            }

            if (scrollViewer != null)
            {
                movingOperation = AcquireOperation(e.Position.ToVector2());
            }
        }

        private void AssociatedElement_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (movingOperation != null && scrollViewer != null)
            {
                var translation = e.Delta.Translation.ToVector2() / scrollViewer.ZoomFactor;

                movingOperation.Move(translation);
            }
        }

        private void AssociatedElement_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            AssociatedElement.ManipulationMode = ManipulationModes.None;

            if (movingOperation != null)
            {
                movingOperation.Complete();
                movingOperation = null;
            }

            scrollViewer = null;
        }

        private NodeMovingOperation AcquireOperation(Vector2 position)
        {
            var mindmap = AssociatedElement.FindParent<Mindmap>();

            if (mindmap?.Renderer == null)
            {
                return null;
            }

            var result = mindmap.Renderer.HitTest(position);

            if (result != null && result.RenderNode != mindmap.TextEditingNode)
            {
                return NodeMovingOperation.Start(mindmap, result.RenderNode);
            }

            return null;
        }
    }
}
