using Stenway.Wsv;

namespace Stenway.Sml
{
	class WsvDocumentLineIterator : IWsvLineIterator
	{
		WsvDocument wsvDocument;
		string endKeyword;
	
		int index;
	
		public WsvDocumentLineIterator(WsvDocument wsvDocument, string endKeyword)
		{
			this.wsvDocument = wsvDocument;
			this.endKeyword = endKeyword;
		}
		
		public string GetEndKeyword()
		{
			return endKeyword;
		}
		
		public bool HasLine()
		{
			return index < wsvDocument.Lines.Count;
		}
		
		public bool IsEmptyLine()
		{
			return HasLine() && !wsvDocument.Lines[index].HasValues();
		}
	
		public WsvLine GetLine()
		{
			WsvLine line = wsvDocument.Lines[index];
			index++;
			return line;
		}
	
		public string[] GetLineAsArray()
		{
			return GetLine().Values;
		}
	
		public override string ToString()
		{
			string result = "(" + index + "): ";
			if (HasLine())
			{
				result += wsvDocument.Lines[index].ToString();
			}
			return result;
		}
	
		public int GetLineIndex()
		{
			return index;
		}
	}
}
