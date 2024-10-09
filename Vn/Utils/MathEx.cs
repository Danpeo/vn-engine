using System.Numerics;

namespace Vn.Utils;

public static class MathEx
{
    public static bool NearlyZero(this float value, float epsilon = 0.0001f) => Math.Abs(value) < epsilon;
    public static bool AlmostEqual(this float lhs, float rhs, float epsilon = 0.0001f) => Math.Abs(lhs - rhs) < epsilon;

    public static bool PracticallyNotEqual(this float lhs, float rhs, float epsilon = 0.0001f) =>
        !AlmostEqual(lhs, rhs, epsilon);
    
    public static float ValueFromPercent(this float value, float percent) => value * percent / 100;

    public static float Lerp(float start, float end, float t)
    {
        t = Math.Clamp(t, 0, 1);
        return (1 - t) * start + t * end;
    }

    public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
    {
        t = Math.Clamp(t, 0, 1);
        return (1 - t) * start + t * end;
    }
}