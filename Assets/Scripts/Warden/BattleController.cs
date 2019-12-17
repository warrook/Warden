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
		private GameObject battleHolder;
		private UIBattleBuilder ui;

		private List<MoveType> activeMoves = new List<MoveType>(); //Active MoveType objects going on.
		private List<MoveType> waitingMoves = new List<MoveType>(); //MoveType objects that have been selected and are waiting to be added to Active on the next phase
		private List<MoveType> endingMoves = new List<MoveType>(); //MoveType objects that are done, need to call OnEnd(), and be deleted

		public BattleInfo BattleInfo => battleInfo;

		public BattleController()
		{
			var team = new Team()
				.AddByName("Warden.FoolsFlame", 5)
				.AddByName("Warden.FoolsFlame", 6);
			battleInfo = new BattleInfo(team);
			ui = new UIBattleBuilder(this);
			battlePhaseInternal = BattlePhase.BattleStart;
			battleInfo.numEnemyBeastsPerTeam = 2;
			EnterBeasts();
		}

		public BattleController(BattleInfo info)
		{
			battleInfo = info;
			ui = new UIBattleBuilder(this);
		}

		public void Proceed()
		{
			
		}

		private void EnterBeasts()
		{
			battleHolder = new GameObject("BattleHolder");
			int beasts = 0;

			//Player beasts
			for (int i = 0; i < BattleInfo.numPlayerBeasts; i++)
			{
				beasts++;
				MakeModel(Constants.Player.GetTeam().Members[i], false, battleHolder)
					.localPosition = new Vector3(-1f - (1.5f * beasts), 0, -0.02f * beasts);
			}

			//Ally beasts
			if (BattleInfo.AllyTeams != null)
			{
				for (int i = 0; i < BattleInfo.AllyTeams.Count; i++)
					for (int j = 0; j < BattleInfo.numAllyBeastsPerTeam; j++)
					{
						beasts++;
						MakeModel(BattleInfo.AllyTeams[i].Members[j], false, battleHolder)
							.localPosition = new Vector3(-1f - (1.5f * beasts), 0, -0.02f * beasts);
					}
						
			}

			beasts = 0;

			//Enemy beasts
			if (BattleInfo.EnemyTeams != null)
			{
				for (int i = 0; i < BattleInfo.EnemyTeams.Count; i++)
					for (int j = 0; j < BattleInfo.numEnemyBeastsPerTeam; j++)
					{
						beasts++;
						MakeModel(BattleInfo.EnemyTeams[i].Members[j], true, battleHolder)
							.localPosition = new Vector3(1f + (1.5f * beasts), 0, -0.02f * beasts);
					}
			}
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

		private Transform MakeModel(Beast beast, bool isEnemy, GameObject parent)
		{
			BeastData data = beast.data;
			GameObject pivot = new GameObject(data.dataName, Type.GetType(data.ModelClass));
			pivot.transform.SetParent(parent.transform);
			pivot.GetComponent<BeastModel>().Setup(data.ModelProps, parent);
			return pivot.transform;
		}
	}
}
