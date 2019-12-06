using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warden
{
	public class Player : IBeastOwner
	{
		private List<Beast> Team;

		public List<Beast> GetTeam()
		{
			return Team;
		}
	}
}
