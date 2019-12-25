using System;
using System.Collections;
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
		private BattlePhase battlePhase;
		public BattlePhase BattlePhase
		{
			get => battlePhase;
			set
			{
				battlePhase = value;
				Proceed();
			}
		}

		private bool go;
		public bool Going => go;

		private BattleInfo battleInfo;
		private GameObject battleHolder;
		private UIBattleBuilder ui;
		private BattleTicker ticker;

		private List<MoveType> activeMoves = new List<MoveType>(); //Active MoveType objects going on.
		private List<MoveType> waitingMoves = new List<MoveType>(); //MoveType objects that have been selected and are waiting to be added to Active on the next phase
		private List<MoveType> endingMoves = new List<MoveType>(); //MoveType objects that are done, need to call OnEnd(), and be deleted

		public BattleInfo BattleInfo => battleInfo;

		public BattleController()
		{
			var team = new Team()
				.AddByName("Warden.Wisp", 5)
				.AddByName("Warden.FoolsFlame", 6);
			battleInfo = new BattleInfo(team);
			ui = new UIBattleBuilder(this);
			ticker = ui.Ticker;
			battleInfo.numEnemyBeastsPerTeam = 2;
			BattlePhase = BattlePhase.BattleStart;
			//EnterBeasts();
		}

		public BattleController(BattleInfo info)
		{
			battleInfo = info;
			ui = new UIBattleBuilder(this);
		}

		public void Go() => go = true;
		public void Wait() => go = false;

		private IEnumerator WaitFor(float sec)
		{
			go = false;
			yield return new WaitForSeconds(sec);
			go = true;
		}

		private IEnumerator WaitForAnd(float sec, BattlePhase phase)
		{
			yield return WaitFor(sec);
			BattlePhase = phase;
		}

		private void Proceed() //Might need to be public?
		{
			switch (BattlePhase)
			{
				case BattlePhase.BattleStart:
					ui.MessageBox("BEGIN");
					EnterBeasts();
					ticker.StartCoroutine(WaitForAnd(Constants.WaitMedium + 1f, BattlePhase.RoundStart));
					//BattlePhase = BattlePhase.RoundStart;
					break;
				case BattlePhase.RoundStart:
					ui.MessageBox("START ROUND");
					ticker.StartCoroutine(WaitForAnd(Constants.WaitMedium, BattlePhase.Waiting));
					//BattlePhase = BattlePhase.Waiting;
					break;
				case BattlePhase.Waiting:
					ui.MessageBox("CHOOSE!");
					Wait();
					ui.Build();
					break;
			}
		}

		/// <summary>
		/// Make the models for each beast, 
		/// </summary>
		private void EnterBeasts()
		{
			battleHolder = new GameObject("BattleHolder");
			int beasts = 0;
			Vector3 pos;
			Transform model;

			//Player beasts
			for (int i = 0; i < BattleInfo.numPlayerBeasts; i++)
			{
				beasts++;
				pos = new Vector3(-1f - (1.5f * beasts), 0, -0.02f * beasts);
				model = BuildModel(Constants.Player.GetTeam().Members[i], battleHolder, pos);
				ticker.StartCoroutine(EnterBeast(model, pos));
			}

			//Ally beasts
			if (BattleInfo.AllyTeams != null)
			{
				for (int i = 0; i < BattleInfo.AllyTeams.Count; i++)
					for (int j = 0; j < BattleInfo.numAllyBeastsPerTeam; j++)
					{
						beasts++;
						pos = new Vector3(-1f - (1.5f * beasts), 0, -0.02f * beasts);
						model = BuildModel(BattleInfo.AllyTeams[i].Members[j], battleHolder, pos);
						ticker.StartCoroutine(EnterBeast(model, pos));
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
						pos = new Vector3(1f + (1.5f * beasts), 0, -0.02f * beasts);
						model = BuildModel(BattleInfo.EnemyTeams[i].Members[j], battleHolder, pos);
						ticker.StartCoroutine(EnterBeast(model, pos));
					}
			}
		}

		private void SwitchBeast(Beast beastOut, Beast beastIn)
		{

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

		private Transform BuildModel(Beast beast, GameObject parent, Vector3 targetPosition)
		{
			int dir = Math.Sign(0 + targetPosition.x);
			BeastData data = beast.data;
			GameObject pivot = new GameObject(data.dataName, Type.GetType(data.ModelClass));
			pivot.transform.SetParent(parent.transform);
			pivot.GetComponent<BeastModel>().Setup(beast, parent);
			pivot.transform.position = new Vector3(targetPosition.x + (6f * dir), targetPosition.y, targetPosition.z);

			//Flip depending on direction (this might not work well for complex models)
			pivot.transform.eulerAngles = new Vector3(0, (90f * (1 + -dir)), 0);

			return pivot.transform;
		}

		private IEnumerator EnterBeast(Transform model, Vector3 targetPosition)
		{
			Wait();

			float elapsedTime = 0;
			float startX = model.position.x;
			float endX = targetPosition.x;
			
			while (elapsedTime < Constants.WaitMedium + 1f/*Vector3.Distance(model.position, targetPosition) >= 0.01f*/)
			{
				model.position = new Vector3(Mathf.Lerp(model.position.x, endX, elapsedTime * 0.04f), targetPosition.y, targetPosition.z);
				elapsedTime += Time.deltaTime;
				//Debug.Log(elapsedTime);
				yield return new WaitForEndOfFrame();
			}
			model.position = targetPosition;

			Go();
			yield return null;
			
		}
	}
}
