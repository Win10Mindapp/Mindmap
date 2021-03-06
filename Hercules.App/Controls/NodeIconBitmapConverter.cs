﻿// ==========================================================================
// NodeIconBitmapConverter.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using System.IO;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using GP.Utils;
using Hercules.Model;

namespace Hercules.App.Controls
{
    public sealed class NodeIconBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var icon = value as INodeIcon;

            if (icon == null)
            {
                return null;
            }

            var bitmapImage = new BitmapImage();

            icon.OpenAsStreamAsync().ContinueWith(stream =>
            {
                if (stream.Result != null)
                {
                    CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            bitmapImage.SetSourceAsync(stream.Result.AsRandomAccessStream()).Forget();
                        }).Forget();
                }
            });

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
