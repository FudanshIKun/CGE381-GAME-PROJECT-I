using System;
using System.Linq;

public static class Utility{
	public static readonly string[] ScenesToExcludeFromSave ={ "MainMenu", "Map", "Prototype" };
	public static           float    LerpWithoutClamp (float a, float b, float t) => a + (b - a) * t;

	public static int LevelToValue(GameManager.Scenes scene){
		var sceneName = scene.ToString();
		if (ScenesToExcludeFromSave.Contains(sceneName))
			return 11;

		var value = scene switch{
			GameManager.Scenes.Level1  => 0,
			GameManager.Scenes.Level2  => 1,
			GameManager.Scenes.Level3  => 2,
			GameManager.Scenes.Level4  => 3,
			GameManager.Scenes.Level5  => 4,
			GameManager.Scenes.Level6  => 5,
			GameManager.Scenes.Level7  => 6,
			GameManager.Scenes.Level8  => 7,
			GameManager.Scenes.Level9  => 8,
			GameManager.Scenes.Level10 => 9,
			_                          => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
		};

		return value;
	}
}