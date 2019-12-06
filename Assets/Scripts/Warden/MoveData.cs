using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Generic;

namespace Warden
{
	public class MoveData : Data
	{
		//Refer to MoveType class
		public string Essence;
		public string Focus;
		public string Stat;
		public string MoveType;
		public XmlElement MoveProps;

		public string ToStringLong()
		{
			string s = base.ToString();
			s += "\ndataName: " + dataName;
			s += "\nName: " + Name;
			s += "\nEssence: " + Essence;
			s += "\nFocus: " + Focus;
			s += "\nMoveType: " + MoveType;
			s += "\nMoveProps: " + MoveProps.InnerXml;
			return s;
		}
	}
}
