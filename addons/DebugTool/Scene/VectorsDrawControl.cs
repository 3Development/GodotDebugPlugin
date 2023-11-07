using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


[Tool]
public partial class VectorsDrawControl : HBoxContainer
{

	
	public struct VectorDisplayInfo
	{
		public Vector3 Origin { get; set; }
		public Vector3 Position { get; set; }
		public Color Color { get; set; }

		public string Name { get; set; }
	}
	private List<VectorDisplayInfo> _activeVectors;
	
	
	
	private InterfaceControl _interfaceControl;
	//Childrens
	//Add vector
	private HBoxContainer _addNewVectorBoxContainer;
	
	private Button _addVectorButton;
	private TextEdit _addVectorOriginText;
	private TextEdit _addVectorEndText;
	private TextEdit _addVectorColorText;
	private TextEdit _addVectorNameText;
	
	
	
	//ActiveVectorContainer
	private HBoxContainer _vectorDetailsContainer;
	private VBoxContainer _activeVectorsContainer;
	
	private Button _changeVectorButton;
	private Button _exitChangeVectorButton;
	private TextEdit _vectorOriginText;
	private TextEdit _vectorPositionText;
	private TextEdit _vectorColorText;
	private TextEdit _vectorNameText;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_activeVectors = new List<VectorDisplayInfo>();
		_interfaceControl = (InterfaceControl)GetParent().GetParent().GetParent();
		_activeVectorsContainer = (VBoxContainer)GetNode("ActiveVectorsContainer");
		_addNewVectorBoxContainer = (HBoxContainer)GetNode("ControlVector/AddNewVectorContainer");
		_vectorDetailsContainer = (HBoxContainer)GetNode("ControlVector/VectorDetailsContainer");


		_addNewVectorBoxContainer.Visible = true;
		_vectorDetailsContainer.Visible = false;
		
		_SetupAddVectorContainer();
		_SetupChangeVectorContainer();
		_RegisterActionToInterfaceControl();

	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void _OnAddVectorPressedButton()
	{

		float[] vectorOrigin = _addVectorOriginText.Text.Split(",").Select(s =>float.Parse(s) ).ToArray();
		float[] vectorEnd = _addVectorEndText.Text.Split(",").Select(s =>float.Parse(s) ).ToArray();
		float[] vectorColor = _addVectorColorText.Text.Split(",").Select(s =>float.Parse(s) ).ToArray();
		string vectorName = _addVectorNameText.Text;
		
		_activeVectors.Add( 
			new VectorDisplayInfo()
			{
				Origin = new Vector3(vectorOrigin[0],vectorOrigin[1],vectorOrigin[2]),
				Position = new Vector3(vectorEnd[0],vectorEnd[1],vectorEnd[2]),
				Color = new Color(vectorColor[0],vectorColor[1],vectorColor[2]),
				Name = vectorName
			}
		);
		
		Label label = new Label();
		label.Text = $"{vectorName}";
		label.MouseFilter = MouseFilterEnum.Pass;
		_activeVectorsContainer.AddChild(label);
		
		_SetupVectorLabelClickEvent(label,_activeVectors[_activeVectors.Count-1]);
		_RestartInputsAfterAddingNewVector();
	}
	
	/**
	 * <summary>
	 *	Preparing references to nodes for adding new vector
	 * </summary>
	 */
	private void _SetupAddVectorContainer()
	{
		_addVectorButton = (Button)GetNode("ControlVector/AddNewVectorContainer/GridContainer/AddVector");
		_addVectorOriginText = (TextEdit)GetNode("ControlVector/AddNewVectorContainer/GridContainer/TextOrigin");
		_addVectorEndText = (TextEdit)GetNode("ControlVector/AddNewVectorContainer/GridContainer/TextEnd");
		_addVectorColorText = (TextEdit)GetNode("ControlVector/AddNewVectorContainer/GridContainer/TextColor");
		_addVectorNameText = (TextEdit)GetNode("ControlVector/AddNewVectorContainer/GridContainer/TextName");
		
		_addVectorButton.Connect("pressed", new Callable(this, "_OnAddVectorPressedButton"));
	}
	
	/**
	 * <summary>
	 *	Preparing references to nodes for adding new vector
	 * </summary>
	 */
	private void _SetupChangeVectorContainer()
	{
		_changeVectorButton = (Button)_vectorDetailsContainer.GetNode("GridContainer/ChangeVector");
		_exitChangeVectorButton = (Button)_vectorDetailsContainer.GetNode("GridContainer/Exit");
		_vectorOriginText = (TextEdit)_vectorDetailsContainer.GetNode("GridContainer/TextOrigin");
		_vectorPositionText = (TextEdit)_vectorDetailsContainer.GetNode("GridContainer/TextEnd");
		_vectorColorText = (TextEdit)_vectorDetailsContainer.GetNode("GridContainer/TextColor");
		_vectorNameText = (TextEdit)_vectorDetailsContainer.GetNode("GridContainer/TextName");

		_exitChangeVectorButton.Connect("pressed", Callable.From(() =>
		{
			_vectorDetailsContainer.Visible = false;
			_addNewVectorBoxContainer.Visible = true;

		}));
	}



	/**
	 * <summary>
	 *	First it will delete all other children and then rerender;
	 * </summary>
	 */
	private void _SetupVectorLabelClickEvent(Label label,VectorDisplayInfo vectorDisplayInfo)
	{
		label.Connect("gui_input",Callable.From((InputEvent @event) =>
		{
			
			if (@event is InputEventMouseButton mouseButton)
			{
				if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
				{
					Console.WriteLine("Clicked");
					_OnVectorLabelPressed(vectorDisplayInfo);
				}
			}
		}));
	}
	
	public void _OnVectorLabelPressed(VectorDisplayInfo vectorDisplayInfo)
	{
		
		_vectorDetailsContainer.Visible = true;
		_addNewVectorBoxContainer.Visible = false;


		_vectorOriginText.Text = vectorDisplayInfo.Origin.ToString();
		_vectorPositionText.Text = vectorDisplayInfo.Position.ToString();
		_vectorColorText.Text = vectorDisplayInfo.Color.ToString();
		_vectorNameText.Text = vectorDisplayInfo.Name.ToString();
		
	}
	
	/**
	 * 
	 */
	private void _RestartInputsAfterAddingNewVector()
	{
		_addVectorOriginText.Clear();
		_addVectorColorText.Clear();
		_addVectorEndText.Clear();
	}

	private void _RegisterActionToInterfaceControl()
	{
		_interfaceControl.completeDrawActions += () =>
		{
			foreach (var vector in _activeVectors)
			{
				//DebugDraw3D.DrawLine(vector.Origin, vector.Position, new Color(1, 1, 0));
			}
			
		};
	}
	
	
	
	
}


