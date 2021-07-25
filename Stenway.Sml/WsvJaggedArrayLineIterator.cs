using Stenway.Wsv;

namespace Stenway.Sml
{
	class WsvJaggedArrayLineIterator : IWsvLineIterator
	{
		private readonly string[][] lines;
		string endKeyword;
	
		int index;
	
		public WsvJaggedArrayLineIterator(string[][] lines, string endKeyword)
		{
			this.lines = lines;
			this.endKeyword = endKeyword;
		}
		
		public string GetEndKeyword()
		{
			return endKeyword;
		}
		
		public bool HasLine()
		{
			return index < lines.Length;
		}
		
		public bool IsEmptyLine()
		{
			return HasLine() && (lines[index] == null || lines[index].Length == 0);
		}
		
		public WsvLine GetLine()
		{
			return new WsvLine(GetLineAsArray());
		}
		
		public string[] GetLineAsArray()
		{
			string[] line = lines[index];
			index++;
			return line;
		}
		
		public override string ToString()
		{
			string result = "(" + index + "): ";
			if (HasLine()) {
				string[] line = lines[index];
				if (line != null) {
					result += WsvSerializer.SerializeLine(line);
				}
			}
			return result;
		}
		
		public int GetLineIndex() {
			return index;
		}
	}
}