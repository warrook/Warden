﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warden
{
	public enum MoveState
	{
		Finished, //Call OnEnd, delete
		Selected, //Not active yet
		Active //Not done yet
	}

	public enum Wait
	{
		None,
		Busy,
		Dialogue		
	}

	public enum BattlePhase
	{
		BattleStart, //Do intro animations -- run once
		RoundStart,
		TurnPre,
		TurnUse,
		TurnPost,
		RoundEnd,
		BattleEnd, //Run once
		Waiting,
		//Extra phases: Movement (switch) and Death
	}

	public enum BattleMenu
	{
		Busy, //Other things are working, so don't display anything
		Start, //Fight/Bag/etc hasn't been chosen yet
		Fight,
		Team,
		Spells
	}
}
