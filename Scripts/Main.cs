using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Main : Node2D
{
	#region Signals
	[Signal]
	public delegate void LivesUpdatedEventHandler(int new_lives);
	[Signal]
	public delegate void GoldUpdatedEventHandler(int new_lives);
	[Signal]
	public delegate void GameOverEventHandler();
	[Signal]
	public delegate void TimeFactorUpdateEventHandler(float t);
	#endregion
	#region Variables
	WaveManager _WaveManager;
	BuildingManager _BuildingManager;
	GameUserInterface _GameUserInterface;

	const int starting_lives = 200;
	const int starting_gold = 200;
	
	int lives = 0;
	int gold = 0;

	public float time_factor = 3f;
	#endregion
	#region Initialization
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_WaveManager = GetNode<WaveManager>("WaveManager");
		_BuildingManager = GetNode<BuildingManager>("BuildingManager");
		_GameUserInterface = GetNode<GameUserInterface>("GameUserInterface");
		_GameUserInterface.Reset += () => ReStartGame();
		_GameUserInterface.Quit += () => QuitGame();
	}
	#endregion
	#region Time Management
	void SetTimeFactor(float t){
		time_factor = t;
		EmitSignal(SignalName.TimeFactorUpdate, time_factor);
	}
	#endregion
	#region Game Management
	public void ReStartGame(){
		_WaveManager.DestroyAll();
		_BuildingManager.DestroyAll();
		StartGame();
	}

	public void StartGame(){
		// Reset TIme
		SetTimeFactor(1f);

		// Reset Lives
		lives = starting_lives;
		EmitSignal(SignalName.LivesUpdated, lives);

		// Reset Gold
		gold = starting_gold;
		EmitSignal(SignalName.GoldUpdated, gold);
		
		// Restart
		_WaveManager.Restart();
	}

	public void LoseGame(){
		SetTimeFactor(0f);
		gold = 0;
		EmitSignal(SignalName.GameOver);
	}

	public void QuitGame(){
		GetTree().Quit();
	}
	#endregion
	#region Process
	int i=0;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Start the game a few waves in, until I trigger it from elsewhere
		i++;
		if(i==1){
			StartGame();
		}

		// Time Speedup / slowdown with + -
		if (Input.IsActionPressed("SpeedUp"))
		{
			SetTimeFactor(Mathf.Min(time_factor + 0.5f, 10f));
		}
		if (Input.IsActionPressed("SlowDown"))
		{
			SetTimeFactor(Mathf.Max(time_factor - 0.5f, 0.5f));
		}
	}
	#endregion
	#region Creep Management
	public void CreepDied(){
		//GD.Print("DIED MaIN");
		gold +=1;
		EmitSignal(SignalName.GoldUpdated, gold);
			}
	public void CreepReachedEnd(){
		//GD.Print("CreepReachedEnd MaIN");
		lives -= 1;
		EmitSignal(SignalName.LivesUpdated, lives);
		if(lives <= 0){
			LoseGame();
		}
			}
	#endregion
	#region Gold Management
	public int GetGold(){
		return gold;
	}
	public void SpendGold(int c){
		if(c > gold){
			throw new NotEnoughGoldException();
		}
		gold -= c;
		EmitSignal(SignalName.GoldUpdated, gold);
	}
	#endregion
}
