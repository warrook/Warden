using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Generic.UI
{
	public class BattleUIMaker : MonoBehaviour
	{
		private GameObject BattleMaker;
		private GameObject OverlayObject;
		private GameObject ChoiceHolder;
		private Canvas canvas;

		public GameObject prefabChoice;

		private void Start()
		{
			BattleMaker = this.gameObject;
			prefabChoice = Resources.Load<GameObject>("Prefabs/UI/BattleChoice");

			OverlayObject = new GameObject("Overlay", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
			OverlayObject.transform.SetParent(BattleMaker.transform);
			canvas = OverlayObject.GetComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.pixelPerfect = true;

			BuildChoices(4);
		}

		//This handles prefab instantiation -- also needs to get move information
		private void SetChoice(Vector3 location, string title, string texturePath)
		{
			GameObject button = Instantiate(prefabChoice, location, Quaternion.identity, ChoiceHolder.transform);
			button.name = prefabChoice.name + "_" + title;
			//button.transform.SetParent(OverlayObject.transform);
			button.transform.Find("Title").gameObject.GetComponent<Text>().text = title;
			button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(texturePath);
			button.GetComponent<BattleChoice>().Initialize(title);
		}

		//This handles positioning
		public void BuildChoices(params[] numChoices)
		{
			ChoiceHolder = new GameObject("UI_ChoiceHolder");
			ChoiceHolder.transform.SetParent(OverlayObject.transform);
			for (int i = 0; i < 4; i++)
			{
				int spacer = 70;
				Vector3 pos = new Vector3(spacer, (Screen.height - spacer) - spacer * i, 0);
				switch (i)
				{
					case 0:
						SetChoice(pos, "Attack", "UI/BattleChoice_Fight");
						break;
					case 1:
						SetChoice(pos, "Run", "UI/BattleChoice_Run");
						break;
					case 2:
						SetChoice(pos, "RunFaster", "UI/BattleChoice_Run");
						break;
					case 3:
						SetChoice(pos, "AttackHarder", "UI/BattleChoice_Fight");
						break;

				}
			}
		}
	}
}
