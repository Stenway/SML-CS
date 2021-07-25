using System.Collections.Generic;
using System.Linq;
using Stenway.Wsv;
using System;

namespace Stenway.Sml
{
	class SmlParser
	{
		private static readonly string ONLY_ONE_ROOT_ELEMENT_ALLOWED				= "Only one root element allowed";
		private static readonly string ROOT_ELEMENT_EXPECTED						= "Root element expected";
		private static readonly string INVALID_ROOT_ELEMENT_START					= "Invalid root element start";
		private static readonly string NULL_VALUE_AS_ELEMENT_NAME_IS_NOT_ALLOWED	= "Null value as element name is not allowed";
		private static readonly string NULL_VALUE_AS_ATTRIBUTE_NAME_IS_NOT_ALLOWED	= "Null value as attribute name is not allowed";
		private static readonly string END_KEYWORD_COULD_NOT_BE_DETECTED			= "End keyword could not be detected";
		
		public static SmlDocument ParseDocument(string content)
		{
			WsvDocument wsvDocument = WsvDocument.Parse(content);
			string endKeyword = DetermineEndKeyword(wsvDocument);
			IWsvLineIterator iterator = new WsvDocumentLineIterator(wsvDocument, endKeyword);
			
			SmlDocument document = new SmlDocument();
			document.EndKeyword = endKeyword;
			
			SmlElement rootElement = ReadRootElement(iterator, document.EmptyNodesBefore);
			ReadElementContent(iterator, rootElement);
			document.Root = rootElement;
			
			ReadEmptyNodes(document.EmptyNodesAfter, iterator);
			if (iterator.HasLine())
			{
				throw GetException(iterator, ONLY_ONE_ROOT_ELEMENT_ALLOWED);
			}
			return document;
		}
				
		public static SmlElement ReadRootElement(IWsvLineIterator iterator, 
				List<SmlEmptyNode> emptyNodesBefore)
		{
			ReadEmptyNodes(emptyNodesBefore, iterator);
			
			if (!iterator.HasLine())
			{
				throw GetException(iterator, ROOT_ELEMENT_EXPECTED);
			}
			WsvLine rootStartLine = iterator.GetLine();
			if (!rootStartLine.HasValues() || rootStartLine.Values.Length != 1 
					|| string.Equals(iterator.GetEndKeyword(), rootStartLine.Values[0], StringComparison.OrdinalIgnoreCase))
			{
				throw GetLastLineException(iterator, INVALID_ROOT_ELEMENT_START);
			}
			string rootElementName = rootStartLine.Values[0];
			if (rootElementName == null)
			{
				throw GetLastLineException(iterator, NULL_VALUE_AS_ELEMENT_NAME_IS_NOT_ALLOWED);
			}
			SmlElement rootElement = new SmlElement(rootElementName);
			rootElement.SetWhitespacesAndComment(WsvBasedFormat.GetWhitespaces(rootStartLine), rootStartLine.Comment);
			return rootElement;
		}
		
		public static SmlNode ReadNode(IWsvLineIterator iterator, SmlElement parentElement)
		{
			SmlNode node;
			WsvLine line = iterator.GetLine();
			if (line.HasValues())
			{
				string name = line.Values[0];
				if (line.Values.Length == 1)
				{
					if (String.Equals(iterator.GetEndKeyword(), name, StringComparison.OrdinalIgnoreCase))
					{
						parentElement.SetEndWhitespacesAndComment(WsvBasedFormat.GetWhitespaces(line), line.Comment);
						return null;
					}
					if (name == null)
					{
						throw GetLastLineException(iterator, NULL_VALUE_AS_ELEMENT_NAME_IS_NOT_ALLOWED);
					}
					SmlElement childElement = new SmlElement(name);
					childElement.SetWhitespacesAndComment(WsvBasedFormat.GetWhitespaces(line), line.Comment);
	
					ReadElementContent(iterator, childElement);
	
					node = childElement;
				}
				else
				{
					if (name == null)
					{
						throw GetLastLineException(iterator, NULL_VALUE_AS_ATTRIBUTE_NAME_IS_NOT_ALLOWED);
					}
					string[] values = line.Values.Skip(1).ToArray();
					SmlAttribute childAttribute = new SmlAttribute(name, values);
					childAttribute.SetWhitespacesAndComment(WsvBasedFormat.GetWhitespaces(line), line.Comment);
	
					node = childAttribute;
				}
			}
			else
			{
				SmlEmptyNode emptyNode = new SmlEmptyNode();
				emptyNode.SetWhitespacesAndComment(WsvBasedFormat.GetWhitespaces(line), line.Comment);
	
				node = emptyNode;
			}
			return node;
		}
		
		private static void ReadElementContent(IWsvLineIterator iterator, SmlElement element)
		{
			while (true)
			{
				if (!iterator.HasLine())
				{
					throw GetLastLineException(iterator, "Element \""+element.Name+"\" not closed");
				}
				SmlNode node = ReadNode(iterator, element);
				if (node == null)
				{
					break;
				}
				element.Add(node);
			}
		}
		
		private static void ReadEmptyNodes(List<SmlEmptyNode> nodes, IWsvLineIterator iterator)
		{
			while (iterator.IsEmptyLine())
			{
				SmlEmptyNode emptyNode = ReadEmptyNode(iterator);
				nodes.Add(emptyNode);
			}
		}
		
		private static SmlEmptyNode ReadEmptyNode(IWsvLineIterator iterator)
		{
			WsvLine line = iterator.GetLine();
			SmlEmptyNode emptyNode = new SmlEmptyNode();
			emptyNode.SetWhitespacesAndComment(WsvBasedFormat.GetWhitespaces(line), line.Comment);
			return emptyNode;
		}
		
		private static string DetermineEndKeyword(WsvDocument wsvDocument)
		{
			for (int i=wsvDocument.Lines.Count-1; i>=0; i--)
			{
				string[] values = wsvDocument.Lines[i].Values;
				if (values != null)
				{
					if (values.Length == 1)
					{
						return values[0];
					}
					else if (values.Length > 1)
					{
						break;
					}
				}
			}
			throw new SmlParserException(wsvDocument.Lines.Count-1, END_KEYWORD_COULD_NOT_BE_DETECTED);
		}
		
		private static SmlParserException GetException(IWsvLineIterator iterator, string message)
		{
			return new SmlParserException(iterator.GetLineIndex(), message);
		}
	
		private static SmlParserException GetLastLineException(IWsvLineIterator iterator, string message)
		{
			return new SmlParserException(iterator.GetLineIndex()-1, message);
		}
		
		public static SmlDocument ParseDocumentNonPreserving(string content)
		{
			string[][] wsvLines = WsvDocument.ParseAsJaggedArray(content);
			return ParseDocument(wsvLines);
		}
			
		public static SmlDocument ParseDocument(string[][] wsvLines)
		{
			string endKeyword = DetermineEndKeyword(wsvLines);
			IWsvLineIterator iterator = new WsvJaggedArrayLineIterator(wsvLines, endKeyword);
			
			SmlDocument document = new SmlDocument();
			document.EndKeyword = endKeyword;
			
			SmlElement rootElement = ParseDocumentNonPreserving(iterator);
			document.Root = rootElement;
			
			return document;
		}
		
		private static SmlElement ParseDocumentNonPreserving(IWsvLineIterator iterator)
		{
			SkipEmptyLines(iterator);
			if (!iterator.HasLine())
			{
				throw GetException(iterator, ROOT_ELEMENT_EXPECTED);
			}
			
			SmlNode node = ReadNodeNonPreserving(iterator);
			if (!(node is SmlElement))
			{
				throw GetLastLineException(iterator, INVALID_ROOT_ELEMENT_START);
			}
			
			SkipEmptyLines(iterator);
			if (iterator.HasLine())
			{
				throw GetException(iterator, ONLY_ONE_ROOT_ELEMENT_ALLOWED);
			}
	
			return (SmlElement)node;
		}
		
		private static void SkipEmptyLines(IWsvLineIterator iterator)
		{
			while (iterator.IsEmptyLine())
			{
				iterator.GetLineAsArray();
			}
		}
		
		private static SmlNode ReadNodeNonPreserving(IWsvLineIterator iterator)
		{
			string[] line = iterator.GetLineAsArray();
			
			string name = line[0];
			if (line.Length == 1)
			{
				if (String.Equals(iterator.GetEndKeyword(), name, StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
				if (name == null)
				{
					throw GetLastLineException(iterator, NULL_VALUE_AS_ELEMENT_NAME_IS_NOT_ALLOWED);
				}
				SmlElement element = new SmlElement(name);
				ReadElementContentNonPreserving(iterator, element);
				return element;
			}
			else
			{
				if (name == null)
				{
					throw GetLastLineException(iterator, NULL_VALUE_AS_ATTRIBUTE_NAME_IS_NOT_ALLOWED);
				}
				string[] values = line.Skip(1).ToArray();
				SmlAttribute attribute = new SmlAttribute(name, values);
				return attribute;
			}
		}
		
		private static void ReadElementContentNonPreserving(IWsvLineIterator iterator, SmlElement element)
		{
			while (true)
			{
				SkipEmptyLines(iterator);
				if (!iterator.HasLine())
				{
					throw GetLastLineException(iterator, "Element \""+element.Name+"\" not closed");
				}
				SmlNode node = ReadNodeNonPreserving(iterator);
				if (node == null)
				{
					break;
				}
				element.Add(node);
			}
		}
		
		private static string DetermineEndKeyword(string[][] lines)
		{
			int i;
			for (i=lines.Length-1; i>=0; i--)
			{
				string[] values = lines[i];
				if (values.Length == 1)
				{
					return values[0];
				}
				else if (values.Length > 1)
				{
					break;
				}
			}
			throw new SmlParserException(lines.Length-1, END_KEYWORD_COULD_NOT_BE_DETECTED);
		}
	}
}
