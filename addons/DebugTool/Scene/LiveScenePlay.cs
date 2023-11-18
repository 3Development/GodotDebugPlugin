using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using WritePlugins.addons.debug_draw_3d.csharp;
using WritePlugins.addons.DebugTool.Utilz;


[Tool]
public partial class LiveScenePlay : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.

	private PackedScene _mainScene;
	
	
	
	// Interface nodes
	private InterfaceControl _interfaceControl;
	private DrawFlagBox _drawFlagBox;
	private RedisController _redis;
	private LineEdit _absolutePathEditNode;
	
	private bool FIRST_INITALIZATION = false;
	private bool RESET_POSITION = false;
	
	
	
	public override void _Ready()
	{
		_interfaceControl = (InterfaceControl)GetParent().GetParent().GetParent();
		_mainScene = GD.Load<PackedScene>("res://Main.tscn");
		_drawFlagBox = _interfaceControl.GetNode<DrawFlagBox>("DrawFlagBox");
		_absolutePathEditNode = GetNode<LineEdit>("./SceneContainer/LineEdit");
		_redis = new RedisController();
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!FIRST_INITALIZATION && _mainScene.GetLocalScene().Name.ToString().Equals(_absolutePathEditNode.Text))
		{
			_redis.SetInitialPositionsForAllNodesInScene(_mainScene);
			FIRST_INITALIZATION = true;
		}
		
		if (_drawFlagBox.DrawState.LivePlay == 1)
		{
			if (Engine.IsEditorHint())
			{
				if (!RESET_POSITION) RESET_POSITION = true;
				
				if (_mainScene.GetLocalScene().Name.ToString().Equals(_absolutePathEditNode.Text))
				{
					foreach (var child in _mainScene.GetLocalScene().GetTree().GetNodesInGroup("DebugNode"))
					{
						Node3D node3D = (Node3D)child;
						Vector3 vector3 = _redis.GetPosition(node3D);
						node3D.Position = vector3;
					}
				}
			}		
		}
		else
		{
			if (RESET_POSITION)
			{
				IDictionary<string, Vector3> initPositions = _redis.GetInitialPositions();
				
				foreach (var child in _mainScene.GetLocalScene().GetChildren())
				{
					Node3D node3D = (Node3D)child;
					node3D.Position = initPositions[child.Name.ToString()];
					_redis.UpdatePosition(node3D);
				}

				RESET_POSITION = false;
			}
		}
	}
	
	
	


}
