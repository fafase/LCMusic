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

	public void InitWithChallenges( int [] newStreakChallenges, int [] bpms)
	{
		if(this.challengeContainer == null) { this.challengeContainer = new RhythmChallengeContainer(); }
		this.challengeContainer.InitWithChallenges(newStreakChallenges, bpms);
	}

	public int CheckCurrentChallenge(int currentStreak)
	{
		return this.challengeContainer.CheckCurrentChallenge(currentStreak);
	}
}

[SerializeField]
public class RhythmChallengeContainer
{
	private int [] challenges = null;
	private int [] bpms = null;
	private int currentChallenge = -1;
	private int index = -1;

	public int CurrentChallenge { get { return this.challenges[index]; } }
	public int CurrentBpm { get { return this.bpms[index]; } }

	public RhythmChallengeContainer() { }

	public void InitWithChallenges( int [] newChallenges, int [] newBpms)
	{
		if(newChallenges == null || newChallenges.Length == 0) { throw new System.Exception("Issue with challenges"); }
		if(newBpms == null || newBpms.Length == 0) { throw new System.Exception("Issue with newBpms"); }
		this.bpms = newBpms;
		this.challenges = newChallenges;
		index = 0;
		this.currentChallenge = this.challenges[index];
	}

	public int CheckCurrentChallenge(int currentStreak)
	{
		if(this.currentChallenge <= 0) { throw new System.Exception("Issue with current challenge"); } 
		if(currentStreak < this.currentChallenge) { return -1; }
		if(++index == this.challenges.Length)
		{
			return 1;
		}
		return 0;
	}
}