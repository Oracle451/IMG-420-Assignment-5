using Godot;

public partial class Player : CharacterBody2D
{
	// Set the players speed
	[Export] public float Speed = 200f;
	
	public override void _PhysicsProcess(double delta)
	{
		// TODO: Implement basic movement (WASD or Arrow keys)
		Vector2 velocity = Vector2.Zero;
		
		if (Input.IsActionPressed("ui_right")) velocity.X += 1;
		if (Input.IsActionPressed("ui_left")) velocity.X -= 1;
		if (Input.IsActionPressed("ui_down")) velocity.Y += 1;
		if (Input.IsActionPressed("ui_up")) velocity.Y -= 1;
		
		// TODO: Use MoveAndSlide()
		Velocity = velocity.Normalized() * Speed;
		MoveAndSlide();
		
		// Handle collisions with other objects (Chain links)
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			// Get the collision from the array of collisions
			var collision = GetSlideCollision(i);
			// Check if the colliding body is a rigid body 2d
			if (collision.GetCollider() is RigidBody2D rigid)
			{
				// If so then apply a force to the body
				// Push the rigid body slightly in the opposite direction of the hit normal
				Vector2 impulse = -collision.GetNormal() * 200f;
				rigid.ApplyImpulse(Vector2.Zero, impulse);
			}
		}
	}
}
