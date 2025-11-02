using Godot;
using System.Collections.Generic;

public partial class PhysicsChain : Node2D
{
	// Number of links in the chain
	[Export] public int ChainSegments = 6;
	// Spacing between links
	[Export] public float SegmentDistance = 50f;
	// The scene for each chain link
	[Export] public PackedScene SegmentScene;
	
	// Create a list of rigid bodys and pin joint to hold the new nodes we create
	private readonly List<RigidBody2D> _segments = new();
	private readonly List<PinJoint2D> _joints = new();
	
	public override async void _Ready()
	{
		// Call function to create the chain
		CreateChain();
		
		// Wait one physics frame before unfreezing, to avoid startup collision forces
		await ToSignal(GetTree(), "physics_frame");
		foreach (var seg in _segments)
			seg.Freeze = false;
	}
	
	// Function to create the physics chain
	private void CreateChain()
	{
		// Check if the segment scene exists
		if (SegmentScene == null)
		{
			// If not then print an error and exit the function
			GD.PrintErr("SegmentScene is not assigned!");
			return;
		}
		
		// Create an invisible anchor that holds the first link
		var anchor = new StaticBody2D();
		AddChild(anchor);
		anchor.GlobalPosition = GlobalPosition + new Vector2(0, 0);
		
		// Create a variable to hold the previous segment
		RigidBody2D prevSegment = null;
		
		// Loop through all of the chain segments
		for (int i = 0; i < ChainSegments; i++)
		{
			// Instantiate a rigidbody segment
			var segment = SegmentScene.Instantiate<RigidBody2D>();
			AddChild(segment);
			
			// Start frozen to prevent initial physics explosions
			segment.Freeze = true;
			
			// Position segments vertically downward (move each segment down the appropriate amount)
			segment.GlobalPosition = anchor.GlobalPosition + new Vector2(0, (i + 1) * SegmentDistance);
			
			// Alternate collision layers to keep adjacent links from colliding
			if (i % 2 == 0)
			{
				segment.CollisionLayer = 1;
				segment.CollisionMask = 2;
			}
			else
			{
				segment.CollisionLayer = 2;
				segment.CollisionMask = 1;
			}
			
			// Add the segment segment to our segments list
			_segments.Add(segment);
			
			// Create and configure a PinJoint2D
			var joint = new PinJoint2D();
			AddChild(joint);
			
			// Check if this is the first link in the list
			if (i == 0)
			{
				// If so then the first link connects to the static anchor
				joint.NodeA = anchor.GetPath();
				joint.GlobalPosition = anchor.GlobalPosition + new Vector2(0, SegmentDistance / 2f);
			}
			else
			{
				// If not the first segment then connect it to the previous segment
				joint.NodeA = prevSegment.GetPath();
				joint.GlobalPosition = prevSegment.GlobalPosition + new Vector2(0, SegmentDistance);
				
				// Prevent immediate neighbors from colliding
				segment.AddCollisionExceptionWith(prevSegment);
			}
			
			// Add a joint to the current segment
			joint.NodeB = segment.GetPath();
			_joints.Add(joint);
			
			// Save the previous segment path
			prevSegment = segment;
		}
	}
}
