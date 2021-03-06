﻿// ==========================================================================
// DocumentLoadedEventArgs.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;

namespace Hercules.App.Components
{
    public sealed class DocumentFileEventArgs : EventArgs
    {
        private readonly IDocumentFileModel file;

        public IDocumentFileModel File
        {
            get { return file; }
        }

        public DocumentFileEventArgs(IDocumentFileModel file)
        {
            this.file = file;
        }
    }
}
