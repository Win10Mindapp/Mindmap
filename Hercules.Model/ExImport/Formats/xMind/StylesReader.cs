﻿// ==========================================================================
// xMindStylesReader.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Hercules.Model.ExImport.Formats.xMind
{
    internal static class StylesReader 
    {
        private static readonly Regex ColorRegex = new Regex("^#[0-9A-F]{6}$", RegexOptions.Compiled);
        
        public static void ReadStyles(XDocument mapStyles, IDictionary<string, xMindStyle> stylesById)
        {
            XElement nodeStyles = mapStyles.Root.Element(Namespaces.Styles("styles"));

            if (nodeStyles != null)
            {
                foreach (XElement style in nodeStyles.Elements(Namespaces.Styles("style")))
                {
                    string id = style.AttributeValue("id");

                    if (string.IsNullOrWhiteSpace(id))
                    {
                        continue;
                    }

                    if (!style.IsAttributeEquals("type", "topic"))
                    {
                        continue;
                    }

                    XElement properties = style.Element(Namespaces.Styles("topic-properties"));

                    if (properties == null)
                    {
                        continue;
                    }

                    string fillString = properties.AttributeValue(Namespaces.SVG("fill"));

                    if (string.IsNullOrWhiteSpace(fillString) || !ColorRegex.IsMatch(fillString))
                    {
                        continue;
                    }

                    int color;

                    if (int.TryParse(fillString.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out color))
                    {
                        stylesById[id] = new xMindStyle { Color = color };
                    }
                }
            }
        }
    }
}
