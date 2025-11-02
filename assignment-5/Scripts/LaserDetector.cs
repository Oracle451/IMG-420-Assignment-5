using Godot;

public partial class LaserDetector : Node2D
{
	// Set laser length in pixels
	[Export] public float LaserLength = 500f;
	// Set the colors for normal and alert stat
	[Export] public Color LaserColorNormal = Colors.Green;
	[Export] public Color LaserColorAlert = Colors.Red;
	// Reference the player node for detection
	[Export] public NodePath PlayerPath;
	
	// Create variables to hold the raycast, visual representation line, and player
	private RayCast2D _rayCast;
	private Line2D _laserBeam;
	private Node2D _player;
	// Boolean to tell if the alarm is triggered and a timer for the alarm going off
	private bool _isAlarmActive = false;
	private Timer _alarmTimer;

	public override void _Ready()
	{
		// Initialize the raycast and visual representation
		SetupRaycast();
		SetupVisuals();

		// Get a reference to the player
		_player = GetNode<Node2D>(PlayerPath);

		// Create a timer that resets the alarm 1.5 seconds after it is set off
		_alarmTimer = new Timer { WaitTime = 1.5, OneShot = true };
		_alarmTimer.Timeout += ResetAlarm;
		AddChild(_alarmTimer);
	}
	
	// Function to create the raycast
	private void SetupRaycast()
	{
		// TODO: Create and configure RayCast2D
		_rayCast = new RayCast2D
		{
			// Create a raycast 2d that projects forward as far as laser length specifies
			TargetPosition = new Vector2(LaserLength, 0),
			Enabled = true
		};
		AddChild(_rayCast);
	}
	
	// Function to set up the visual representation of the raycast
	private void SetupVisuals()
	{
		// TODO: Create Line2D for laser visualization
		_laserBeam = new Line2D
		{
			// Set the beam width and color
			Width = 3,
			DefaultColor = LaserColorNormal
		};
		// Create the points for the line to connect
		_laserBeam.AddPoint(Vector2.Zero);
		_laserBeam.AddPoint(new Vector2(LaserLength, 0));
		AddChild(_laserBeam);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// TODO: Force raycast update
		_rayCast.ForceRaycastUpdate();

		// TODO: Check if raycast is colliding
		if (_rayCast.IsColliding())
		{
			// Get the point of the collision
			Vector2 collisionPoint = _rayCast.GetCollisionPoint();
			Node collider = _rayCast.GetCollider() as Node;
			
			// TODO: Update laser beam visualization
			_laserBeam.SetPointPosition(1, ToLocal(collisionPoint));
			
			// TODO: Check if hit object is player
			if (collider == _player && !_isAlarmActive)
			{
				// If the trigger was a player then acitvate the alarm
				TriggerAlarm();
			}
		}
		else
		{
			// Set the laser to its full length again
			_laserBeam.SetPointPosition(1, new Vector2(LaserLength, 0));
		}
	}
	
	// Function to activate the alarm
	private void TriggerAlarm()
	{
		// Set the boolean to true
		_isAlarmActive = true;
		// Change laser color to red
		_laserBeam.DefaultColor = LaserColorAlert;
		// Print activation message
		GD.Print("ALARM! Player detected!");
		// Start the alarm timer
		_alarmTimer.Start();
	}
	
	// Function to deactivate the alarm
	private void ResetAlarm()
	{
		// Reset the active alarm boolean and the laser color
		_isAlarmActive = false;
		_laserBeam.DefaultColor = LaserColorNormal;
	}
}
