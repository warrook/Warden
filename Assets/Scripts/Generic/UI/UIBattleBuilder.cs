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
		private BattleMenu menu;
		public BattleMenu Menu
		{
			get => menu;
			set { menu = value; Debug.Log("Set menu to " + value); Build(); }
		}

		private BattleController controller;
		public BattleController Controller => controller;

		private BattleTicker ticker;
		public BattleTicker Ticker => ticker;

		private GameObject OverlayObject;
		private GameObject ChoiceContainer;
		
		private GameObject prefabButton;
		private Canvas canvas;
		private bool built = false;

		private int Spacer => 70;

		public UIBattleBuilder(BattleController battleController)
		{
			controller = battleController;
			Initialize();
			Menu = BattleMenu.Start;
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

			ticker = OverlayObject.AddComponent<BattleTicker>();
			ticker.builder = this;

			GameObject ribbon = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/BattleRibbon"));
			ribbon.transform.SetParent(OverlayObject.transform);
			var ribbonTransform = ribbon.transform as RectTransform;
			//Make vertical sizing fit the content instead
			ribbonTransform.sizeDelta = new Vector2(Screen.width, Screen.height * 0.25f);
		}

		public void Build()
		{
			if (controller.Going)
			{

			}
			else
			{
				if (controller.BattlePhase == BattlePhase.Waiting)
				{
					BuildMenu();
				}
			}
		}

		public void BuildMenu()
		{
			ClearChoiceContainer();
			
			switch (Menu)
			{
				case BattleMenu.Start:
					BuildMenuStart();
					break;
				case BattleMenu.Fight:
					BuildMenuFight();
					break;
				case BattleMenu.Team:
					BuildMenuTeam();
					break;
				default:
					break;
			}
		}

		private void BuildMenuStart()
		{
			int i = 0;
			MakeButton(new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i), "Fight", "UI/BattleChoice_Fight", () =>
			{
				Menu = BattleMenu.Fight;
			});

			i++;

			MakeButton(new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i), "Spells", "UI/BattleChoice_Run", () =>
			{
				Menu = BattleMenu.Spells;
			});

			i++;

			MakeButton(new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i), "Team", "UI/BattleChoice_Run", () =>
			{
				Menu = BattleMenu.Team;
			});
		}

		/// <summary>
		/// Get moves from the beast at the front
		/// </summary>
		private void BuildMenuFight()
		{
			int i = 0;
			foreach (MoveData move in Constants.Player.GetPrimaryMoveSet())
			{
				Vector3 pos = new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i);
				MakeButton(pos, move.Name, "UI/BattleChoice_Fight", () =>
				{
					Menu = BattleMenu.Busy;
					Debug.Log("Hello there, I am " + move.dataName);
					Controller.AddMove(move, Constants.Player.GetTeam().First);
				});
				i++;
			}
		}

		/// <summary>
		/// Team switching, stat-viewing
		/// </summary>
		private void BuildMenuTeam()
		{
			int i = 0;
			foreach (Beast beast in Constants.Player.GetTeam().Members)
			{
				Vector3 pos = new Vector3(Spacer, (Screen.height - Spacer) - Spacer * i);
				MakeButton(pos, beast.Name + " - " + beast.Level, "UI/BattleChoice_Run", () =>
				{
					Menu = BattleMenu.Busy;
					Debug.Log(beast.Name + ": I have been picked!");
					Controller.SwitchBeast(Constants.Player.GetTeam(), Constants.Player.GetTeam().First, beast);
					
				});

				i++;
			}
		}

		private GameObject MakeButton(Vector3 position, string title, string texturePath, Action onChoose)
		{
			GameObject button = GameObject.Instantiate(prefabButton, position, Quaternion.identity, ChoiceContainer.transform);
			button.name = prefabButton.name + "_" + title;
			button.transform.Find("Title").gameObject.GetComponent<Text>().text = title;
			button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(texturePath);
			button.GetComponent<UIBattleButton>().OnChoose = onChoose;
			return button;
		}

		/// <summary>
		/// Checks if a message box already exists, and if it does, change its text to msg.
		/// </summary>
		/// <param name="msg"></param>
		public void MessageBox(string msg)
		{
			Transform t = OverlayObject.transform.Find("MessageBox");
			if (t == null)
			{
				GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/MessageBox");
				GameObject MessageBox = GameObject.Instantiate(prefab, OverlayObject.transform);
				MessageBox.name = "MessageBox";
				t = MessageBox.transform;
			}

			t.Find("TextContent").GetComponent<TextMeshProUGUI>().text = msg;
		}

		private void ClearChoiceContainer()
		{
			for (int i = 0; i < ChoiceContainer.transform.childCount; i++)
			{
				GameObject.Destroy(ChoiceContainer.transform.GetChild(i).gameObject);
			}
		}
	}
}
