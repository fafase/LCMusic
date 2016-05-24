using UnityEngine;
using System.Collections;

public interface IRhythmChallenge{ }
[RequireComponent(typeof(RhythmController))]
public class RhythmChallengeController : MonoBehaviour , IRhythmChallenge
{
	private IRhythmController rhythmController = null;
	private IRhythmStreak rhythmStreak = null;
	private RhythmChallengeContainer challengeContainer = null;

	private void Awake()
	{
		this.rhythmController = this.gameObject.GetComponent<IRhythmController>();
		this.rhythmStreak = this.gameObject.GetComponent<IRhythmStreak>();
		if(this.challengeContainer == null){ this.challengeContainer = new RhythmChallengeContainer(); }
	}

	public void InitWithChallenges( int newStreakChallenges, int bpms)
	{
		if(this.challengeContainer == null) { this.challengeContainer = new RhythmChallengeContainer(); }
		this.challengeContainer.InitWithChallenges(newStreakChallenges, bpms);
	}

	public int CheckCurrentChallenge(int currentStreak)
	{
		return this.challengeContainer.CheckWithChallenge(currentStreak);
	}
}

[SerializeField]
public class RhythmChallengeContainer
{
	public int CurrentChallenge { get; private set; }
	public int CurrentBpm { get; private set; }

	public RhythmChallengeContainer() { }

	public void InitWithChallenges( int newChallenge, int newBpm)
	{
		if(newChallenge <= 0) { throw new System.Exception("Issue with challenges"); }
		if(newBpm <=0) { throw new System.Exception("Issue with newBpms"); }
		this.CurrentBpm = newBpm;
		this.CurrentChallenge = newChallenge;
	}

	public int CheckWithChallenge(int currentStreak)
	{
		if(this.CurrentChallenge <= 0) { throw new System.Exception("Issue with current challenge"); } 
		if(currentStreak < this.CurrentChallenge) { return -1; }
		return 1;
	}
}