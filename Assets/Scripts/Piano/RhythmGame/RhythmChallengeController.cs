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

	public void CheckCurrentChallenge(int currentStreak)
	{
		RhythmChallengeContainer.ChallengeState cs = this.challengeContainer.CheckCurrentChallenge(currentStreak);
		switch(cs)
		{
		case RhythmChallengeContainer.ChallengeState.None:
			Debug.Log("None");
			break;
		case RhythmChallengeContainer.ChallengeState.CurrentChallenge:
			this.rhythmController.SetUI(" Well done! \n To the next challenge. ", this.challengeContainer.CurrentBpm);
			break;
		case RhythmChallengeContainer.ChallengeState.FinalChallenge:
			this.rhythmController.SetUI(" Well done! \n You made it. ", 0);
			this.rhythmController.ResetRhythmGame();
			Debug.Log("Final");
			break;
		}
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

	public ChallengeState CheckCurrentChallenge(int currentStreak)
	{
		if(this.currentChallenge <= 0) { throw new System.Exception("Issue with current challenge"); } 
		if(currentStreak < this.currentChallenge) { return ChallengeState.None; }
		if(++index == this.challenges.Length)
		{
			return ChallengeState.FinalChallenge;
		}
		return ChallengeState.CurrentChallenge;
	}

	public enum ChallengeState{ None = -1, CurrentChallenge, FinalChallenge }
}