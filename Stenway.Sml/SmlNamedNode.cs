using System;
namespace Stenway.Sml
{
	public abstract class SmlNamedNode : SmlNode
	{
		private string name;
		
		public string Name
		{
			get
			{
				return name;			
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Name cannot be null");
				}
				this.name = value;
			}
		}
		
		protected SmlNamedNode(string name)
		{
			Name = name;
		}
		
		public bool HasName(string name)
		{
			return String.Equals(this.name, name, StringComparison.OrdinalIgnoreCase);
		}
	}
}