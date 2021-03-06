﻿// ==========================================================================
// Win2DTextRenderer.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Text;
using GP.Utils.Mathematics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

// ReSharper disable InvertIf

namespace Hercules.Win2D.Rendering
{
    public sealed class Win2DTextRenderer : IResourceHolder
    {
        private const float PaddingY = 2;
        private CanvasTextFormat textFormat;
        private Vector2 renderSize;
        private Vector2 renderPosition;
        private string lastText;
        private bool isFirstMeasure = true;
        private float fontSize = 14;

        public Rect2 RenderBounds
        {
            get { return new Rect2(renderPosition, renderSize); }
        }

        public Vector2 RenderPosition
        {
            get { return renderPosition; }
        }

        public Vector2 RenderSize
        {
            get { return renderSize; }
        }

        public float FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                if (Math.Abs(fontSize - value) > float.Epsilon)
                {
                    fontSize = value;

                    textFormat = null;
                }
            }
        }

        private CanvasTextFormat TextFormat
        {
            get
            {
                return textFormat ?? (textFormat = new CanvasTextFormat
                {
                    WordWrapping = CanvasWordWrapping.NoWrap,
                    HorizontalAlignment = CanvasHorizontalAlignment.Left,
                    FontSize = fontSize,
                    FontWeight = FontWeights.Normal,
                    VerticalAlignment = CanvasVerticalAlignment.Center
                });
            }
        }

        public void ClearResources()
        {
            textFormat?.Dispose();
            textFormat = null;
        }

        public void Measure(Win2DRenderable renderable, ICanvasResourceCreator resourceCreator)
        {
            var text = renderable.Node.Text;

            if (isFirstMeasure || !string.Equals(text, lastText, StringComparison.CurrentCulture))
            {
                isFirstMeasure = true;

                lastText = text;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    using (var textLayout = new CanvasTextLayout(resourceCreator, text, TextFormat, 0.0f, 0.0f))
                    {
                        renderSize = new Vector2(
                            (float)textLayout.DrawBounds.Width,
                            (float)textLayout.DrawBounds.Height);
                    }
                }
                else
                {
                    renderSize = Vector2.Zero;
                }

                renderSize.Y = Math.Max(renderSize.Y, TextFormat.FontSize * 1.5f) + PaddingY;
                renderSize.X = Math.Max(renderSize.X, TextFormat.FontSize * 2);

                renderSize = MathHelper.RoundToMultipleOfTwo(renderSize);
            }
        }

        public void Arrange(Vector2 position)
        {
            renderPosition = position;
        }

        public void Render(Win2DRenderable renderable, CanvasDrawingSession session)
        {
            var text = renderable.Node.Text;

            var rect = RenderBounds.ToRect();
#if DRAW_OUTLINE
            session.DrawRectangle(rect, Colors.Red);
#endif
            if (!string.IsNullOrWhiteSpace(text))
            {
                session.DrawText(text, rect, Colors.Black, TextFormat);
            }
        }
    }
}
