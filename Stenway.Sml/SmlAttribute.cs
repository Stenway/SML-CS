using System;
using System.Linq;
using Stenway.Wsv;
namespace Stenway.Sml
{
	public class SmlAttribute : SmlNamedNode
	{
		internal string[] values;
		
		public string[] Values
		{
			get
			{
				return values;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					throw new ArgumentException("Values must contain at least one value");
				}
				this.values = value;
			}
		}
		
		public SmlAttribute(string name, params string[] values) : base(name)
		{
			Values = values;
		}
		
		public SmlAttribute(string name, params object[] values) : base(name)
		{
			SetValues(values);
		}
				
		public SmlAttribute(string name, params byte[][] values) : base(name)
		{
			SetValues(values);
		}
		
		public void SetValues(params string[] values)
		{
			if (values == null || values.Length == 0)
			{
				throw new ArgumentException("Values must contain at least one value");
			}
			this.values = values;
		}
		
		public void SetValues(params object[] values)
		{
			SetValues(values.Select(x => x.ToString()).ToArray());
		}
		
		public void SetValues(params byte[][] values)
		{
			string[] strValues = new string[values.Length];
			for (int i=0; i<values.Length; i++)
			{
				strValues[i] = Convert.ToBase64String(values[i]);
			}
			SetValues(strValues);
		}
				
		public string[] GetValues(int offset)
		{
			return Values.Skip(offset).ToArray();
		}
		
		public int[] GetIntValues()
		{
			return Values.Select(x=>int.Parse(x)).ToArray();
		}
		
		public int[] GetIntValues(int offset)
		{
			return Values.Skip(offset).Select(x=>int.Parse(x)).ToArray();
		}
		
		public float[] GetFloatValues()
		{
			return Values.Select(x=>float.Parse(x)).ToArray();
		}
		
		public float[] GetFloatValues(int offset)
		{
			return Values.Skip(offset).Select(x=>float.Parse(x)).ToArray();
		}
		
		public double[] GetDoubleValues()
		{
			return Values.Select(x=>double.Parse(x)).ToArray();
		}
		
		public double[] GetDoubleValues(int offset)
		{
			return Values.Skip(offset).Select(x=>double.Parse(x)).ToArray();
		}
		
		public bool[] GetBoolValues()
		{
			return Values.Select(x=>bool.Parse(x)).ToArray();
		}
		
		public bool[] GetBoolValues(int offset)
		{
			return Values.Skip(offset).Select(x=>bool.Parse(x)).ToArray();
		}
		
		public byte[][] GetBytesValues()
		{
			return GetBytesValues(0);
		}
		
		public byte[][] GetBytesValues(int offset)
		{
			string[] values = GetValues(offset);
			byte[][] result = new byte[values.Length][];
			for (int i=0; i<values.Length; i++)
			{
				result[i] = Convert.FromBase64String(values[i]);
			}
			return result;
		}
		
		public string GetString()
		{
			return values[0];
		}
		
		public string GetString(int index)
		{
			return values[index];
		}
		
		public int GetInt()
		{
			return GetInt(0);
		}
		
		public int GetInt(int index)
		{
			return int.Parse(values[index]);
		}
		
		public float GetFloat()
		{
			return GetFloat(0);
		}
		
		public float GetFloat(int index)
		{
			return float.Parse(values[index]);
		}
		
		public double GetDouble()
		{
			return GetDouble(0);
		}
		
		public double GetDouble(int index)
		{
			return double.Parse(values[index]);
		}
		
		public bool GetBool()
		{
			return GetBool(0);
		}
		
		public bool GetBool(int index)
		{
			return bool.Parse(values[index]);
		}
		
		public byte[] GetBytes()
		{
			return GetBytes(0);
		}
		
		public byte[] GetBytes(int index)
		{
			return Convert.FromBase64String(values[index]);
		}
		
		public void SetValue(string value)
		{
			SetValues(value);
		}
		
		public void SetValue(object value)
		{
			SetValues(value);
		}
		
		public void SetValue(byte[] value)
		{
			SetValues(value);
		}
		
		public override string ToString()
		{
			return SmlSerializer.SerializeAttribute(this);
		}
		
		internal override void ToWsvLines(WsvDocument document, int level, string defaultIndentation, string endKeyword)
		{
			SmlSerializer.SerializeAttribute(this, document, level, defaultIndentation);
		}
	}
}