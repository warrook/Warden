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
		
		public bool Contains(Beast beast) => Members.Contains(beast);
		public Beast First => Members.First();

		/// <summary>
		/// Adds a new beast to team by dataName (with pack name)
		/// </summary>
		/// <param name="dataName"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public Team AddNewByName(string dataName, int level)
		{
			AddNewByData(Database<BeastData>.GetByName(dataName), level);
			return this;
		}

		/// <summary>
		/// Adds a new beast to team by BeastData object
		/// </summary>
		/// <param name="data"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public Team AddNewByData(BeastData data, int level)
		{
			Members.Add(new Beast(data, level));
			return this;
		}

		/// <summary>
		/// Add an existing beast to team
		/// </summary>
		/// <param name="beast"></param>
		/// <returns></returns>
		public Team Add(Beast beast)
		{
			if (!Contains(beast))
				Members.Add(beast);
			else
				UnityEngine.Debug.Log("Tried to add duplicate beast to team");
			return this;
		}

		//Add

		/// <summary>
		/// Moves the given beast to the front of the team.
		/// </summary>
		/// <param name="beast"></param>
		public void MoveToFront(Beast beast)
		{
			UnityEngine.Debug.LogFormat("Moving {0} to front of team", beast.data.dataName);
			if (Members.Contains(beast))
			{
				Members.Remove(beast);
				Members.Insert(0, beast);
			}
		}

		//Untested
		public bool Switch(Beast beastOut, Beast beastIn)
		{
			//UnityEngine.Debug.LogFormat("Switching out {0} for {1}", beastOut.data.dataName, beastIn.data.dataName);
			if (!beastIn.Equals(beastOut) && Contains(beastOut) && Contains(beastIn))
			{
				int moveTo = Members.IndexOf(beastOut);
				int moveFrom = Members.IndexOf(beastIn);

				Members.Remove(beastOut);
				Members.Remove(beastIn);
				Members.Insert(moveTo, beastIn);
				Members.Insert(moveFrom, beastOut);
				return true;
			}
			else
			{
				UnityEngine.Debug.LogFormat("Failed to switch {0} with {1}", beastOut, beastIn);
				return false;
			}
		}
	}
}
