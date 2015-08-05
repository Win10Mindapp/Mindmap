﻿using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

namespace Hercules.Model.Rendering.Win2D.Default
{
    public sealed class DefaultRootNode : DefaultRenderNode
    {
        private static readonly Vector2 ContentPadding = new Vector2(15, 5);
        private static readonly Vector2 SelectionMargin = new Vector2(-5, -5);
        private static readonly float MinHeight = 50;
        private static readonly float MinWidth = 100;
        private readonly Win2DTextRenderer textRenderer;

        public override Win2DTextRenderer TextRenderer
        {
            get
            {
                return textRenderer;
            }
        }

        public DefaultRootNode(NodeBase node, DefaultRenderer renderer) 
            : base(node, renderer)
        {
            textRenderer = new Win2DTextRenderer(16, node);
        }

        protected override void ArrangeInternal(CanvasDrawingSession session)
        {
            Vector2 textPosition = Bounds.Center;

            textPosition.X -= textRenderer.Size.X * 0.5f;
            textPosition.Y -= textRenderer.Size.Y * 0.5f;

            textRenderer.Arrange(textPosition);

            base.ArrangeInternal(session);
        }

        protected override Vector2 MeasureInternal(CanvasDrawingSession session)
        {
            textRenderer.Measure(session);

            Vector2 size = textRenderer.Size + 2 * ContentPadding;

            size.X = Math.Max(size.X, MinWidth);
            size.Y = Math.Max(size.Y, MinHeight);

            return size;
        }

        protected override void RenderInternal(CanvasDrawingSession session, ThemeColor color)
        {
            ICanvasBrush borderBrush = color.DarkBrush(session);

            ICanvasBrush backgroundBrush =
                Node.IsSelected ?
                    color.LightBrush(session) :
                    color.NormalBrush(session);

            float radiusX = 0.5f * Size.X;
            float radiusY = 0.5f * Size.Y;

            session.FillEllipse(
                Bounds.Center,
                radiusX,
                radiusY,
                backgroundBrush);

            session.DrawEllipse(
                Bounds.Center,
                radiusX,
                radiusY,
                borderBrush);
            
            textRenderer.Render(session);

            if (!HideControls)
            {
                if (Node.IsSelected)
                {
                    radiusX -= SelectionMargin.X;
                    radiusY -= SelectionMargin.Y;

                    session.DrawEllipse(
                        Bounds.Center,
                        radiusX,
                        radiusY,
                        borderBrush, 2f, SelectionStrokeStyle);
                }

                Button.Render(session);
            }
        }

        protected override Win2DRenderNode CloneInternal()
        {
            return new DefaultRootNode(Node, (DefaultRenderer)Renderer);
        }
    }
}
