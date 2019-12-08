using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generic;

namespace Warden
{
	public class MoveType_BasicDamage : MoveType
	{
		public float BaseDamage; //Before stat adjustments

		public MoveType_BasicDamage(MoveData data, Beast user) : base(data, user)
		{
			BaseDamage = float.Parse(CustomXmlReader.GetContent(data.MoveProps, "BaseDamage"));
		}

		public override void OnUse()
		{
			base.OnUse();
			UnityEngine.Debug.LogFormat("{0} used move {1}. Total attack power is {2} {3}.", User.Name, Name, CalcDamage(BaseDamage), Essence);
			stateInternal = MoveState.Finished;
		}
	}
}
