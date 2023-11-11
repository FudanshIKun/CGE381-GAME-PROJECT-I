public static class Utility{
	public static float LerpWithoutClamp (float a, float b, float t) => a + (b - a) * t;
}