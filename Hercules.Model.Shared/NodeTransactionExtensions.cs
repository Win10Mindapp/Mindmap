﻿// ==========================================================================
// NodeTransactionExtensions.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using GP.Utils;

// ReSharper disable InvertIf

namespace Hercules.Model
{
    public static class NodeTransactionExtensions
    {
        public static NodeBase AddSibilingTransactional(this NodeBase node)
        {
            var selectedNormalNode = node as Node;

            if (selectedNormalNode?.Document != null)
            {
                var command = new InsertChildCommand(selectedNormalNode.Parent, null, selectedNormalNode.NodeSide);

                var tansactionName = LocalizationManager.GetString("TransactionName_AddSibling");

                selectedNormalNode.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(command);
                });

                command.Child.Select();

                return command.Child;
            }

            return null;
        }

        public static NodeBase AddChildTransactional(this NodeBase node)
        {
            if (node?.Document != null)
            {
                var command = new InsertChildCommand(node, null, NodeSide.Auto);

                var tansactionName = LocalizationManager.GetString("TransactionName_AddChild");

                node.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(command);
                });

                command.Child.Select();

                return command.Child;
            }

            return null;
        }

        public static void RemoveTransactional(this NodeBase node)
        {
            var selectedNormalNode = node as Node;

            if (selectedNormalNode?.Document != null)
            {
                (node.FindBottomOf() ?? node.Parent)?.Select();

                var tansactionName = LocalizationManager.GetString("TransactionName_RemoveNode");

                node.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(new RemoveChildCommand(selectedNormalNode.Parent, selectedNormalNode));
                });
            }
        }

        public static void MoveTransactional(this NodeBase node, NodeBase target, int? index, NodeSide side)
        {
            var selectedNormalNode = node as Node;

            if (selectedNormalNode?.Document != null)
            {
                var tansactionName = LocalizationManager.GetString("TransactionName_MoveNode");

                selectedNormalNode.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(new RemoveChildCommand(selectedNormalNode.Parent, selectedNormalNode));

                    commands.Apply(new InsertChildCommand(target, index, side, selectedNormalNode));
                });
            }
        }

        public static void CollapseTransactional(this NodeBase node)
        {
            if (node.Document != null && node.HasChildren && !node.IsCollapsed)
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ExpandCollapse");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ToggleCollapseCommand(node));
                });
            }
        }

        public static void ExpandTransactional(this NodeBase node)
        {
            if (node.Document != null && node.HasChildren && node.IsCollapsed)
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ExpandCollapse");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ToggleCollapseCommand(node));
                });
            }
        }

        public static void ToggleCheckedTransactional(this NodeBase node)
        {
            if (node.Document != null)
            {
                var tansactionName = LocalizationManager.GetString("TransactionName_ToggleChecked");

                node.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(new ToggleCheckedCommand(node));
                });
            }
        }

        public static void ToggleNotesTransactional(this NodeBase node)
        {
            if (node.Document != null)
            {
                var tansactionName = LocalizationManager.GetString("TransactionName_ToggleNotes");

                node.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(new ToggleNotesCommand(node));
                });
            }
        }

        public static void ToggleHullTransactional(this NodeBase node)
        {
            if (node.Document != null)
            {
                var tansactionName = LocalizationManager.GetString("TransactionName_ToggleHull");

                node.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(new ToggleHullCommand(node));
                });
            }
        }

        public static void ToggleCollapseTransactional(this NodeBase node)
        {
            if (node.Document != null && node.HasChildren)
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ExpandCollapse");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ToggleCollapseCommand(node));
                });
            }
        }

        public static void ChangeShapeTransactional(this Node node, NodeShape? shape)
        {
            if (node.Document != null && !Equals(node.Shape, shape))
            {
                var tansactionName = LocalizationManager.GetString("TransactionName_ChangeShape");

                node.Document.MakeTransaction(tansactionName, commands =>
                {
                    commands.Apply(new ChangeShapeCommand(node, shape));
                });
            }
        }

        public static void ChangeTextTransactional(this NodeBase node, string text)
        {
            if (node.Document != null && !Equals(node.Text, text))
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeText");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeTextCommand(node, text ?? string.Empty));
                });
            }
        }

        public static void ChangeNotesTransactional(this NodeBase node, string notes)
        {
            if (node.Document != null && !Equals(node.Notes, notes))
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeNotes");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeNotesCommand(node, notes ?? string.Empty));
                });
            }
        }

        public static void ChangeColorTransactional(this NodeBase node, INodeColor color)
        {
            if (node.Document != null && !Equals(node.Color, color) && color != null)
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeColor");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeColorCommand(node, color));
                });
            }
        }

        public static void ChangeIconTransactional(this NodeBase node, INodeIcon icon)
        {
            if (node.Document != null && !Equals(node.Icon, icon))
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeIcon");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeIconCommand(node, icon));
                });
            }
        }

        public static void ChangeCheckableModeTransactional(this NodeBase node, CheckableMode checkableMode)
        {
            if (node.Document != null && !Equals(node.CheckableMode, checkableMode))
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeCheckableMode");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeCheckableModeCommand(node, checkableMode));
                });
            }
        }

        public static void ChangeIconSizeTransactional(this NodeBase node, IconSize iconSize)
        {
            if (node.Document != null && !Equals(node.IconSize, iconSize))
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeIconSize");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeIconSizeCommand(node, iconSize));
                });
            }
        }

        public static void ChangeIconPositionTransactional(this NodeBase node, IconPosition iconPosition)
        {
            if (node.Document != null && !Equals(node.IconPosition, iconPosition))
            {
                var transactionName = LocalizationManager.GetString("TransactionName_ChangeIconPosition");

                node.Document.MakeTransaction(transactionName, commands =>
                {
                    commands.Apply(new ChangeIconPositionCommand(node, iconPosition));
                });
            }
        }
    }
}
