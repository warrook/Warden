﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Warden;
using Generic.UI;

namespace Warden
{
	public class BattleTicker : MonoBehaviour
	{
		private bool forceUpdate = false;
		public UIBattleBuilder builder;
		private BattleController controller => builder.Controller;
		private BattleMenu currentMenu;

		public void ForceUpdate()
		{
			forceUpdate = true;
		}

		private void Start()
		{
			currentMenu = builder.Menu;
			ForceUpdate();
			controller.Go();
			//StartCoroutine(controller.WaitFor(Constants.WaitMedium));
		}

		private void Update()
		{
			//controller.Update();
			//builder.Build(); //Run once

			//Move to Coroutine?
			//if (controller.BattlePhase == BattlePhase.Waiting)
			//{
			//	//Menuing
			//	if (builder.Menu == BattleMenu.Busy)
			//	{
			//		//Don't do any menu thing.
			//	}
			//	else if (currentMenu != builder.Menu || forceUpdate)
			//	{
			//		forceUpdate = false;
			//		currentMenu = builder.Menu;
			//		builder.BuildMenu();
			//	}
			//}
		}
	}
}
