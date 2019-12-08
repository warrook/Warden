using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic;

namespace Warden
{
	/// <summary>
	/// Holder class for teams of beasts.
	/// </summary>
	public class Team
	{
		public List<Beast> Members = new List<Beast>();

		public Beast First()
		{
			return Members.First();
		}

		/// <summary>
		/// Adds beast to team by dataName (with pack name)
		/// </summary>
		/// <param name="dataName"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public Team AddByName(string dataName, int level)
		{
			AddByData(Database<BeastData>.GetByName(dataName), level);
			return this;
		}

		/// <summary>
		/// Adds beast to team by BeastData object
		/// </summary>
		/// <param name="data"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public Team AddByData(BeastData data, int level)
		{
			Members.Add(new Beast(data, level));
			return this;
		}

		/// <summary>
		/// Moves the given beast to the front of the team.
		/// </summary>
		/// <param name="beast"></param>
		public void MoveToFront(Beast beast)
		{
			if (Members.Contains(beast))
			{
				Members.Remove(beast);
				Members.Insert(0, beast);
			}
		}

		//Untested
		public void Switch(Beast beastOut, Beast beastIn)
		{
			if (Members.Contains(beastOut) && Members.Contains(beastIn))
			{
				int moveTo = Members.IndexOf(beastOut);
				int moveFrom = Members.IndexOf(beastIn);

				Members.Remove(beastOut);
				Members.Remove(beastIn);
				Members.Insert(moveTo, beastIn);
				Members.Insert(moveFrom, beastOut);
			}
		}
	}
}
