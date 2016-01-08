﻿// ==========================================================================
// EditorPropertiesView.xaml.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using Windows.UI.Xaml;

namespace Hercules.App.Modules.Editor.Views
{
    public sealed partial class EditorPropertiesView
    {
        public EditorPropertiesView()
        {
            InitializeComponent();
        }

        private void EditorPropertiesView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Root.Width = e.NewSize.Width;
        }
    }
}