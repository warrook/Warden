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
		private Vector3 startPos;

		void Awake()
		{
			startPos = new Vector3(0f, 128 * 0.01f);
		}
		
		public override void Setup(Beast beast, GameObject holder)
		{
			base.Setup(beast, holder);
			SpriteRenderer r = Leaves.First().GetComponent<SpriteRenderer>();

			Leaves.First().transform.localPosition = startPos; //100 pixels per unit
		}

		protected void Update()
		{
			var temp = Leaves.First().transform;

			//current time(to sync it up with others) * pi * frequency(higher is faster) * amplitude
			float y = startPos.y + Mathf.Sin(Time.fixedTime * Mathf.PI * 1f) * 0.05f;
			Leaves.First().transform.localPosition = new Vector3(temp.localPosition.x, y, temp.localPosition.z);
		}
	}
}
