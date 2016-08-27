﻿// ==========================================================================
// HorizontalStraightLayoutNode.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System.Numerics;
using Hercules.Model.Rendering;

// ReSharper disable LocalVariableHidesMember
// ReSharper disable ArrangeThisQualifier

namespace Hercules.Model.Layouting.HorizontalStraight
{
    internal sealed class HorizontalStraightLayoutNode
    {
        private readonly HorizontalStraightLayoutNode parent;
        private readonly Vector2 renderSize;
        private readonly IRenderNode renderNode;

        public Vector2 Position { get; set; }

        public Vector2 TreeSize { get; set; }

        public float TreeWidth
        {
            get { return TreeSize.X; }
        }

        public float TreeHeight
        {
            get { return TreeSize.Y; }
        }

        public HorizontalStraightLayoutNode Parent
        {
            get { return parent; }
        }

        public Vector2 RenderSize
        {
            get { return renderSize; }
        }

        public float RenderWidth
        {
            get { return renderSize.X; }
        }

        public float RenderHeight
        {
            get { return renderSize.Y; }
        }

        public HorizontalStraightLayoutNode(IRenderNode renderNode, HorizontalStraightLayoutNode parent)
        {
            Vector2 renderSize = renderNode.RenderSize;

            this.parent = parent;
            this.renderSize = renderSize;
            this.renderNode = renderNode;

            TreeSize = renderNode.RenderSize;
        }

        public void MoveTo(Vector2 position, NodeSide anchor)
        {
            Position = position;

            renderNode.MoveToLayout(position, anchor);
        }
    }
}
