// Pixel shader extracts the brighter areas of an image.
// This is the first step in applying a bloom postprocess.

sampler TextureSampler : register(s0);


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the original image color.
    float4 c = tex2D(TextureSampler, texCoord);
	return float4(1-c.r,1-c.g,1-c.b,c.a);
	//return float4(1,0,0,0);
    // Adjust it to keep only values brighter than the specified threshold.
   //return saturate((c - 0.5) / (1 - 0.5));
}


technique Inverse
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
