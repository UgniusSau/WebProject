using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeLounge.Data.Models
{
	public class CrashGameState
	{
		public enum Stage
		{
			BETTING, IN_PROGRESS, CRASHED
		}

		public Stage CurrentStage { get; set; }

		public TimeSpan Countdown { get; set; }

		public float CrashingPoint { get; set; }

		public float CrashValue { get; set; } 

		public CrashGameState()
		{
			CurrentStage = Stage.BETTING;
			Countdown = TimeSpan.FromSeconds(10);
			CrashingPoint = 0;
			CrashValue = 0;

		}



	}
}
