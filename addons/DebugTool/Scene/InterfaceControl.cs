using Godot;
using System;

public delegate void DrawAction();

[Tool]
public partial class InterfaceControl : Control
{

	public struct LiveSceneFlags
	{
		private bool play = false;


		public LiveSceneFlags(){}
	}
	
	private Button _button;

	private DrawFlagBox _drawFlagBox;


	public DrawAction completeDrawActions = (() => { });

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_drawFlagBox = GetNode<DrawFlagBox>("DrawFlagBox");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if (_drawFlagBox.DrawState.Draw == 1)
			{
				completeDrawActions();
			}
		}
	}
}
