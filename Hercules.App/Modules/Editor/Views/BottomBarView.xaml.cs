﻿// ==========================================================================
// BottomBarView.xaml.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using Windows.UI.Xaml;

namespace Hercules.App.Modules.Editor.Views
{
    public sealed partial class BottomBarView
    {
        public event EventHandler ListButtonClicked;

        public BottomBarView()
        {
            InitializeComponent();
        }

        private void ListAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ListButtonClicked?.Invoke(sender, EventArgs.Empty);
        }
    }
}
