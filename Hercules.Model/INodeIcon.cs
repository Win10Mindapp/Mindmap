﻿// ==========================================================================
// INodeIcon.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;

namespace Hercules.Model
{
    public interface INodeIcon : IEquatable<INodeIcon>, IWritable
    {
        int PixelWidth { get; }

        int PixelHeight { get; }
    }
}
