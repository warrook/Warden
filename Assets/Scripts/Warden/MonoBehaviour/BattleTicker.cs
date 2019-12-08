using System;
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
		}

		private void Update()
		{
			if (controller.BattlePhase == BattlePhase.Waiting)
			{
				if (builder.Menu == BattleMenu.Busy)
				{
					//Don't do any menu thing.
				}
				else if (currentMenu != builder.Menu || forceUpdate)
				{
					currentMenu = builder.Menu;
					builder.BuildMenu();
				}
			}
		}
	}
}
