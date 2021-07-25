using System;
namespace Stenway.Sml
{
	public class SmlParserException : Exception
	{
		public readonly int LineIndex;
		
		public SmlParserException(int lineIndex, string message) :
			base(string.Format("{0} ({1})", message, lineIndex + 1))
		{
			LineIndex = lineIndex;
		}
	}
}