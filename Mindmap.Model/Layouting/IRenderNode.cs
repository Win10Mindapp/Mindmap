﻿// ==========================================================================
// IRenderNode.cs
// Mindmap Application
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using Windows.Foundation;

namespace MindmapApp.Model.Layouting
{
    public interface IRenderNode
    {
        Point Position { get; }

        Rect Bounds { get; }

        Size Size { get; }

        void Hide();

        void Show();

        void MoveTo(Point position, AnchorPoint anchor);
    }
}