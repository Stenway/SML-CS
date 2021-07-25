using Stenway.Wsv;

namespace Stenway.Sml
{
	interface IWsvLineIterator
	{
		bool HasLine();
		bool IsEmptyLine();
		WsvLine GetLine();
		string[] GetLineAsArray();
		string GetEndKeyword();
		int GetLineIndex();
	}
}