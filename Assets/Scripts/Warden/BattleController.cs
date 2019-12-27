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
		private int turnActions;

		public BattleController()
		{
			var team = new Team()
				.AddNewByName("Warden.Wisp", 5)
				.AddNewByName("Warden.FoolsFlame", 6);
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

		public void Go()
		{
			Debug.Log("Go");
			go = true;
		}

		public void Wait()
		{
			Debug.Log("Wait");
			go = false;
		}

		private IEnumerator WaitFor(float sec)
		{
			Wait();
			yield return new WaitForSeconds(sec);
			Go();
		}

		private IEnumerator WaitForAnd(float sec, BattlePhase phase)
		{
			yield return WaitFor(sec);
			BattlePhase = phase;
		}

		private void Proceed() //Might need to be public?
		{
			if (ui.Menu == BattleMenu.Busy && BattlePhase == BattlePhase.Waiting)
			{
				BattlePhase = BattlePhase.TurnPre;
				return;
			}

			//TODO: Modify for allowing things to be true during a phase separate from phase transitions
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
					turnActions = BattleInfo.maxActions;
					Wait();
					ui.Menu = BattleMenu.Start;
					break;
				default:
					ui.MessageBox("BEHOLD! " + BattlePhase);
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

		public void SwitchBeast(Team team, Beast beastOut, Beast beastIn)
		{
			if (team.Switch(beastOut, beastIn))
			{
				BeastModel[] models = battleHolder.transform.GetComponentsInChildren<BeastModel>();

				Transform modelOut = models.Where(b => b.Beast == beastOut).First().gameObject.transform;
				Vector3 pos = modelOut.position;

				Transform modelIn = BuildModel(beastIn, battleHolder, pos);

				ticker.StartCoroutine(SwapBeast(modelOut, modelIn));
				turnActions--;
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

		private Transform BuildModel(Beast beast, GameObject parent, Vector3 targetPosition)
		{
			int dir = Math.Sign(0 + targetPosition.x);
			BeastData data = beast.data;
			GameObject pivot = new GameObject(data.dataName, Type.GetType(data.ModelClass));
			pivot.transform.SetParent(parent.transform);
			pivot.GetComponent<BeastModel>().Setup(beast, parent);
			pivot.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

			//Flip depending on direction (this might not work well for complex models)
			pivot.transform.eulerAngles = new Vector3(0, (90f * (1 + -dir)), 0);

			return pivot.transform;
		}

		private IEnumerator EnterBeast(Transform model, Vector3 targetPosition)
		{
			//Debug.Log("Enter called");

			float elapsedTime = 0;
			model.position = new Vector3(model.position.x + (6f * Math.Sign(0 + targetPosition.x)), model.position.y, model.position.z); //this is just weird
			float endX = targetPosition.x;

			while (elapsedTime < Constants.WaitMedium + 1f)
			{
				model.position = new Vector3(Mathf.Lerp(model.position.x, endX, elapsedTime * 0.04f), targetPosition.y, targetPosition.z);
				elapsedTime += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			model.position = targetPosition;
			yield return null;
		}

		private IEnumerator ExitBeast(Transform modelOut, Transform modelIn)
		{
			modelIn.gameObject.SetActive(false);
			float elapsedTime = 0;
			Vector3 pos = modelOut.position;

			float mini;

			while (elapsedTime < Constants.WaitShort)
			{
				mini = Mathf.Lerp(modelOut.localScale.x, 0, elapsedTime / Constants.WaitShort);
				modelOut.localScale = new Vector3(mini, mini, mini);
				elapsedTime += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			GameObject.Destroy(modelOut.gameObject);
			modelIn.gameObject.SetActive(true);
			yield return null;
		}

		private IEnumerator SwapBeast(Transform modelOut, Transform modelIn)
		{
			if (!Going)
			{
				Go();
				Vector3 pos = modelOut.position;
				yield return ExitBeast(modelOut, modelIn);
				yield return EnterBeast(modelIn, pos);
			}
			Wait();
		}
	}
}
