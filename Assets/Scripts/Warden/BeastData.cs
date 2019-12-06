using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Generic;

namespace Warden
{
	public class BeastData : Data
	{
		//Refer to Beast class
		public string Description; //Narrative description
		public List<string> Essences; //1 or 2 essences of the beast
		public Dictionary<string, Stat> Stats; //Base stat values
		public SortedDictionary<int, string> LearnSet; //<level, movedata> to learn at levels

		public string ToStringLong()
		{
			string s = base.ToString();
			s += "\ndataName: " + dataName;
			s += "\nName: " + Name;
			s += "\nDescription: " + Description;
			s += "\nEssences: \n\t";
			s += string.Join("\n\t", Essences);
			s += "\nStats: \n\t";
			s += string.Join("\n\t", Stats);
			s += "\nLearnSet: \n\t";
			s += string.Join("\n\t", LearnSet);
			return s;
		}
	}
}
