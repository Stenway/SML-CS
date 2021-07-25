using System;
using System.Collections.Generic;
using Stenway.Wsv;
using System.Linq;

namespace Stenway.Sml
{
	public class SmlElement : SmlNamedNode
	{
		public readonly List<SmlNode> Nodes = new List<SmlNode>();
		
		internal string[] endWhitespaces;
		internal string endComment;
		
		public string EndComment
		{
			get
			{
				return endComment;
			}
			set
			{
				WsvLine.ValidateComment(value);
				this.endComment = value;
			}
		}
		
		
		public string[] EndWhitespaces
		{
			get
			{
				if (endWhitespaces == null)
				{
					return null;
				}
				return (string[])endWhitespaces.Clone();
			}
			set
			{
				WsvLine.ValidateWhitespaces(value);
				this.endWhitespaces = value;
			}
		}
		
		public SmlElement(string name) : base(name)
		{
			
		}
	
		public void SetEndWhitespaces(params string[] whitespaces)
		{
			EndWhitespaces = whitespaces;
		}
					
		internal void SetEndWhitespacesAndComment(string[] whitespaces, string comment)
		{
			this.endWhitespaces = whitespaces;
			this.endComment = comment;
		}
		
		public SmlNode Add(SmlNode node)
		{
			Nodes.Add(node);
			return node;
		}
		
		public SmlAttribute AddAttribute(string name, params string[] values)
		{
			SmlAttribute attribute = new SmlAttribute(name, values);
			Add(attribute);
			return attribute;
		}
		
		public SmlAttribute AddAttribute(string name, params object[] values)
		{
			SmlAttribute attribute = new SmlAttribute(name, values);
			Add(attribute);
			return attribute;
		}
				
		public SmlAttribute AddAttribute(string name, params byte[][] values)
		{
			SmlAttribute attribute = new SmlAttribute(name, values);
			Add(attribute);
			return attribute;
		}
		
		public SmlElement AddElement(string name)
		{
			SmlElement element = new SmlElement(name);
			Add(element);
			return element;
		}
		
		public SmlEmptyNode AddEmptyNode()
		{
			SmlEmptyNode emptyNode = new SmlEmptyNode();
			Add(emptyNode);
			return emptyNode;
		}
		
		public SmlAttribute[] Attributes()
		{
			return Nodes.Where(x => x is SmlAttribute).Cast<SmlAttribute>().ToArray();
		}
		
		public SmlAttribute[] Attributes(string name)
		{
			return Nodes.Where(x => x is SmlAttribute && ((SmlNamedNode)x).HasName(name)).Cast<SmlAttribute>().ToArray();
		}
		
		public SmlAttribute Attribute(string name)
		{
			SmlAttribute attribute = Nodes.Where(x => x is SmlAttribute && ((SmlNamedNode)x).HasName(name)).Cast<SmlAttribute>().FirstOrDefault();
			if (attribute != null) {
				return attribute;
			} else {
				throw new ArgumentException("Element \""+Name+"\" does not contain a \""+name+"\" attribute");
			}
		}
		
		public bool HasAttribute(string name)
		{
			return Nodes.Any(x => x is SmlAttribute && ((SmlNamedNode)x).HasName(name));
		}
		
		public SmlElement[] Elements()
		{
			return Nodes.Where(x => x is SmlElement).Cast<SmlElement>().ToArray();
		}
		
		public SmlElement[] Elements(string name)
		{
			return Nodes.Where(x => x is SmlElement && ((SmlNamedNode)x).HasName(name)).Cast<SmlElement>().ToArray();
		}
		
		public SmlElement Element(string name)
		{
			SmlElement element = Nodes.Where(x => x is SmlElement && ((SmlNamedNode)x).HasName(name)).Cast<SmlElement>().FirstOrDefault();
			if (element != null) {
				return element;
			} else {
				throw new ArgumentException("Element \""+Name+"\" does not contain a \""+name+"\" element");
			}
		}
		
		public bool HasElement(string name)
		{
			return Nodes.Any(x => x is SmlElement && ((SmlNamedNode)x).HasName(name));
		}
		
		public string GetString(string attributeName)
		{
			return Attribute(attributeName).GetString();
		}
		
		public int GetInt(string attributeName)
		{
			return Attribute(attributeName).GetInt();
		}
		
		public float GetFloat(string attributeName)
		{
			return Attribute(attributeName).GetFloat();
		}
		
		public double GetDouble(string attributeName)
		{
			return Attribute(attributeName).GetDouble();
		}
		
		public bool GetBool(string attributeName)
		{
			return Attribute(attributeName).GetBool();
		}
		
		public byte[] GeBytes(string attributeName)
		{
			return Attribute(attributeName).GetBytes();
		}
		
		public string GetString(string attributeName, string defaultValue)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetString();
			}
			else
			{
				return defaultValue;
			}
		}
		
		public int GetInt(string attributeName, int defaultValue)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetInt();
			}
			else
			{
				return defaultValue;
			}
		}
		
		public float GetFloat(string attributeName, float defaultValue)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetFloat();
			}
			else
			{
				return defaultValue;
			}
		}
		
		public double GetDouble(string attributeName, double defaultValue)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetDouble();
			}
			else
			{
				return defaultValue;
			}
		}
		
		public bool GetBool(string attributeName, bool defaultValue)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetBool();
			}
			else
			{
				return defaultValue;
			}
		}
		
		public byte[] GetBytes(string attributeName, byte[] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetBytes();
			}
			else
			{
				return defaultValues;
			}
		}
		
		public string GetStringOrNull(string attributeName)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetString();
			}
			else
			{
				return null;
			}
		}
		
		public int? GetIntOrNull(string attributeName)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetInt();
			}
			else
			{
				return null;
			}
		}
		
		public float? GetFloatOrNull(string attributeName)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetFloat();
			}
			else
			{
				return null;
			}
		}
		
		public double? GetDoubleOrNull(string attributeName)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetDouble();
			}
			else
			{
				return null;
			}
		}
		
		public bool? GetBoolOrNull(string attributeName)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetBool();
			}
			else
			{
				return null;
			}
		}
		
		public byte[] GetBytesOrNull(string attributeName)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetBytes();
			}
			else
			{
				return null;
			}
		}
		
		public string[] GetValues(string attributeName)
		{
			return Attribute(attributeName).Values;
		}
		
		public int[] GetIntValues(string attributeName)
		{
			return Attribute(attributeName).GetIntValues();
		}
		
		public float[] GetFloatValues(string attributeName)
		{
			return Attribute(attributeName).GetFloatValues();
		}
		
		public double[] GetDoubleValues(string attributeName)
		{
			return Attribute(attributeName).GetDoubleValues();
		}
		
		public bool[] GetBoolValues(string attributeName)
		{
			return Attribute(attributeName).GetBoolValues();
		}
		
		public byte[][] GetBytesValues(string attributeName)
		{
			return Attribute(attributeName).GetBytesValues();
		}
			
		public string[] GetValues(string attributeName, string[] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).Values;
			}
			else
			{
				return defaultValues;
			}
		}
		
		public int[] GetIntValues(string attributeName, int[] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetIntValues();
			}
			else
			{
				return defaultValues;
			}
		}
		
		public float[] GetFloatValues(string attributeName, float[] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetFloatValues();
			}
			else
			{
				return defaultValues;
			}
		}
		
		public double[] GetDoubleValues(string attributeName, double[] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetDoubleValues();
			}
			else
			{
				return defaultValues;
			}
		}
		
		public bool[] GetBoolValues(string attributeName, bool[] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetBoolValues();
			}
			else
			{
				return defaultValues;
			}
		}
		
		public byte[][] GetBytesValues(string attributeName, byte[][] defaultValues)
		{
			if (HasAttribute(attributeName))
			{
				return Attribute(attributeName).GetBytesValues();
			}
			else
			{
				return defaultValues;
			}
		}
		
		public override string ToString()
		{
			return SmlSerializer.SerializeElement(this);
		}
		
		internal override void ToWsvLines(WsvDocument document, int level, string defaultIndentation, string endKeyword)
		{
			SmlSerializer.SerializeElement(this, document, level, defaultIndentation, endKeyword);
		}
		
		public override void Minify()
		{
			SmlNode[] toRemoveList = Nodes.Where(x => x is SmlEmptyNode).ToArray();
			foreach (SmlNode toRemove in toRemoveList)
			{
				Nodes.Remove(toRemove);
			}
			whitespaces = null;
			comment = null;
			endWhitespaces = null;
			endComment = null;
			foreach (SmlNode node in Nodes) {
				node.Minify();
			}
		}
	}
}