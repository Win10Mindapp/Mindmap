﻿// ==========================================================================
// MenuFlyoutItemCommandShortcutBehavior.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace GP.Windows.UI.Interactivity
{
    /// <summary>
    /// A behavior to invoke the command of the button when the shortcut key is pressed.
    /// </summary>
    public class MenuFlyoutItemCommandShortcutBehavior : ShortcutBehaviorBase
    {
        /// <summary>
        /// Called when the shortcut must be invoked.
        /// </summary>
        protected override void InvokeShortcut()
        {
            MenuFlyoutItem flyoutItem = AssociatedElement as MenuFlyoutItem;
            
            if (flyoutItem != null)
            {
                ICommand command = flyoutItem.Command;

                if (command != null)
                {
                    object parameter = flyoutItem.CommandParameter;

                    if (command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }
                }
            }
        }
    }
}
