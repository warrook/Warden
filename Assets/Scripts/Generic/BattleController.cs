using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warden;
using UnityEngine;

namespace Generic
{
	/* Phases: ---------------------
	 * RoundStart
	 * [Wait on: Move selection]
	 * First beast's Pre Use Post
	 * Second beast's Pre Use Post
	 * RoundEnd
	 * BattleEnd
	 */
	public class BattleController : MonoBehaviour
	{
		private enum InteractState
		{
			Idle, //Waiting for player
			Busy //Stuff is happening
		}

		//An array of the teams involved in the battle. The first entry must be the player's.
		//The first beast of each team is the beast currently on the field.
		public List<Beast>[] Teams;
		private List<MoveType> activeMoves; //Active MoveTypes going on. They have all the needed info; this blindly calls them.
		private List<MoveType> waitingMoves; //MoveTypes that have been chosen and are waiting for QueueMoves so they can be used.
		private List<MoveType> endingMoves;
		private InteractState state;

		public BattleController(List<Beast> playerTeam, List<Beast> opponentTeam)
		{
			Teams = new List<Beast>[]
			{
				playerTeam,
				opponentTeam
			};
		}

		private void Start()
		{
			state = InteractState.Idle;
		}

		private void Update()
		{
			if (state == InteractState.Idle)
			{
				//Choose move (how?)
				//Gotta make visuals right now I think so I can figure it out
			}
		}

		private void ActivateMoves()
		{
			foreach (MoveType move in activeMoves)
			{
				if (move.Flag == -1)
				{
					endingMoves.Add(move);
					activeMoves.Remove(move);
				}
				if (waitingMoves.Contains(move) == false && activeMoves.Contains(move))
					waitingMoves.Add(move);
			}

			activeMoves.Clear();
			foreach (MoveType move in waitingMoves)
				activeMoves.Add(move);
			activeMoves.Sort();
			waitingMoves.Clear();
		}

		private void EndNextMove()
		{
			if (endingMoves.Count == 0)
				return;
			MoveType move = endingMoves.First();
			move.OnEnd();
			endingMoves.Remove(endingMoves.First());
		}
	}
}
