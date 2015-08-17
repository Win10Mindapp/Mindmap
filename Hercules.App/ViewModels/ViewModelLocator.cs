// ==========================================================================
// ViewModelLocator.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Hercules.App.Components;
using Hercules.App.Components.Implementations;
using Hercules.Model.Storing;
using Hercules.Model.Storing.Json;
using Windows.ApplicationModel;

namespace Hercules.App.ViewModels
{
    public class ViewModelLocator
    {
        [Dependency]
        public EditorViewModel Editor { get; set; }

        [Dependency]
        public MindmapsViewModel Mindmaps { get; set; }

        public ViewModelLocator()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                IUnityContainer unityContainer = new UnityContainer();

                unityContainer.RegisterType<IMessenger, Messenger>(
                    new ContainerControlledLifetimeManager());
                unityContainer.RegisterType<IDocumentStore, JsonDocumentStore>(
                    new ContainerControlledLifetimeManager());
                unityContainer.RegisterType<ISettingsProvider, DefaultSettingsProvider>(
                    new ContainerControlledLifetimeManager());
                unityContainer.RegisterType<ILocalizationManager, ResourcesLocalizationManager>(
                    new ContainerControlledLifetimeManager());
                unityContainer.RegisterType<MindmapsViewModel>(
                    new PerResolveLifetimeManager());
                unityContainer.RegisterType<EditorViewModel>(
                    new PerResolveLifetimeManager());

                unityContainer.BuildUp(this);
            }
        }
    }
}