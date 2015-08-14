﻿// ==========================================================================
// HtmlOutlineGenerator.cs
// Hercules Mindmap App
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ==========================================================================
using GP.Windows;
using GP.Windows.UI;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hercules.Model.Export.Html
{
    public sealed class HtmlOutlineGenerator : IOutlineGenerator
    {
        private const string ULStyle = "padding-left:18px;";
        private const string LIStyle = "padding-top:4px;padding-bottom:4px;";

        public string GenerateOutline(Document document, bool useColors, string noTextPlaceholder)
        {
            Guard.NotNull(document, nameof(document));
            Guard.NotNullOrEmpty(noTextPlaceholder, nameof(noTextPlaceholder));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings { OmitXmlDeclaration = true });

                xmlWriter.WriteStartElement("div");

                WriteNode(xmlWriter, document.Root, "1.4em", useColors, noTextPlaceholder);

                List<Node> children = document.Root.LeftChildren.Union(document.Root.RightChildren).ToList();

                if (children.Count > 0)
                {
                    xmlWriter.WriteStartElement("ul");
                    xmlWriter.WriteAttributeString("style", ULStyle);

                    foreach (Node node in children)
                    {
                        xmlWriter.WriteStartElement("li");
                        xmlWriter.WriteAttributeString("style", LIStyle);

                        WriteNodeWithChildren(xmlWriter, node, "1.2em", useColors, noTextPlaceholder);

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                memoryStream.Position = 0;

                byte[] buffer = memoryStream.ToArray();
                
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }

        private static void WriteNodeWithChildren(XmlWriter xmlWriter, Node node, string fontSize, bool useColors, string noTextPlaceholder)
        {
            WriteNode(xmlWriter, node, fontSize, useColors, noTextPlaceholder);

            if (node.Children.Count > 0)
            {
                xmlWriter.WriteStartElement("ul");
                xmlWriter.WriteAttributeString("style", ULStyle);

                foreach (Node child in node.Children)
                {
                    xmlWriter.WriteStartElement("li");
                    xmlWriter.WriteAttributeString("style", LIStyle);

                    WriteNodeWithChildren(xmlWriter, child, "1.em", useColors, noTextPlaceholder);

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }
        }

        private static void WriteNode(XmlWriter xmlWriter, NodeBase nodeBase, string fontSize, bool useColors, string noTextPlaceholder)
        {
            string color = "#000";

            if (useColors)
            {
                color = ColorsHelper.ConvertToRGBString(nodeBase.Color, 0, 0.2, -0.3);
            }

            xmlWriter.WriteStartElement("span");
            xmlWriter.WriteAttributeString("style", string.Format(CultureInfo.CurrentCulture, "color:{0};font-size:{1};", color, fontSize));

            if (!string.IsNullOrWhiteSpace(nodeBase.Text))
            {
                xmlWriter.WriteValue(nodeBase.Text);
            }
            else
            {
                xmlWriter.WriteValue(noTextPlaceholder);
            }

            xmlWriter.WriteEndElement();
        }
    }
}
