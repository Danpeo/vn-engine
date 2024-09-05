namespace Vn.Utils;

public static class NumberExtensions
{
    public static bool IsNearlyZero(this float value, float epsilon = 0.0001f) => Math.Abs(value) < epsilon;
    public static bool IsNearlyZero(this double value, double epsilon = 0.0001) => Math.Abs(value) < epsilon;
    public static bool IsAlmostEqual(this float lhs, float rhs, float epsilon = 0.0001f) => Math.Abs(lhs - rhs) < epsilon;
    public static bool IsAlmostEqual(this double lhs, double rhs, double epsilon = 0.0001) => Math.Abs(lhs - rhs) < epsilon;

    public static bool IsBarelyEqual(this float lhs, float rhs, float epsilon = 0.0001f) =>
        !IsAlmostEqual(lhs, rhs, epsilon);

    public static bool IsBarelyEqual(this double lhs, double rhs, double epsilon = 0.0001) =>
        !IsAlmostEqual(lhs, rhs, epsilon);
}