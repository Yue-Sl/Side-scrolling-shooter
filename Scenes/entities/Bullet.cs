using Godot;
using System;

public partial class Bullet : Area2D
{
	Vector2 dir = Vector2.Right;
	float speed = 800f;

	VisibleOnScreenNotifier2D visibleOnScreen;

	public override void _Ready()
	{
		visibleOnScreen = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		visibleOnScreen.ScreenExited += LeaveScene;
		var tween = CreateTween();
		tween.TweenProperty(GetNode<Sprite2D>("Sprite2D"),"scale",Vector2.One,0.2f).From(Vector2.Zero);
	}

	public override void _Process(double delta)
	{
		Position += dir * speed * (float)delta;
	}

	public void Setup(Vector2 pos, Vector2 dir)
	{
		Position = pos;
		this.dir = dir;
	}

	void LeaveScene()
	{
		QueueFree();
	}
}
