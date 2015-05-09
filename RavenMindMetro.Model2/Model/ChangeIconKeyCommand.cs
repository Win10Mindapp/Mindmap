﻿// ==========================================================================
// ChangeIconKeyCommand.cs
// RavenMind Application
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

namespace RavenMind.Model
{
    public sealed class ChangeIconKeyCommand : CommandBase
    {
        private string newIconKey;
        private string oldIconKey;

        public ChangeIconKeyCommand(CommandProperties properties, Document document)
            : base(properties, document)
        {
            newIconKey = properties.Get<string>("IconKey");
        }

        public ChangeIconKeyCommand(NodeBase nodeId, string newIconKey)
            : base(nodeId)
        {
            this.newIconKey = newIconKey;
        }

        public override void Save(CommandProperties properties)
        {
            properties.Set("IconKey", newIconKey);

            base.Save(properties);
        }

        public override void Execute()
        {
            oldIconKey = Node.IconKey;

            Node.ChangeIconKey(newIconKey);
            Node.Select();
        }

        public override void Revert()
        {
            Node.ChangeIconKey(oldIconKey);
            Node.Select();
        }
    }
}
