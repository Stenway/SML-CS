using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stenway.Wsv;

namespace Stenway.Sml
{
	public static class SmlSerializer
	{
		public static string SerializeDocument(SmlDocument document)
		{
			WsvDocument wsvDocument = new WsvDocument();
			
			SerialzeEmptyNodes(document.EmptyNodesBefore, wsvDocument);
			document.Root.ToWsvLines(wsvDocument, 0, document.defaultIndentation, document.endKeyword);
			SerialzeEmptyNodes(document.EmptyNodesAfter, wsvDocument);
			
			return wsvDocument.ToString();
		}
		
		public static string SerializeElement(SmlElement element)
		{
			WsvDocument wsvDocument = new WsvDocument();
			element.ToWsvLines(wsvDocument, 0, null, "End");
			return wsvDocument.ToString();
		}
	
		public static string SerializeAttribute(SmlAttribute attribute)
		{
			WsvDocument wsvDocument = new WsvDocument();
			attribute.ToWsvLines(wsvDocument, 0, null, null);
			return wsvDocument.ToString();
		}
		
		public static string SerializeEmptyNode(SmlEmptyNode emptyNode)
		{
			WsvDocument wsvDocument = new WsvDocument();
			emptyNode.ToWsvLines(wsvDocument, 0, null, null);
			return wsvDocument.ToString();
		}
		
		private static void SerialzeEmptyNodes(List<SmlEmptyNode> emptyNodes, WsvDocument wsvDocument)
		{
			foreach (SmlEmptyNode emptyNode in emptyNodes)
			{
				emptyNode.ToWsvLines(wsvDocument, 0, null, null);
			}
		}
	
		public static void SerializeElement(SmlElement element, WsvDocument wsvDocument,
				int level, string defaultIndentation, string endKeyword)
		{
			if (endKeyword != null && element.HasName(endKeyword))
			{
				throw new SmlException("Element name matches the end keyword '"+endKeyword+"'");
			}
			int childLevel = level + 1;
			
			string[] whitespaces = GetWhitespaces(element.whitespaces, level, defaultIndentation);
			wsvDocument.AddLine(new string[]{element.Name}, whitespaces, element.comment);
			
			foreach (SmlNode child in element.Nodes)
			{
				child.ToWsvLines(wsvDocument, childLevel, defaultIndentation, endKeyword);
			}
			
			string[] endWhitespaces = GetWhitespaces(element.endWhitespaces, level, defaultIndentation);
			wsvDocument.AddLine(new string[]{endKeyword}, endWhitespaces, element.endComment);
		}
		
		private static string[] GetWhitespaces(string[] whitespaces, int level, 
				string defaultIndentation)
		{
			if (whitespaces != null && whitespaces.Length > 0)
			{
				return whitespaces;
			}
			if (defaultIndentation == null)
			{
				string indentStr = new string('\t', level);
				return new string[] { indentStr };
			}
			else
			{
				string indentStr = GetIndentationString(defaultIndentation, level);
				return new string[] {indentStr};
			}
		}
		
		private static string GetIndentationString(string defaultIndentation, int level)
		{
			return new StringBuilder(defaultIndentation.Length * level).Insert(0, defaultIndentation, level).ToString();
		}
		
		public static void SerializeAttribute(SmlAttribute attribute, WsvDocument wsvDocument,
				int level, string defaultIndentation)
		{
			string[] whitespaces = GetWhitespaces(attribute.whitespaces, level, defaultIndentation);
			string[] combined = Combine(attribute.Name, attribute.values);
			wsvDocument.AddLine(combined, whitespaces, attribute.comment);
		}
		
		private static string[] Combine(string name, string[] values)
		{
			return new string[] {name}.Concat(values).ToArray();
		}
		
		public static void SerializeEmptyNode(SmlEmptyNode emptyNode, WsvDocument wsvDocument, 
				int level, string defaultIndentation)
		{
			string[] whitespaces = GetWhitespaces(emptyNode.whitespaces, level, defaultIndentation);
			wsvDocument.AddLine(null, whitespaces, emptyNode.comment);
		}
		
		public static string SerializeDocumentNonPreserving(SmlDocument document)
		{
			return SerializeDocumentNonPreserving(document, false);
		}
		
		public static string SerializeDocumentNonPreserving(SmlDocument document, bool minified)
		{
			StringBuilder sb = new StringBuilder();
			string defaultIndentation = document.DefaultIndentation;
			if (defaultIndentation == null)
			{
				defaultIndentation = "\t";
			}
			string endKeyword = document.EndKeyword;
			if (minified)
			{
				defaultIndentation = "";
				endKeyword = null;
			}
			SerializeElementNonPreserving(sb, document.Root, 0, defaultIndentation, endKeyword);
			sb.Remove(sb.Length-1,1);
			return sb.ToString();
		}
	
		private static void SerializeElementNonPreserving(StringBuilder sb, SmlElement element,
				int level, string defaultIndentation, string endKeyword)
		{
			SerializeIndentation(sb, level, defaultIndentation);
			WsvSerializer.SerializeValue(sb, element.Name);
			sb.Append('\n'); 
	
			int childLevel = level + 1;
			foreach (SmlNode child in element.Nodes)
			{
				if (child is SmlElement)
				{
					SerializeElementNonPreserving(sb, (SmlElement)child, childLevel, defaultIndentation, endKeyword);
				}
				else if (child is SmlAttribute)
				{
					SerializeAttributeNonPreserving(sb, (SmlAttribute)child, childLevel, defaultIndentation);
				}
			}
			
			SerializeIndentation(sb, level, defaultIndentation);
			WsvSerializer.SerializeValue(sb, endKeyword);
			sb.Append('\n'); 
		}
		
		private static void SerializeAttributeNonPreserving(StringBuilder sb, SmlAttribute attribute,
				int level, string defaultIndentation)
		{
			SerializeIndentation(sb, level, defaultIndentation);
			WsvSerializer.SerializeValue(sb, attribute.Name);
			sb.Append(' '); 
			WsvSerializer.SerializeLine(sb, attribute.values);
			sb.Append('\n'); 
		}
		
		private static void SerializeIndentation(StringBuilder sb, int level, string defaultIndentation)
		{
			string indentStr = GetIndentationString(defaultIndentation, level);
			sb.Append(indentStr);
		}
	}
}