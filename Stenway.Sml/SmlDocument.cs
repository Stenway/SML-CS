using System;
using System.Collections.Generic;
using Stenway.ReliableTxt;
using Stenway.Wsv;

namespace Stenway.Sml
{
	public class SmlDocument
	{
		internal SmlElement root;
		internal ReliableTxtEncoding encoding;
		internal string endKeyword = "End";
		internal string defaultIndentation;
		
		public SmlElement Root
		{
			get
			{
				return root;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Root element cannot be null");
				}
				this.root = value;
			}
		}
		
		public ReliableTxtEncoding Encoding 
		{
			get { return encoding; } 
			set
			{
				int encodingIndex = (int)value;
				encoding = value;
			}
		}
		
		public string EndKeyword
		{
			get
			{
				return endKeyword;
			}
			set
			{
				this.endKeyword = value;
			}
		}
		
		public string DefaultIndentation
		{
			get
			{
				return defaultIndentation;
			}
			set
			{
				if (value != null && value.Length > 0 &&
						!WsvString.isWhitespace(value)) {
					throw new ArgumentException(
							"Indentation value contains non whitespace character or line feed");
				}
				this.defaultIndentation = value;
			}
		}
		
		public readonly List<SmlEmptyNode> EmptyNodesBefore = new List<SmlEmptyNode>();
		public readonly List<SmlEmptyNode> EmptyNodesAfter = new List<SmlEmptyNode>();
		
		internal SmlDocument(ReliableTxtEncoding encoding = ReliableTxtEncoding.Utf8) : this(new SmlElement("Root"), encoding)
		{
			
		}
		
		public SmlDocument(string rootName, ReliableTxtEncoding encoding = ReliableTxtEncoding.Utf8) :
			this(new SmlElement(rootName), encoding)
		{
			
		}
		
		public SmlDocument(SmlElement root, ReliableTxtEncoding encoding = ReliableTxtEncoding.Utf8) {
			Root = root;
			Encoding = encoding;
		}
		
		
		public void Minify() {
			EmptyNodesBefore.Clear();
			EmptyNodesAfter.Clear();
			DefaultIndentation = "";
			EndKeyword = null;
			root.Minify();
		}
		
		public override string ToString()
		{
			return ToString(true);
		}
		
		public string ToString(bool preserveWhitespaceAndComments)
		{
			if (preserveWhitespaceAndComments)
			{
				return SmlSerializer.SerializeDocument(this);
			}
			else
			{
				return SmlSerializer.SerializeDocumentNonPreserving(this, false);
			}
		}
		
		public string ToStringMinified()
		{
			return SmlSerializer.SerializeDocumentNonPreserving(this, true);
		}
		
		public void Save(string filePath) 
		{
			string content = ToString();
			ReliableTxtDocument.Save(content, encoding, filePath);
		}
		
		public static SmlDocument Load(string filePath)
		{
			ReliableTxtDocument txt = ReliableTxtDocument.Load(filePath);
			SmlDocument document = Parse(txt.Text);
			document.encoding = txt.Encoding;
			return document;
		}
	
		public static SmlDocument Parse(string content)
		{
			return Parse(content, true);
		}
		
		public static SmlDocument Parse(string content, bool preserveWhitespaceAndComments)
		{
			if (preserveWhitespaceAndComments)
			{
				return SmlParser.ParseDocument(content);
			}
			else
			{
				return SmlParser.ParseDocumentNonPreserving(content);
			}
		}
	}
}