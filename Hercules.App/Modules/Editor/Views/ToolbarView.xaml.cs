﻿// ==========================================================================
// ToolbarView.xaml.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using Windows.UI.Xaml;

namespace Hercules.App.Modules.Editor.Views
{
    public sealed partial class ToolbarView
    {
        public event EventHandler<RoutedEventArgs> ListButtonClicked;

        public event EventHandler<RoutedEventArgs> PropertiesButtonClicked;

        public ToolbarView()
        {
            InitializeComponent();
        }

        private void ListAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ListButtonClicked?.Invoke(sender, new RoutedEventArgs());
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            PropertiesButtonClicked?.Invoke(sender, new RoutedEventArgs());
        }
    }
}
