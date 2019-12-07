using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warden
{
	/// <summary>
	/// Contains information of encounter weather, the stage to use, etc
	/// </summary>
	public class BattleInfo
	{
		public List<Beast> OpponentTeam;
		public string Intro; //TODO: Make into dictionary

		public BattleInfo(List<Beast> beasts)
		{
			OpponentTeam = beasts;
			Intro = "Fight me";
		}
	}
}
