namespace Vn.Utils;

public static class NumberExtensions
{
    public static bool NearlyZero(this float value, float epsilon = 0.0001f) => Math.Abs(value) < epsilon;
    public static bool NearlyZero(this double value, double epsilon = 0.0001) => Math.Abs(value) < epsilon;
    public static bool AlmostEqual(this float lhs, float rhs, float epsilon = 0.0001f) => Math.Abs(lhs - rhs) < epsilon;
    public static bool AlmostEqual(this double lhs, double rhs, double epsilon = 0.0001) => Math.Abs(lhs - rhs) < epsilon;

    public static bool PracticallyNotEqual(this float lhs, float rhs, float epsilon = 0.0001f) =>
        !AlmostEqual(lhs, rhs, epsilon);

    public static bool PracticallyNotEqual(this double lhs, double rhs, double epsilon = 0.0001) =>
        !AlmostEqual(lhs, rhs, epsilon);
}