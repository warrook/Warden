using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Warden
{
	public class Stat
	{
		public float StatBase;
		public string Grade;
		public float Current;

		public Stat(float statbase, string grade)
		{
			StatBase = statbase;
			Grade = grade;
			Current = CalcStatAtLevel(0);
		}

		public void MakeCurrent(int level)
		{
			Current = CalcStatAtLevel(level);
			//Debug.Log("Stat made current for level " + level + ": " + this.ToString());
		}

		public float CalcStatAtLevel(int level)
		{
			float f = StatBase + (StatBase * level * 0.5f) + Constants.ScaleFactor[Grade] * level;
			return f;
		}

		public Stat Instantiate()
		{
			return new Stat(StatBase, Grade);
		}

		public override string ToString()
		{
			return string.Format("{2} ({0},{1})", StatBase, Grade, Current);
		}
	}
}
