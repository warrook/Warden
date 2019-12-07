using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warden
{
	public interface IBeastOwner
	{
		List<Beast> GetTeam();
		List<MoveData> GetPrimaryMoveSet();
	}
}
