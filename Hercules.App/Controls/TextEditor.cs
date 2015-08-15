﻿// ==========================================================================
// TextEditor.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================
using Hercules.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Hercules.App.Components.Implementations;
using Hercules.Model.Rendering.Win2D;
using Windows.UI.Xaml.Media;
using System.Numerics;

namespace Hercules.App.Controls
{
    public sealed class TextEditor : TextBox
    {
        private readonly CompositeTransform renderTransform = new CompositeTransform();
        private Win2DRenderNode editingNode;

        public Win2DRenderNode EditingNode
        {
            get
            {
                return editingNode;
            }
        }

        public TextEditor()
        {
            RenderTransform = renderTransform;

            GotFocus += TextEditor_GotFocus;
        }

        private void TextEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                Select(0, Text.Length);
            }
        }

        public void Transform(Win2DRenderer renderer)
        {
            if (editingNode?.Node.Document != null && renderer != null)
            {
                Vector2 position = renderer.GetOverlayPosition(editingNode.TextRenderer.RenderPosition);

                renderTransform.TranslateX = position.X;
                renderTransform.TranslateY = position.Y;

                renderTransform.ScaleX = renderer.ZoomFactor;
                renderTransform.ScaleY = renderer.ZoomFactor;

                Height = editingNode.TextRenderer.RenderSize.Y;

                Width = editingNode.TextRenderer.RenderSize.X;
            }
        }

        public void BeginEdit(Win2DRenderNode renderNode)
        {
            if (renderNode != null)
            {
                if (editingNode != renderNode)
                {
                    EndEdit();

                    editingNode = renderNode;
                    editingNode.TextRenderer.HideText = true;

                    Text = renderNode.Node.Text ?? string.Empty;

                    AdjustStyle();

                    Show();
                }

                Focus(FocusState.Pointer);
            }
        }

        public void EndEdit()
        {
            if (editingNode != null)
            {
                try
                {
                    string transactionName = ResourceManager.GetString("ChangeTextTransactionName");

                    editingNode.Node.Document.MakeTransaction(transactionName, commands =>
                    {
                        commands.Apply(new ChangeTextCommand(editingNode.Node, Text, true));
                    });

                    Text = string.Empty;
                }
                finally
                {
                    Hide();

                    editingNode.TextRenderer.HideText = false;
                    editingNode = null;
                }
            }
        }

        private void AdjustStyle()
        {
            FontSize = editingNode.TextRenderer.FontSize;
        }

        private void Show()
        {
            Visibility = Visibility.Visible;
        }

        private void Hide()
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
