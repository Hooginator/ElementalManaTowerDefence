using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Main : Node2D
{
	const int starting_lives = 20;
	const int starting_gold = 20;
	
	[Signal]
	public delegate void LivesUpdatedEventHandler(int new_lives);
	
	[Signal]
	public delegate void GoldUpdatedEventHandler(int new_lives);
	
	[Signal]
	public delegate void GameOverEventHandler();
	int lives = 0;
	int gold = 0;
	WaveManager _WaveManager;
	BuildingManager _BuildingManager;
	GameUserInterface _GameUserInterface;

	[Signal]
	public delegate void TimeFactorUpdateEventHandler(float t);
	public float time_factor = 3f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// EnemyAndEnemyAccessories.CreepDied +=
		_WaveManager = GetNode<WaveManager>("WaveManager");
		_BuildingManager = GetNode<BuildingManager>("BuildingManager");
		_GameUserInterface = GetNode<GameUserInterface>("GameUserInterface");

		_GameUserInterface.Reset += () => ReStartGame();
		
		_GameUserInterface.Quit += () => QuitGame();

		
	}





	void SetTimeFactor(float t){
		time_factor = t;
		EmitSignal(SignalName.TimeFactorUpdate, time_factor);

	}

	public void ReStartGame(){
		_WaveManager.DestroyAll();
		_BuildingManager.DestroyAll();
		StartGame();
	}


	public void StartGame(){
		SetTimeFactor(1f);
		lives = starting_lives;
		EmitSignal(SignalName.LivesUpdated, lives);
		gold = starting_gold;
		EmitSignal(SignalName.GoldUpdated, gold);
		_WaveManager.Restart();

	}

	public void QuitGame(){
		GetTree().Quit();
	}

	int i=0;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		i++;
		if(i==1){
			StartGame();
		}

if (Input.IsActionPressed("SpeedUp"))
	{
		SetTimeFactor(Mathf.Min(time_factor + 0.5f, 10f));
	}
if (Input.IsActionPressed("SlowDown"))
	{
		SetTimeFactor(Mathf.Max(time_factor - 0.5f, 0.5f));
	}


	}

	public void CreepDied(){
		GD.Print("DIED MaIN");
		gold +=1;
		EmitSignal(SignalName.GoldUpdated, gold);
			}
	public void CreepReachedEnd(){
		GD.Print("CreepReachedEnd MaIN");
		lives -= 1;
		EmitSignal(SignalName.LivesUpdated, lives);
		if(lives <= 0){
			LoseGame();
		}
			}

			public int GetGold(){
				return gold;
			}
			public void SpendGold(int c){
				gold -= c;
		EmitSignal(SignalName.GoldUpdated, gold);
			}

			public void LoseGame(){
				SetTimeFactor(0f);
				gold = 0;
		EmitSignal(SignalName.GameOver);

			}
}
