namespace Vn.Utils;

public static class MathEx
{
    public static bool NearlyZero(this float value, float epsilon = 0.0001f) => Math.Abs(value) < epsilon;
    public static bool AlmostEqual(this float lhs, float rhs, float epsilon = 0.0001f) => Math.Abs(lhs - rhs) < epsilon;

    public static bool PracticallyNotEqual(this float lhs, float rhs, float epsilon = 0.0001f) =>
        !AlmostEqual(lhs, rhs, epsilon);
    
    public static float ValueFromPercent(this float value, float percent) => value * percent / 100;
}