using Godot;
using System;
using System.Collections.Generic;

public partial class WaveManager : Node2D
{
	private List<EnemyAndEnemyAccessories> enemy_list = new List<EnemyAndEnemyAccessories>();
	public PackedScene enemy { get; set; }
	float time_factor = 1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemy = GD.Load<PackedScene>("res://Enemy.tscn");
		_Main = GetParent<Main>();
		_Main.TimeFactorUpdate += (float t) => SetTimeFactor(t);
		
	}
	float i=0f;
	int creep_remaining = 0;
	bool is_wave_happening = false;
	int _wave_number = 0;
	private Main _Main;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
				// Summon a creep
		if(i/150 > 1 && is_wave_happening){
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
		creep_remaining = 20;
	await ToSignal(GetTree().CreateTimer(7.0f), SceneTreeTimer.SignalName.Timeout);
		
		StartWave();

	}
	public void Restart(){
		_wave_number = 0;
		StartWave();
	}
	public void StartWave(){
		creep_remaining = 20;
		is_wave_happening = true;
		_wave_number+=1;

	}

	private void SummonCreep(){
			var creep = enemy.Instantiate();
			AddChild(creep);
			var creep2 = creep.GetNode<EnemyAndEnemyAccessories>(".");
			creep2.SetLevel(_wave_number);
			creep2.Position = new Vector2(0,0);
			enemy_list.Add(creep2);

			creep2.CreepDied += () => _Main.CreepDied();
			creep2.CreepReachedEnd += () => _Main.CreepReachedEnd();
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
			enemy_list.Remove(e);
		}
		return to_return;
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
}
