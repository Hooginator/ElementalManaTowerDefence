using Godot;
using System;
using System.Collections.Generic;

public partial class WaveManager : Node2D
{
	
	[Signal]
	public delegate void WaveStartedEventHandler();
	private List<EnemyAndEnemyAccessories.stats> enemy_base_state = new List<EnemyAndEnemyAccessories.stats>()
	{
		{new EnemyAndEnemyAccessories.stats("Basic", 2, 1.5f, 1, 150, 20, 2, 5)},
		{new EnemyAndEnemyAccessories.stats("Small", 1, 3f, 1, 40, 50, 1, 2)},
		{new EnemyAndEnemyAccessories.stats("Big", 5, 0.5f, 5, 400, 10, 10, 20)}
	};
	private List<EnemyAndEnemyAccessories> enemy_list = new List<EnemyAndEnemyAccessories>();
	public PackedScene enemy { get; set; }
	
	List<SpriteFrames> enemy_bases = new List<SpriteFrames>();
	float time_factor = 1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = GD.Load<PackedScene>("res://Enemy.tscn");
		_Main = GetParent<Main>();
		_Main.TimeFactorUpdate += (float t) => SetTimeFactor(t);
		
		
		enemy_bases.Add(GD.Load<SpriteFrames>("res://Images/Enemy/Enemy.tres"));
		enemy_bases.Add(GD.Load<SpriteFrames>("res://Images/Enemy/SmallEnemy.tres"));
		enemy_bases.Add(GD.Load<SpriteFrames>("res://Images/Enemy/BigEnemy.tres"));
	}
	float i=0f;
	int spawn_rate= 150;
	int creep_remaining = 0;
	bool is_wave_happening = false;
	int _wave_number = 0;
	private Main _Main;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
				// Summon a creep
		if(i/spawn_rate > 1 && is_wave_happening){
			if(creep_remaining > 0){
			SummonCreep();
			creep_remaining -= 1;
			}else{
				EndWave();
			}
			i = 0;
		}
		i+=time_factor;
	}

	private async void EndWave(){
		is_wave_happening = false;
		int temp_wave_number = _wave_number;
		// creep_remaining = 20;
	await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
		// want to wait until previous wave defeated
		if(is_wave_happening == false){ StartWave(temp_wave_number);}

	}
	public void Restart(){
		UpdateWave( 0) ;
		StartWave();
	}
	public void StartWave(int n = -1){
		if(n > -1 && _wave_number != n){
			return;
		}
		is_wave_happening = true;
		UpdateWave(_wave_number+1);
		EmitSignal(SignalName.WaveStarted);
		creep_remaining = enemy_base_state[_wave_number%enemy_base_state.Count].spawn_total;
		spawn_rate = enemy_base_state[_wave_number%enemy_base_state.Count].spawn_rate;

	}

	public void UpdateWave(int new_wave){
		_wave_number = new_wave;
		_Main.UpdateWave(new_wave);
	}

	private void SummonCreep(){
			var creep = enemy.Instantiate();
			AddChild(creep);
			var creep2 = creep.GetNode<EnemyAndEnemyAccessories>(".");
			creep2.Initialize(_wave_number
			,	enemy_base_state[_wave_number%enemy_base_state.Count]
			, enemy_bases[_wave_number%enemy_base_state.Count]);
			creep2.CreepDied += (EnemyAndEnemyAccessories e) => RemoveEnemy(e);
			creep2.CreepReachedEnd += (EnemyAndEnemyAccessories e) => RemoveEnemy(e);
			creep2.Position = new Vector2(0,0);
			enemy_list.Add(creep2);

			creep2.CreepDied += (EnemyAndEnemyAccessories e) => _Main.CreepDied(e);
			creep2.CreepReachedEnd += (EnemyAndEnemyAccessories e) => _Main.CreepReachedEnd();
			creep2.SetTimeFactor(time_factor);
			_Main.TimeFactorUpdate += (float t) => creep2.SetTimeFactor(t);

	}

	public Vector2 GetClosest(Vector2 v){
		float dist = 99999;
		Vector2 to_return = new Vector2(0,0);
		List<EnemyAndEnemyAccessories> to_remove = new List<EnemyAndEnemyAccessories>();
		foreach(var e in enemy_list){
			try{
			float d = Mathf.Abs(v[0] - e.Position[0]) + Mathf.Abs(v[1] - e.Position[1]);
			if(d < dist){
				dist = d;
				to_return = e.Position;
			}
			}catch(System.ObjectDisposedException){
				//GD.Print($"That enemy is dead, dont target it any more");
				to_remove.Add(e);
			}
		}
		foreach(var e in to_remove){
			// remove dead
			RemoveEnemy(e);
		}
		return to_return;
	}

	public String GetNextWaveName(){
		return enemy_base_state[(_wave_number + 1)%enemy_base_state.Count].name;
	}

	public void RemoveEnemy(EnemyAndEnemyAccessories e){
		enemy_list.Remove(e);

		// Check if that's the last creep for the wave
		if(!is_wave_happening && enemy_list.Count == 0 ){
			StartWave();
		}
	}

	
	public void SetTimeFactor(float t){
		time_factor = t;
	}
	public void DestroyAll(){
		foreach(var e in enemy_list){
			e.QueueFree();
		}
		enemy_list.Clear();
	}

	public int GetWaveNumber(){
		return _wave_number;
	}
}
