using Godot;
using System;
using System.ComponentModel;
using System.Reflection;

[Tool]
public partial class DrawFlagBox : VBoxContainer
{

	
	public struct DrawFlags
	{
		public int Draw { get; set; }
		public int Freeze { get; set; }
		
		public int LivePlay { get; set; }
	}


	public DrawFlags DrawState  = new DrawFlags(){Draw = 1,Freeze = 0,LivePlay = 0};
	
	
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			SetupDrawFlagsCheckboxes();
		}
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}




	/**
	 * Draw in VBoxContainer all the checkboxes for drawFlags
	 */

	private void SetupDrawFlagsCheckboxes()
	{
		if (Engine.IsEditorHint())
		{
			foreach (var field in typeof(DrawFlags).GetFields(BindingFlags.Public
																	   |BindingFlags.Instance
																	   |BindingFlags.NonPublic))
			{
				CheckBox checkBox = new CheckBox();
				int state = (int)field.GetValue(DrawState);
				string name = field.Name;
				
				if (state > 0)
				{
					checkBox.SetPressedNoSignal(true);
				}

				checkBox.Connect("pressed",Callable.From(() =>
				{
					OnPressed(checkBox);
				}));
				checkBox.Text = name.Substring(name.IndexOf("<")+1, name.IndexOf(">")-1);
				AddChild(checkBox);
			}		
		}
	
	}



	private void OnPressed(CheckBox checkBox)
	{
		

		switch (checkBox.Text)
		{
			case "Freeze":
			{
				DrawState.Freeze = DrawState.Freeze == 1 ? 0 : 1;
				break;
			}
			case "LivePlay":
			{
				DrawState.LivePlay = DrawState.LivePlay == 1 ? 0 : 1;
				break;
			}
			case "Draw":
			{
				DrawState.Draw = DrawState.Draw == 1 ? 0 : 1;
				break;
			}
				
		}
	}
}
