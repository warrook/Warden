using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Generic.UI
{
	public class BattleChoice : MonoBehaviour, IPointerClickHandler
	{
		public void Initialize(string select)
		{
			Type t = Type.GetType("Generic.UI.BattleChoice_" + select);
			if (t != null)
				this.gameObject.AddComponent(t);
			else
				Debug.Log("Unable to find BattleChoice_" + select);
		}

		void Update()
		{
			float distance = Vector2.Distance(this.transform.position, Input.mousePosition);
			float radius = this.GetComponentInChildren<Image>().mainTexture.width * 0.5f + 2;
			string s = "Hmm";
			if (distance < radius + 10f)
			{
				//s = string.Format("{0} < {1}", distance, radius + 10f);
				this.transform.Find("Background").GetComponent<Image>().color = new Color(1.0f, 0.75f, 0.75f);

				if (distance < radius)
				{
					//s = "Hello" + string.Format(" {0} < {1}", distance, radius);
					this.transform.Find("Background").GetComponent<Image>().color = new Color(1f, 0.25f, 0.25f);
				}
			}
			else
			{
				this.transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1);
			}

			s = this.name;

			this.transform.Find("Title").GetComponent<Text>().text = s;

		}

		public void OnPointerClick(PointerEventData eventData)
		{
			float distance = Vector2.Distance(this.transform.position, eventData.position);
			float radius = this.GetComponentInChildren<Image>().mainTexture.width * 0.5f + 2;
			if (distance < radius)
			{
				DoChoice();
			}
		}

		public virtual void DoChoice() { }
	}
}
