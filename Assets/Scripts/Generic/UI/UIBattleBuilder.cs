using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Warden;

namespace Generic.UI
{
	public class UIBattleBuilder
	{
		private BattleController controller;
		public BattleController Controller => controller;

		private GameObject OverlayObject;
		private GameObject ChoiceContainer;
		private GameObject Ticker;
		private GameObject prefabButton;
		private Canvas canvas;

		private int Spacer => 70;

		public UIBattleBuilder(BattleController battleController)
		{
			controller = battleController;
			Initialize();
		}

		private void Initialize()
		{
			prefabButton = Resources.Load<GameObject>("Prefabs/UI/BattleButton");

			OverlayObject = new GameObject("UI_Overlay", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
			canvas = OverlayObject.GetComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.pixelPerfect = true;

			ChoiceContainer = new GameObject("UI_ChoiceContainer");
			ChoiceContainer.transform.SetParent(OverlayObject.transform);

			Ticker = new GameObject("Ticker");
			Ticker.transform.SetParent(OverlayObject.transform);
			Ticker.AddComponent<BattleTicker>().builder = this;

			//Build();
		}

		public void Build()
		{
			ClearChoiceContainer();

			BattleController.Choice choice = Controller.State;
			switch (choice)
			{
				case BattleController.Choice.Start:
					BuildStart();
					break;
				case BattleController.Choice.Fight:
					BuildFight();
					break;
				default:
					break;
			}
		}

		private void BuildStart()
		{
			int i = 0;
			MakeButton(new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i), "Fight", "UI/BattleChoice_Fight", () =>
			{
				Controller.State = BattleController.Choice.Fight;
			}).transform.SetParent(ChoiceContainer.transform);

			i++;

			MakeButton(new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i), "Spells", "UI/BattleChoice_Run", () =>
			{
				Controller.State = BattleController.Choice.Spells;
			}).transform.SetParent(ChoiceContainer.transform);
		}

		private void BuildFight()
		{
			int i = 0;
			foreach (MoveData move in Constants.Player.GetPrimaryMoveSet())
			{
				Vector3 pos = new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i);
				MakeButton(pos, move.Name, "UI/BattleChoice_Fight", () =>
				{
					Debug.Log("Hello there, I am " + move.dataName);
					Controller.AddMove(move, Constants.Player.GetTeam().First());
				});
				i++;
			}

			//Debug.Log(string.Join(",", Constants.Player.GetTeam().First().MoveSet));
			//for (int i = 0; i < Constants.Player.GetPrimaryMoveSet().Count; i++)
			//{
			//	List<MoveData> moves = Constants.Player.GetPrimaryMoveSet();
			//	Vector3 pos = new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i);
			//	MakeButton(pos, moves[i].Name, "UI/BattleChoice_Fight", () =>
			//	{
			//		Debug.Log("Hello there, I am " + moves[i].dataName);
			//	});
			//}
		}

		private GameObject MakeButton(Vector3 position, string title, string texturePath, Action onChoose)
		{
			GameObject button = GameObject.Instantiate(prefabButton, position, Quaternion.identity, OverlayObject.transform);
			button.name = prefabButton.name + "_" + title;
			button.transform.Find("Title").gameObject.GetComponent<Text>().text = title;
			button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(texturePath);
			button.GetComponent<UIBattleButton>().OnChoose = onChoose;
			return button;
		}

		private void ClearChoiceContainer()
		{
			for (int i = 0; i < ChoiceContainer.transform.childCount; i++)
			{
				GameObject.Destroy(ChoiceContainer.transform.GetChild(i).gameObject);
			}
		}

		public void MessageBox(string msg)
		{
			//Controller.State = BattleController.Choice.Dialogue;

			GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/MessageBox");
			GameObject MessageBox = GameObject.Instantiate(prefab, OverlayObject.transform);
			TextMeshProUGUI tmp = MessageBox.transform.Find("TextContent").GetComponent<TextMeshProUGUI>();
			tmp.text = msg;
		}
	}
}
