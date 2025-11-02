using Godot;
using System;

public partial class ParticleController : GpuParticles2D
{
	private ShaderMaterial _shaderMaterial;

	public override void _Ready()
	{
		// Load the custom shader from file
		var shader = GD.Load<Shader>("res://Shaders/custom_particle.gdshader");
		
		// Create and assign a shader material
		_shaderMaterial = new ShaderMaterial();
		_shaderMaterial.Shader = shader;

		// Apply the shader material
		ProcessMaterial = _shaderMaterial;

		// Configure particle system properties
		Amount = 10;
		Lifetime = 2.0f;
		SpeedScale = 1.0f;
		Explosiveness = 0.0f;
		Emitting = true;

		// Configure basic emission shape and direction
		var material = new ParticleProcessMaterial();
		material.Gravity = new Vector3(0, 0, 0);
		material.InitialVelocityMin = 20.0f;
		material.InitialVelocityMax = 40.0f;
		material.Spread = 50.0f;
		material.Direction = new Vector3(0, -1, 0);
		material.ScaleMin = 0.5f;
		material.ScaleMax = 1.0f;

		ProcessMaterial = material;
		Material = _shaderMaterial;
	}

	public override void _Process(double delta)
	{
		if (_shaderMaterial != null)
		{
			// Pass elapsed time to shader for animation
			_shaderMaterial.SetShaderParameter("time", (float)Time.GetTicksMsec() / 1000.0f);
		}
	}
}
