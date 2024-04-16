static const int bayer2[2 * 2] =
{
    0, 2,
    3, 1
};

static const int bayer4[4 * 4] =
{
    0, 8, 2, 10,
    12, 4, 14, 6,
    3, 11, 1, 9,
    15, 7, 13, 5
};

static const int bayer8[8 * 8] =
{
    0, 32, 8, 40, 2, 34, 10, 42,
    48, 16, 56, 24, 50, 18, 58, 26,
    12, 44, 4, 36, 14, 46, 6, 38,
    60, 28, 52, 20, 62, 30, 54, 22,
    3, 35, 11, 43, 1, 33, 9, 41,
    51, 19, 59, 27, 49, 17, 57, 25,
    15, 47, 7, 39, 13, 45, 5, 37,
    63, 31, 55, 23, 61, 29, 53, 21
};

float GetBayer2(uint x, uint y)
{
    return float(bayer2[(x % 2) + (y % 2) * 2]) * (1.0f / 4.0f);
}

float GetBayer4(uint x, uint y)
{
    return float(bayer4[(x % 4) + (y % 4) * 4]) * (1.0f / 16.0f);
}

float GetBayer8(uint x, uint y)
{
    return float(bayer8[(x % 8) + (y % 8) * 8]) * (1.0f / 64.0f);
}

float4 GetClosestColor(float4 original, UnityTexture2D palette, UnitySamplerState samplerState, int colorCount)
{
    float4 closestColor = 1;
    float closestDistance = 1000;
    float step = 1.0f / colorCount;
    float origin = step * 0.5f;
    
    for (int i = 0; i < colorCount; i++)
    {
        float4 color = palette.Sample(samplerState, origin + i * step);
        float currentDist = distance(original, color);
        
        if (currentDist < closestDistance)
        {
            closestColor = color;
            closestDistance = currentDist;
        }
    }
    
    return closestColor;
}

void BayerDithering_float(float4 In, float4 ScreenPosition, float2 ScreenResolution, UnityTexture2D ColorPalette, UnitySamplerState PaletteSampler, float Spread, int BayerLevel, int ColorCount, out float4 Out)
{
    float2 uv = ScreenPosition.xy * ScreenResolution;
   
    float bayerValues[4] = { 0, 0, 0, 0 };
    bayerValues[0] = 0;
    bayerValues[1] = GetBayer2(uv.x, uv.y);
    bayerValues[2] = GetBayer4(uv.x, uv.y);
    bayerValues[3] = GetBayer8(uv.x, uv.y);
    
    float4 output = (In + Spread * (bayerValues[BayerLevel] - 0.5f));
    output = floor((ColorCount - 1.0f) * output + 0.5f) / (ColorCount - 1.0f);
    
    float4 color = GetClosestColor(output, ColorPalette, PaletteSampler, ColorCount);
    
    Out = color;
}