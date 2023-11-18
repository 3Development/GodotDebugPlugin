using Godot;

namespace WritePlugins.addons.debug_draw_3d.csharp;


[Tool]
public partial class DebugNode3d : StaticBody3D
{
	public override void _Process(double delta)
	{
		
		
	}

	public override void _Ready()
	{
		
	}
	
	/**
	 * 
	 */
	public void SetupNewPosition(Vector3 vector3)
	{
		Position = vector3;
	}
}
