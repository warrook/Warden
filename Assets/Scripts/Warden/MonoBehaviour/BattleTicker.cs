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
		public UIBattleBuilder builder;
		private BattleController controller => builder.Controller;
		private BattleController.Choice choice;

		private void Start()
		{
			choice = controller.State;
			builder.Build();
		}

		private void Update()
		{
			if (choice != builder.Controller.State)
			{
				choice = controller.State;
				builder.Build();
			}
		}
	}
}
