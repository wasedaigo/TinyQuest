using UnityEngine;
using System.Collections.Generic;
public class Roga2dIntervalPlayer {
	private List<Roga2dBaseInterval> stopIntervals;
	private List<Roga2dBaseInterval> intervals;
	private static Roga2dIntervalPlayer instance;
	
	public static Roga2dIntervalPlayer GetInstance()
	{
		if (instance == null) {
			instance = new Roga2dIntervalPlayer();
		}
		return instance;
	}
	
	private Roga2dIntervalPlayer() {
		this.intervals = new List<Roga2dBaseInterval>();
		this.stopIntervals = new List<Roga2dBaseInterval>();
	}

	public Roga2dBaseInterval Play(Roga2dBaseInterval interval) {
		this.intervals.Add(interval);
		return interval;
	}

	public void Stop(Roga2dBaseInterval interval) {
		this.stopIntervals.Add(interval);
	}
	
	private bool isStopping(Roga2dBaseInterval interval) {
		foreach (Roga2dBaseInterval stopInterval in this.stopIntervals) {
			if (interval == stopInterval) {
				return true;	
			}
		}
		return false;
	}
	
	public void Update() {
        for (int i = this.intervals.Count - 1; i >= 0; i-- ) {
            Roga2dBaseInterval interval = this.intervals[i];
            if (this.isStopping(interval) || interval.IsDone()) {
				this.intervals.RemoveAt(i);
				this.stopIntervals.Remove(interval);
            } else {
				interval.Update(Time.deltaTime);
			}
        }
	}
}