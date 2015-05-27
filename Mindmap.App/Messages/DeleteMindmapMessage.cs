﻿// ==========================================================================
// DeleteMindmapMessage.cs
// Mindmap Application
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using GalaSoft.MvvmLight.Messaging;
using System;

namespace Mindmap.Messages
{
    public sealed class DeleteMindmapMessage : GenericMessage<Guid>
    {
        public DeleteMindmapMessage(Guid id)
            : base(id)
        {
        }
    }
}
