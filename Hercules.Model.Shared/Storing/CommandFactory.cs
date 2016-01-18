﻿// ==========================================================================
// CommandFactory.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hercules.Model.Storing
{
    public static class CommandFactory
    {
        private const string Suffix = "Command";
        private const string ModelAssemblyNew = "Hercules.Model.Shared,";
        private const string ModelAssemblyOld = "Hercules.Model,";
        private static readonly Dictionary<string, Type> TypesByName = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<Type, string> NamesByType = new Dictionary<Type, string>();

        static CommandFactory()
        {
            Type commandBaseType = typeof(IUndoRedoCommand);

            Assembly assembly = typeof(CommandFactory).GetTypeInfo().Assembly;

            foreach (Type type in assembly.GetTypes())
            {
                TypeInfo typeInfo = type.GetTypeInfo();

                if (type.GetInterfaces().Contains(commandBaseType))
                {
                    string typeName = ResolveTypeName(type);

                    AddCommand(type, typeName);
                }

                LegacyNameAttribute legacyName = typeInfo.GetCustomAttribute<LegacyNameAttribute>();

                if (!string.IsNullOrWhiteSpace(legacyName?.OldName))
                {
                    AddCommand(type, legacyName.OldName);
                }
            }
        }

        public static string ToTypeName(IUndoRedoCommand command)
        {
            return NamesByType[command.GetType()];
        }

        private static void AddCommand(Type type, string name)
        {
            TypesByName[name] = type;

            if (!NamesByType.ContainsKey(type))
            {
                NamesByType[type] = name;
            }
        }

        private static string ResolveTypeName(Type type)
        {
            string result = type.Name;

            if (result.EndsWith(Suffix, StringComparison.OrdinalIgnoreCase))
            {
                result = result.Substring(0, result.Length - Suffix.Length);
            }

            return result;
        }

        public static IUndoRedoCommand CreateCommand(string typeName, PropertiesBag properties, Document document)
        {
            Type type = ResolveType(typeName);

            return CreateCommand(properties, document, type);
        }

        private static IUndoRedoCommand CreateCommand(PropertiesBag properties, Document document, Type type)
        {
            return (IUndoRedoCommand)Activator.CreateInstance(type, properties, document);
        }

        private static Type ResolveType(string typeName)
        {
            Type type = TryResolveTypeName(typeName);

            if (type == null)
            {
                if (!typeName.Contains(ModelAssemblyNew))
                {
                    string newTypeName = typeName.Replace(ModelAssemblyOld, ModelAssemblyNew);

                    type = TryResolveTypeName(newTypeName);
                }
            }

            if (type == null)
            {
                throw new IOException($"Invalid type name: '{typeName}");
            }

            return type;
        }

        private static Type TryResolveTypeName(string typeName)
        {
            Type type;

            TypesByName.TryGetValue(typeName, out type);

            if (type == null)
            {
                try
                {
                    type = Type.GetType(typeName);
                }
                catch
                {
                    type = null;
                }
            }

            return type;
        }
    }
}