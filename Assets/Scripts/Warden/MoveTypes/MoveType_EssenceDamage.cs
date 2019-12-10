using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Warden
{
	public class MoveType_EssenceDamage : MoveType
	{
		//Amount of damage to do, before stat adjustments.
		public float BaseDamage; 

		//Essence of the damage; may be different from move essence.
		public string DamageEssence;
		
		public MoveType_EssenceDamage(MoveData data, Beast user) : base(data, user)
		{
			BaseDamage = float.Parse(Generic.CustomXmlReader.GetContent(data.MoveProps, "BaseDamage"));
			DamageEssence = Generic.CustomXmlReader.GetValidContent(data.MoveProps, "DamageEssence", "Essence");
		}

		public override void OnUse()
		{
			Debug.LogFormat("{0} used move {1}. Base damage is {2} {3}.", User.Name, Name, BaseDamage, DamageEssence);
		}
	}
}
