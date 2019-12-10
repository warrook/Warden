using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic.UI;
using UnityEngine;

namespace Warden
{
	/// <summary>
	/// Responsible for receiving and delivering information about the battle. A new instance is created per battle.
	/// </summary>
	public class BattleController
	{
		private BattlePhase battlePhaseInternal;
		public BattlePhase BattlePhase => battlePhaseInternal;

		private BattleInfo battleInfo;
		private UIBattleBuilder ui;
		private List<Beast> Participants;

		private List<MoveType> activeMoves = new List<MoveType>(); //Active MoveType objects going on.
		private List<MoveType> waitingMoves = new List<MoveType>(); //MoveType objects that have been selected and are waiting to be added to Active on the next phase
		private List<MoveType> endingMoves = new List<MoveType>(); //MoveType objects that are done, need to call OnEnd(), and be deleted

		public BattleInfo BattleInfo => battleInfo;
		//public List<Beast> OpponentTeam => BattleInfo.OpponentTeam;
		//public List<IBeastOwner> Owners = new List<IBeastOwner>();

		public BattleController()
		{
			var team = new Team()
				.AddByName("Warden.Wisp", 5)
				.AddByName("Warden.FoolsFlame", 6);
			battleInfo = new BattleInfo(team);
			ui = new UIBattleBuilder(this);
			battlePhaseInternal = BattlePhase.Waiting;
		}

		public BattleController(BattleInfo info)
		{
			battleInfo = info;
			ui = new UIBattleBuilder(this);
		}

		//Move done moves to the ending list
		private void EndMoves()
		{
			foreach (MoveType move in activeMoves)
			{
				if (move.State == MoveState.Finished)
				{
					activeMoves.Remove(move);
					endingMoves.Add(move);
				}
			}
		}

		//Move waiting moves to active list, clear waiting
		private void ActivateMoves()
		{
			activeMoves.Clear();
			foreach (MoveType move in waitingMoves)
				activeMoves.Add(move);
			activeMoves.Sort();
			waitingMoves.Clear();
		}

		//Add move to waiting list
		public void AddMove(MoveData data, Beast user)
		{
			MoveType move = (MoveType)Activator.CreateInstance(Type.GetType(data.MoveType), data, user);
			waitingMoves.Add(move);
			Debug.Log(string.Join(",", waitingMoves));
		}
	}
}
