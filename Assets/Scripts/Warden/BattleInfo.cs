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
		//public List<Beast> OpponentTeam;
		public string Intro; //TODO: Make into dictionary

		public int numPlayerBeasts = 1;
		public int numAllyBeastsPerTeam = 1;
		public int numEnemyBeastsPerTeam = 1;
		public int maxActions = 1;

		public List<Team> EnemyTeams;
		public List<Team> AllyTeams;

		public BattleInfo(List<Beast> beasts)
		{
			Intro = "Fight me";
		}

		public BattleInfo(Team enemies)
		{
			EnemyTeams = new List<Team>
			{
				enemies
			};
			AllyTeams = new List<Team>();
			Intro = "Fight me";
		}
	}
}
