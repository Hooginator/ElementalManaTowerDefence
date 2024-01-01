using Godot;
using System;
using System.Collections.Generic;

public partial class BuildingManager : Node2D
{
	#region variables

	
	
	[Signal]
	public delegate void FailedBuildEventHandler();
	[Signal]
	public delegate void SuccessfulBuildEventHandler();
	// Tower resources
	public List<PackedScene> _towers = new List<PackedScene>();
	List<TowerAndTowerAccessories> TowerAndTowerAccessoriesList = new List<TowerAndTowerAccessories>();

	// Tower parameters
	List<int> costs = new List<int>(){  };
	private int _default_cost = 20;
	private float tower_size = 100f;
	List<SpriteFrames> tower_bases = new List<SpriteFrames>();
	List<TowerAndTowerAccessories.stats> tower_stats = new List<TowerAndTowerAccessories.stats>()
	{
		// Basic
		{new TowerAndTowerAccessories.stats(
			_rotation_speed: 1f, _range: 500f, _damage: 2f, _attack_rate: 1f, _cost: 10, _projectile_speed: 1f, _projectile_lifetime: 400f)},
		//  Rapid Fire
		{new TowerAndTowerAccessories.stats(
			_rotation_speed: 2f, _range: 400f, _damage: 1f, _attack_rate: 3f, _cost: 10, _projectile_speed: 2f, _projectile_lifetime: 200f)},
		// Sniper
		{new TowerAndTowerAccessories.stats(
			_rotation_speed: 0.5f, _range: 800f, _damage: 10f, _attack_rate: 0.3f, _cost: 10, _projectile_speed: 8f, _projectile_lifetime: 400f)}
		
		
	};

	// Tower Management
	private List<TowerAndTowerAccessories> tower_list = new List<TowerAndTowerAccessories>();
	
	// Placement Management
	bool just_clicked = false;
	private int _tower_index = 0;

	// Other
	private Main _root;
	private EnemyPath _EnemyPath;
	// To get from elsehwewre later
		private List<Vector2> waypoints = new List<Vector2>(){new Vector2(0, 0), new Vector2(300, 300), new Vector2(300, 600), new Vector2(600, 600), new Vector2(600, 300), new Vector2(900,300), new Vector2(1200,300)};

	

	#endregion
	#region Initialization
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_root = GetParent<Main>();
		_EnemyPath = GetParent().GetNode<EnemyPath>("EnemyPath");
		_towers.Add( GD.Load<PackedScene>("res://Tower.tscn"));

		tower_bases.Add(GD.Load<SpriteFrames>("res://Images/Tower1Sprite.tres"));
		tower_bases.Add(GD.Load<SpriteFrames>("res://Images/Tower2Sprite.tres"));
		tower_bases.Add(GD.Load<SpriteFrames>("res://Images/Tower3Sprite.tres"));
		// _towers.Add( GD.Load<PackedScene>("res://Towers/Tower2.tscn"));

		int i = 0;
		foreach (var t in _towers){
			// Memory leak waiting to happen...
			var temp = t.Instantiate();
			var all_children = temp.GetChildren();
			all_children.Add(temp);
			foreach (var c in all_children){
				if (c is TowerAndTowerAccessories){
					GD.Print($"FOUND TOWERSEESSES! + {i}");
					TowerAndTowerAccessories tt = (TowerAndTowerAccessories) c;
					TowerAndTowerAccessoriesList.Add(tt);
					// Costs
					costs.Add(tt.cost);
					break;
				}
			}
			i++;
		}

	}
	#endregion
	#region Process
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Select Tower
		if (Input.IsActionPressed("SelectBuild1")){
			_tower_index = 0;
		}
		if (Input.IsActionPressed("SelectBuild2")){
			_tower_index = 1;
		}
		if (Input.IsActionPressed("SelectBuild3")){
			_tower_index = 2;
		}

		
		 if (Input.IsActionPressed("Confirm1") )
		{
			if(_root.GetGold() >= costs[0]){
				bool too_close = false;
				Vector2 mouse_pos = GetViewport().GetMousePosition();
			foreach(var t in tower_list){
				var diff_vec = t.Position - mouse_pos;
				if(diff_vec.Length() < tower_size){
					too_close = true;
					break;
				}
			}
			if(!too_close){
				// Too close to path?
			if(_EnemyPath.GetClosestDistance(mouse_pos) < tower_size/2){
				too_close = true;
			}

			}

			// Build
			if(!just_clicked && !too_close)
			{	
				just_clicked = true;
				BuildTower();
				EmitSignal(SignalName.SuccessfulBuild);
			}else{
				EmitSignal(SignalName.FailedBuild);
			}
			}
		}else{
			just_clicked = false;
		}
	}
	#endregion
	#region Tower Management
	public void BuildTower(){
		// Spend the gold needed
		try{
			_root.SpendGold(costs[0]);	
		}catch(NotEnoughGoldException){
			return;
		}

		// Build the tower
		var just_built = _towers[0].Instantiate();
		_root.AddChild(just_built);

		// Initialize the tower
		var tower = just_built.GetNode<TowerAndTowerAccessories>(".");
		tower.Position = GetViewport().GetMousePosition();
		tower.SetTimeFactor(_root.time_factor);

		GD.Print($"Building tower with {_tower_index}");
		tower.SetTowerBaseSprite(tower_bases[_tower_index]);
		tower.Initialize(tower_stats[_tower_index]);

		// Don't double purchase
		just_clicked = true;
	}

	public void AddTower(TowerAndTowerAccessories tw){
			GD.Print("Adding tower: ");
			// Should be _root :/
			// Link tower to time
			GetParent<Main>().TimeFactorUpdate += (float t) => tw.SetTimeFactor(t);

			// Tower monitoring list
			tower_list.Add(tw);
	}
	
	public void DestroyAll(){
		// Destroy the Nodes
		foreach(var t in tower_list){
			t.QueueFree();
		}
		// Clear tower management lists
		tower_list.Clear();
	}
	#endregion
}
