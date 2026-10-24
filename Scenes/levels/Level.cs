using Godot;
using System;

public partial class Level : Node2D
{
	[Export] public PackedScene bulletPrefab;

	Player player;
	Node2D bulletsRoot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("player");

		player.Shoot += OnPlayerShoot;

		bulletsRoot = GetNode<Node2D>("bullets");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void OnPlayerShoot(Vector2 pos, Vector2 dir)
	{
        Bullet bullet = bulletPrefab.Instantiate<Bullet>();
		bullet.Setup(pos,dir);
		bulletsRoot.AddChild(bullet);
	}
}
