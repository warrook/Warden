using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warden
{
	/// <summary>
	/// This is a 'real' version of a given MoveData.
	/// All MoveTypes must derive from this.
	/// </summary>
	public class MoveType : IComparable
	{
		public MoveData data;
		protected sbyte flagInternal; //When the Flag is -1, it is done
		public sbyte Flag => flagInternal;
		public float Priority; //The speed stat of the user, modified by the move's actual priority

		//Base variables, used by derived classes for stuff
		public Beast User;
		public Beast[] Targets;
		public string Name; //Display name of the move
		public string Essence; //Essence of the move
		public string Focus; //Valid targets
		public string Stat; //Generic.Constants.Stat value to use

		public MoveType(MoveData data, Beast user)
		{
			//All info has already been validated in Generic.CustomXmlReader
			this.data = data;
			User = user;
			Name = data.Name;
			Essence = data.Essence;
			Focus = data.Focus;
			Stat = data.Stat;
		}

		public virtual string ExposeData()
		{
			return string.Join(",", User.data.dataName, Priority, Name, Essence, Focus, Stat);
		}

		//public virtual bool IsValid()
		// { Determine conditions that the move would be or wouldn't be valid -- to help AI choose moves }
		//public virtual float Validity()
		// { Determine conditions that make the move prioritized (such as a move does 200% damage against burns, and the target is burned) }

		//Fires right before the move is used.
		public virtual void OnPre() { }

		//Fires when the move is used.
		public virtual void OnUse() { flagInternal = 1; }

		//Fires right after the move is used.
		public virtual void OnPost() { }

		//Fires when the effects of the move end.
		public virtual void OnEnd() { }

		//Fires at the beginning of the round, the round after it was used.
		public virtual void OnRoundStart() { }

		//Fires at the end of the round, *including* the round the move was used.
		public virtual void OnRoundEnd() { }

		//Fires when anything moves or switches after the move is used.
		public virtual void OnMovement() { }

		//Fires when anything dies after the move is used.
		public virtual void OnDeath() { }

		protected float CalcDamage(float baseDamage)
		{
			return baseDamage * (User.Stats[Stat].Current * 0.1f) + User.Level;
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return 1;
			if (obj is MoveType other)
				return this.Priority.CompareTo(other.Priority);
			else
				throw new ArgumentException("Object is not a MoveType");
		}
	}
}
