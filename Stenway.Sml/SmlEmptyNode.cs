using Stenway.Wsv;
namespace Stenway.Sml
{
	public class SmlEmptyNode : SmlNode
	{
		public override string ToString()
		{
			return SmlSerializer.SerializeEmptyNode(this);
		}
		
		internal override void ToWsvLines(WsvDocument document, int level, string defaultIndentation, string endKeyword)
		{
			SmlSerializer.SerializeEmptyNode(this, document, level, defaultIndentation);
		}
	}
}