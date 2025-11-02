shader_type canvas_item;

// Time-based animation
uniform float time;

// Controls how strong the wave distortion is
uniform float wave_intensity = 0.1;

// Start and end colors for the gradient
uniform vec4 color_start : source_color = vec4(1.0, 0.5, 0.0, 1.0);
uniform vec4 color_end   : source_color = vec4(1.0, 0.0, 0.5, 1.0);

void fragment() {
    vec2 uv = UV;

    // Apply a sine wave distortion to the x-coordinate
    uv.x += sin(uv.y * 10.0 + time * 3.0) * wave_intensity;

    vec4 texture_color = texture(TEXTURE, uv);

    // Create color gradient
    vec4 gradient_color = mix(color_start, color_end, uv.y);

    // Combine gradient and texture
    vec4 final_color = texture_color * gradient_color;

    // Apply final color with alpha
    COLOR = final_color;
}

