﻿// ==========================================================================
// RootNode.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hercules.Model
{
    public sealed class RootNode : NodeBase
    {
        private readonly List<Node> leftChildren = new List<Node>();
        private readonly List<Node> rightChildren = new List<Node>();

        public override bool HasChildren
        {
            get { return leftChildren.Count + rightChildren.Count > 0; }
        }

        public IReadOnlyList<Node> RightChildren
        {
            get { return rightChildren; }
        }

        public IReadOnlyList<Node> LeftChildren
        {
            get { return leftChildren; }
        }

        public RootNode(Guid id)
            : base(id)
        {
        }

        public override void Remove(Node child, out int oldIndex)
        {
            var isRemoved = Remove(rightChildren, child, out oldIndex) || Remove(leftChildren, child, out oldIndex);

            if (isRemoved)
            {
                OnPropertyChanged(nameof(HasChildren));
            }
        }

        public override void Insert(Node child, int? index, NodeSide side)
        {
            if (side != NodeSide.Auto)
            {
                if (side == NodeSide.Right)
                {
                    Add(rightChildren, child, index, NodeSide.Right);
                }
                else
                {
                    Add(leftChildren, child, index, NodeSide.Left);
                }
            }
            else
            {
                if (rightChildren.Count <= leftChildren.Count)
                {
                    Add(rightChildren, child, index, NodeSide.Right);
                }
                else
                {
                    Add(leftChildren, child, index, NodeSide.Left);
                }
            }

            OnPropertyChanged(nameof(HasChildren));
        }

        public bool HasChild(Node child)
        {
            return child != null && (leftChildren.Contains(child) || rightChildren.Contains(child) || leftChildren.Any(n => n.HasChild(child)) || rightChildren.Any(n => n.HasChild(child)));
        }
    }
}
