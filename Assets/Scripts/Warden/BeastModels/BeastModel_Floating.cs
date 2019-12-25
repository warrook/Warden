using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Warden
{
	public class BeastModel_Floating : BeastModel_Basic
	{
		public override void Setup(Beast beast, GameObject holder)
		{
			base.Setup(beast, holder);
			SpriteRenderer r = Leaves.First().GetComponent<SpriteRenderer>();
			Leaves.First().transform.localPosition = new Vector3(0f, 128 * 0.01f); //100 pixels per unit
		}

		protected void Update()
		{
			//Leaves.First().transform.Rotate(Vector3.up, 3f);
			var temp = Leaves.First().transform;
			Leaves.First().transform.localPosition = new Vector3(temp.localPosition.x, temp.localPosition.y + Mathf.Sin(Time.fixedTime * Mathf.PI * 0.75f) * 0.005f, temp.localPosition.z);
			
		}
	}
}
