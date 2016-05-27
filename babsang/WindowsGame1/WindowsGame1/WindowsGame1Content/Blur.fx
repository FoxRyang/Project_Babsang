// Pixel shader applies a one dimensional gaussian blur filter.
// This is used twice by the bloom postprocess, first to
// blur horizontally, and then again to blur vertically.

sampler TextureSampler : register(s0);




float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 c = 0;
    
   
    //c += tex2D(TextureSampler, texCoord +float2(0,0.01));
    c += tex2D(TextureSampler, texCoord +float2(0,0));
    //c += tex2D(TextureSampler, texCoord +float2(0,-0.01));
	//c += tex2D(TextureSampler, texCoord +float2(0.01,0.01));
    c += tex2D(TextureSampler, texCoord +float2(0.005,0));
    //c += tex2D(TextureSampler, texCoord +float2(0.01,-0.01));
	//c += tex2D(TextureSampler, texCoord +float2(-0.01,0.01));
    c += tex2D(TextureSampler, texCoord +float2(-0.005,0));
    //c += tex2D(TextureSampler, texCoord +float2(-0.01,-0.01));
      //return float4(1,0,0,1); 
     return float4(c.r/3.0f,c.g/3.0f,c.b/3.0f,1.0f);
}


technique Blur
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
