using Vn.Utils;

namespace Vn.UI;

public class PulseCircle
{
    private readonly float _minRadius;
    private readonly float _maxRadius;
    private readonly float _pulseSpeed;
    private float _radius;
    private int _x;
    private int _y;
    private float _alpha;
    private readonly Color _color;
    
    public PulseCircle(int x, int y, float minRadius = 5f, float maxRadius = 8f, float pulseSpeed = 3f, float radius = 0.0f, Color? color = null)
    {
        _x = x;
        _y = y;
        _minRadius = minRadius;
        _maxRadius = maxRadius;
        _pulseSpeed = pulseSpeed;
        _radius = radius;
        _color = color ?? Color.Red;
    }

    public void Update(int x, int y, float alpha)
    {
        _x = x;
        _y = y;
        _alpha = alpha;
    }

    public void Draw()
    {
        Color color = _color with { A = (byte)(_alpha * 255) };
        
        _radius = _minRadius + (_maxRadius - _minRadius) * (0.5f * (1 + MathF.Sin((float)(GetTime() * _pulseSpeed))));
        DrawCircle(_x, _y, _radius, color);
    }
}