using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Generic;

namespace Warden
{
	public class Beast
	{
		public BeastData data;

		private int levelInternal;
		private float expInternal;
		public int Level => levelInternal; //Zero-based level
		public float Exp => expInternal; //Total experience points

		public string Name; //Display name of the beast
		public Dictionary<string, Stat> Stats; //Instanced copy of data stats
		public float CurrentHP;
		public float MaxHP;
		//TODO: Resistances

		public List<MoveData> MoveSet; //The current loadout
		public List<MoveData> KnownMoves = new List<MoveData>(); //All learned moves, including trained

		public Beast(BeastData data, int level)
		{
			this.data = data;
			Name = data.Name;
			Stats = data.Stats.ToDictionary(
				entry => entry.Key,
				entry => entry.Value.Instantiate()
				);
			LevelUp(level);
		}

		public MoveType InstantiateKnownMove(MoveData move)
		{
			if (KnownMoves.Contains(move))
			{
				MoveType t = (MoveType)Activator.CreateInstance(Type.GetType(move.MoveType), move, this);
				t.OnUse();
				return t;
			}
			return null;
		}

		public void LevelUp()
		{
			levelInternal++;
			MakeStatsCurrent();
		}

		public void LevelUp(int level)
		{
			this.levelInternal = level; //Calculate needed experience
			MakeStatsCurrent();
		}

		private void MakeStatsCurrent()
		{
			foreach (KeyValuePair<string, Stat> pair in Stats)
				pair.Value.MakeCurrent(levelInternal);
			MaxHP = (float)Math.Round(Stats["Stat_Vitality"].Current + levelInternal, 2);
			CurrentHP = MaxHP;
			LearnMovesForLevel();
		}

		private void LearnMovesForLevel()
		{
			foreach (KeyValuePair<int, string> pair in data.LearnSet)
			{
				MoveData move = Database<MoveData>.GetByName(pair.Value);
				//Debug.Log(string.Format("Trying to add move {0} to {1}'s known moves.", move.dataName, Name));
				if (levelInternal >= pair.Key)
				{
					if (!KnownMoves.Contains(move))
						KnownMoves.Add(move);
				}
				else
					break;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} uses BeastData: {1}. Currently {2} with {3} HP.", Name, data.dataName, Level, Mathf.Round(MaxHP));
		}
	}
}
