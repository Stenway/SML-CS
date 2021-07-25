using Stenway.Wsv;
namespace Stenway.Sml
{
	public abstract class SmlNode
	{
		internal string[] whitespaces;
		internal string comment;
		
		public string Comment
		{
			get
			{
				return comment;
			}
			set
			{
				WsvLine.ValidateComment(value);
				this.comment = value;
			}
		}
		
		
		public string[] Whitespaces
		{
			get
			{
				if (whitespaces == null)
				{
					return null;
				}
				return (string[])whitespaces.Clone();
			}
			set
			{
				WsvLine.ValidateWhitespaces(value);
				this.whitespaces = value;
			}
		}
		
		public void SetWhitespaces(params string[] whitespaces)
		{
			Whitespaces = whitespaces;
		}
		
		internal void SetWhitespacesAndComment(string[] whitespaces, string comment) {
			this.whitespaces = whitespaces;
			this.comment = comment;
		}
		
		internal abstract void ToWsvLines(WsvDocument document, int level, string defaultIndentation, string endKeyword);
			
		public virtual void Minify() {
			whitespaces = null;
			comment = null;
		}
	}
}