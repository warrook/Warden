using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic;

namespace Warden
{
	public class Player : IBeastOwner
	{
		//private List<Beast> Team;

		//public List<Beast> GetTeam() => Team;
		//public List<MoveData> GetPrimaryMoveSet() => GetTeam().First().MoveSet;

		private Team Team;
		public Team GetTeam() => Team;
		public List<MoveData> GetPrimaryMoveSet() => GetTeam().Members.First().MoveSet;

		public Player SetDefault()
		{
			Team = new Team()
				.AddNewByName("Warden.Wisp", 5)
				.AddNewByName("Warden.Mote", 2);
			return this;
		}
	}
}
