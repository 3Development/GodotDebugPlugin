using Godot;
using System;
using WritePlugins.addons.DebugTool.Utilz;

public partial class PlayerScript : StaticBody3D
{
	private RedisController _redisController;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_redisController = new();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = new Vector3(Position.X + 0.001F, 0, 0);
		
		_redisController.UpdatePosition(this);
	}
}
