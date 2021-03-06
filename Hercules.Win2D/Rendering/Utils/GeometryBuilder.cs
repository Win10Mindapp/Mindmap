﻿// ==========================================================================
// GeometryBuilder.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GP.Utils;
using GP.Utils.Mathematics;
using Hercules.Model;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace Hercules.Win2D.Rendering.Utils
{
    public static class GeometryBuilder
    {
        private const float Radius = 10;
        private const float Padding = 10;
        private const float StartHeightHalf = 20;

        public static CanvasGeometry ComputeHullGeometry(ICanvasResourceCreator resourceCreator, Win2DScene scene, Win2DRenderNode renderNode)
        {
            Guard.NotNull(scene, nameof(scene));
            Guard.NotNull(renderNode, nameof(renderNode));
            Guard.NotNull(resourceCreator, nameof(resourceCreator));

            var node = renderNode.Node;

            if (!node.HasChildren || node.IsCollapsed)
            {
                return null;
            }

            IList<Rect2> childBounds =
                node.AllChildren()
                    .Select(x => (Win2DRenderNode)scene.FindRenderNode(x)).Union(new[] { renderNode }).Where(x => x.IsVisible)
                    .Select(x => Rect2.Inflate(new Rect2(x.TargetLayoutPosition, x.RenderSize), new Vector2(Padding, Padding)))
                    .ToList();

            if (childBounds.Count <= 0)
            {
                return null;
            }

            var hull = ConvexHull.Compute(childBounds);

            var points = RoundCorners(hull);

            return BuildGeometry(resourceCreator, points);
        }

        private static List<Vector2> RoundCorners(ConvexHull hull)
        {
            var size = hull.Points.Count;

            var points = new List<Vector2>(size * 3);

            var e = size - 1;

            for (var i = 0; i < size; i++)
            {
                Vector2 c = hull.Points[i], next, prev, back, forw;

                prev = i > 0 ? hull.Points[i - 1] : hull.Points[e];
                next = i < e ? hull.Points[i + 1] : hull.Points[0];

                back = prev - c;
                forw = next - c;

                var lengthNext = Math.Min(Radius, back.Length() * 0.5f);
                var lengthForw = Math.Min(Radius, forw.Length() * 0.5f);

                points.Add(c + (Vector2.Normalize(back) * lengthNext));
                points.Add(c);
                points.Add(c + (Vector2.Normalize(forw) * lengthForw));
            }

            return points;
        }

        private static CanvasGeometry BuildGeometry(ICanvasResourceCreator resourceCreator, IReadOnlyList<Vector2> points)
        {
            using (var builder = new CanvasPathBuilder(resourceCreator.Device))
            {
                builder.BeginFigure(points[0]);

                for (var i = 0; i < points.Count / 3; i++)
                {
                    builder.AddQuadraticBezier(points[(i * 3) + 1], points[(i * 3) + 2]);

                    var lastIndex = (i * 3) + 3;

                    if (lastIndex < points.Count - 1)
                    {
                        builder.AddLine(points[lastIndex]);
                    }
                }

                builder.EndFigure(CanvasFigureLoop.Closed);

                return CanvasGeometry.CreatePath(builder);
            }
        }
        public static CanvasGeometry ComputeFilledPath(Win2DRenderNode target, Win2DRenderNode parent, ICanvasResourceCreator resourceCreator)
        {
            Guard.NotNull(target, nameof(target));
            Guard.NotNull(resourceCreator, nameof(resourceCreator));

            if (parent == null || !target.IsVisible)
            {
                return null;
            }

            var targetRect = target.RenderBounds;
            var parentRect = parent.RenderBounds;

            var targetOffset = target.VerticalPathRenderOffset;

            var point1 = Vector2.Zero;
            var point2 = Vector2.Zero;

            CalculateCenter(parentRect, ref point1);

            if (targetRect.CenterX > parentRect.CenterX)
            {
                CalculateCenterL(targetRect, ref point2, targetOffset);
            }
            else
            {
                CalculateCenterR(targetRect, ref point2, targetOffset);
            }

            MathHelper.Round(ref point1);
            MathHelper.Round(ref point2);

            return CreateFilledPath(resourceCreator, point1, point2);
        }

        private static CanvasGeometry CreateFilledPath(ICanvasResourceCreator session, Vector2 point1, Vector2 point2)
        {
            var halfX = (point1.X + point2.X) * 0.5f;

            using (var builder = new CanvasPathBuilder(session.Device))
            {
                builder.BeginFigure(new Vector2(point1.X, point1.Y - StartHeightHalf));

                builder.AddCubicBezier(
                    new Vector2(halfX, point1.Y - StartHeightHalf),
                    new Vector2(halfX, point2.Y),
                    new Vector2(point2.X, point2.Y));
                builder.AddLine(point2.X, point2.Y);
                builder.AddCubicBezier(
                    new Vector2(halfX, point2.Y),
                    new Vector2(halfX, point1.Y + StartHeightHalf),
                    new Vector2(point1.X, point1.Y + StartHeightHalf));

                builder.EndFigure(CanvasFigureLoop.Closed);

                return CanvasGeometry.CreatePath(builder);
            }
        }

        public static CanvasGeometry ComputeLinePath(Win2DRenderNode target, Win2DRenderNode parent, ICanvasResourceCreator resourceCreator, bool parentCentered = false)
        {
            Guard.NotNull(target, nameof(target));
            Guard.NotNull(resourceCreator, nameof(resourceCreator));

            if (parent == null || !target.IsVisible)
            {
                return null;
            }

            var targetRect = target.RenderBounds;
            var parentRect = parent.RenderBounds;

            var targetOffset = target.VerticalPathRenderOffset;
            var parentOffset = parent.VerticalPathRenderOffset;

            var point1 = Vector2.Zero;
            var point2 = Vector2.Zero;

            if (targetRect.CenterX > parentRect.CenterX)
            {
                if (parentCentered)
                {
                    CalculateCenter(parentRect, ref point1);
                }
                else
                {
                    CalculateCenterR(parentRect, ref point1, parentOffset);
                }

                CalculateCenterL(targetRect, ref point2, targetOffset);
            }
            else
            {
                if (parentCentered)
                {
                    CalculateCenter(parentRect, ref point1);
                }
                else
                {
                    CalculateCenterL(parentRect, ref point1, parentOffset);
                }

                CalculateCenterR(targetRect, ref point2, targetOffset);
            }

            MathHelper.Round(ref point1);
            MathHelper.Round(ref point2);

            return CreateLinePath(resourceCreator, point1, point2);
        }

        private static CanvasGeometry CreateLinePath(ICanvasResourceCreator resourceCreator, Vector2 point1, Vector2 point2)
        {
            var halfX = (point1.X + point2.X) * 0.5f;

            using (var builder = new CanvasPathBuilder(resourceCreator.Device))
            {
                builder.BeginFigure(point1);

                builder.AddCubicBezier(
                    new Vector2(halfX, point1.Y),
                    new Vector2(halfX, point2.Y),
                    point2);

                builder.EndFigure(CanvasFigureLoop.Open);

                return CanvasGeometry.CreatePath(builder);
            }
        }

        private static void CalculateCenterL(Rect2 rect, ref Vector2 point, float offset)
        {
            point.X = rect.X;
            point.Y = rect.Y + (rect.Height * 0.5f) + offset;
        }

        private static void CalculateCenterR(Rect2 rect, ref Vector2 point, float offset)
        {
            point.X = rect.X + rect.Width;
            point.Y = rect.Y + (rect.Height * 0.5f) + offset;
        }

        private static void CalculateCenter(Rect2 rect, ref Vector2 point)
        {
            point.X = rect.X + (rect.Width * 0.5f);
            point.Y = rect.Y + (rect.Height * 0.5f);
        }
    }
}
