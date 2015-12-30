﻿// ==========================================================================
// Ellipse.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace Hercules.Win2D.Rendering.Geometries.Bodies
{
    public sealed class Ellipse : BodyBase
    {
        private static readonly Vector2 SelectionMargin = new Vector2(-5, -5);

        protected override Vector2 CalculatePadding(Win2DRenderable renderable, Vector2 contentSize)
        {
            float sqrt2 = (float)Math.Sqrt(2);
            
            float a = (contentSize.X + 5) / sqrt2;
            float b = (contentSize.Y + 5) / sqrt2;

            return new Vector2(a - (contentSize.X * 0.5f), b - (contentSize.Y * 0.5f));
        }

        public override void Render(Win2DRenderable renderable, CanvasDrawingSession session, Win2DColor color, bool renderControls)
        {
            ICanvasBrush borderBrush = renderable.Resources.ThemeDarkBrush(color);

            ICanvasBrush backgroundBrush =
                renderable.Node.IsSelected ?
                    renderable.Resources.ThemeLightBrush(color) :
                    renderable.Resources.ThemeNormalBrush(color);

            float radiusX = 0.5f * renderable.RenderSize.X;
            float radiusY = 0.5f * renderable.RenderSize.Y;

            session.FillEllipse(
                renderable.RenderBounds.Center,
                radiusX,
                radiusY,
                backgroundBrush);

            session.DrawEllipse(
                renderable.RenderBounds.Center,
                radiusX,
                radiusY,
                borderBrush);

            RenderIcon(renderable, session);

            if (renderControls && renderable.Node.IsSelected)
            {
                radiusX -= SelectionMargin.X;
                radiusY -= SelectionMargin.Y;

                session.DrawEllipse(
                    renderable.RenderBounds.Center,
                    radiusX,
                    radiusY,
                    borderBrush, 2f, SelectionStrokeStyle);
            }
        }

        public override IBodyGeometry Clone()
        {
            return new Ellipse();
        }
    }
}
