﻿// ==========================================================================
// FilledPath.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using Windows.UI;
using Hercules.Win2D.Rendering.Utils;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;

namespace Hercules.Win2D.Rendering.Parts.Paths
{
    public class FilledPath : GeometryPathBase
    {
        public FilledPath(Color strokeColor, float opacity = 1)
            : base(strokeColor, opacity)
        {
        }

        protected override CanvasGeometry CreateGeometry(Win2DRenderNode renderNode, ICanvasResourceCreator resourceCreator)
        {
            return GeometryBuilder.ComputeFilledPath(renderNode, renderNode.Parent, resourceCreator);
        }

        protected override void RenderInternal(Win2DRenderable renderable, CanvasDrawingSession session, CanvasGeometry geometry, ICanvasBrush brush)
        {
            session.FillGeometry(geometry, brush);
        }
    }
}