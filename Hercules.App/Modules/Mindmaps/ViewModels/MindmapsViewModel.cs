﻿// ==========================================================================
// MindmapsViewModel.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GP.Windows;
using Hercules.App.Components;
using Hercules.App.Messages;
using Hercules.Model;
using Hercules.Model.Storing;
using Microsoft.Practices.Unity;
using PropertyChanged;

namespace Hercules.App.Modules.Mindmaps.ViewModels
{
    [ImplementPropertyChanged]
    public sealed class MindmapsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<MindmapItem> mindmaps = new ObservableCollection<MindmapItem>();

        public ObservableCollection<MindmapItem> Mindmaps
        {
            get
            {
                return mindmaps;
            }
        }

        [Dependency]
        public IDocumentStore DocumentStore { get; set; }

        [Dependency]
        public ISettingsProvider SettingsProvider { get; set; }

        [Dependency]
        public ILocalizationManager LocalizationManager { get; set; }

        [NotifyUI]
        public MindmapItem SelectedMindmap { get; set; }

        [NotifyUI]
        public bool IsLoaded { get; set; }

        public MindmapsViewModel()
        {
            Messenger.Default.Register<DeleteMindmapMessage>(this, OnDeleteMindmap);
            Messenger.Default.Register<OpenMindmapMessage>(this, OnOpenMindmap);
            Messenger.Default.Register<NameChangedMessage>(this, OnNameChanged);
            Messenger.Default.Register<MindmapSavedMessage>(this, OnMindmapSaved);
        }

        public void OnMindmapSaved(MindmapSavedMessage message)
        {
            MindmapItem item = Mindmaps.FirstOrDefault(x => x.MindmapId == message.Content);

            if (item != null)
            {
                item.LastUpdate = DateTimeOffset.UtcNow;
            }
        }

        public void OnNameChanged(NameChangedMessage message)
        {
            if (SelectedMindmap != null)
            {
                SelectedMindmap.Title = message.Content;
            }
        }

        public void OnOpenMindmap(OpenMindmapMessage message)
        {
            Func<MindmapItem, bool> predicate = x => true;

            if (message.Content != null)
            {
                predicate = x => x.MindmapId == message.Content.Value;
            }

            SelectedMindmap = Mindmaps.FirstOrDefault(predicate) ?? Mindmaps.FirstOrDefault();
        }

        public async void OnDeleteMindmap(DeleteMindmapMessage message)
        {
            await DocumentStore.DeleteAsync(message.Content);

            Mindmaps.Remove(Mindmaps.Single(x => x.MindmapId == message.Content));
        }

        public void OnSelectedMindmapChanged()
        {
            if (IsLoaded && SelectedMindmap != null)
            {
                Messenger.Default.Send(new OpenMindmapMessage(SelectedMindmap.MindmapId));
            }
        }

        public async Task CreateNewMindmapAsync(string name, string text)
        {
            Document document = new Document(Guid.NewGuid(), name);

            DocumentRef documentRef = await DocumentStore.StoreAsync(document, null);

            Mindmaps.Insert(0, new MindmapItem(documentRef));

            SelectedMindmap = Mindmaps.FirstOrDefault();
        }

        public async Task LoadAsync()
        {
            if (!IsLoaded)
            {
                IsLoaded = true;

                if (SettingsProvider.IsAlreadyStarted)
                {
                    IEnumerable<DocumentRef> documents = await DocumentStore.LoadAllAsync();

                    foreach (DocumentRef documentRef in documents)
                    {
                        if (Mindmaps.All(x => x.MindmapId != documentRef.DocumentId))
                        {
                            MindmapItem mindmapItem = new MindmapItem(documentRef);

                            for (int i = 0; i < 100; i++)
                            {
                                Mindmaps.Add(mindmapItem);
                            }
                        }
                    }

                    SelectedMindmap = Mindmaps.FirstOrDefault();
                }
                else
                {
                    SettingsProvider.IsAlreadyStarted = true;

                    await CreateNewMindmapAsync(LocalizationManager.GetString("MyMindmap"), LocalizationManager.GetString("Start"));
                }
            }
        }
    }
}