﻿// ==========================================================================
// EnterNameView.xaml.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using GP.Windows.UI;
using Hercules.App.Modules.Mindmaps.ViewModels;

namespace Hercules.App.Modules.Mindmaps.Views
{
    public sealed partial class EnterNameView : IPopupControl
    {
        public Popup Popup { get; set; }

        public EnterNameView()
        {
            InitializeComponent();
        }

        public override void OnOpened()
        {
            ErrorTextBlock.Opacity = 0;

            TitleTextBox.Text = string.Empty;
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                ErrorTextBlock.Opacity = 1;
            }
            else
            {
                MindmapsViewModel viewModel = (MindmapsViewModel)DataContext;

                await viewModel.CreateNewMindmapAsync(TitleTextBox.Text, TitleTextBox.Text);

                Flyout.Hide();
            }
        }
    }
}
