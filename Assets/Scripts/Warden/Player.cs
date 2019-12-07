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
		private List<Beast> Team;

		public List<Beast> GetTeam() => Team;
		public List<MoveData> GetPrimaryMoveSet() => GetTeam().First().MoveSet;

		public Player SetDefault()
		{
			//UnityEngine.Debug.LogWarning("Initializing player team");
			Team = new List<Beast>()
			{
				new Beast(Database<BeastData>.GetByName("Warden.WillOWisp"), 10)
			};
			return this;
		}
	}
}
