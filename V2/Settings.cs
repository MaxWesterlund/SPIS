using Tomlyn;
using Tomlyn.Model;

static class Settings {
	public static int Width;
	public static int Height;
	public static int Scaling;
	public static int FrameSkip;
	public static bool DrawAgents;
	public static string? SplashText;

	public static float PheremoneDecayRate;

	public static int AgentCount;
	public static float AgentSpeed;
	public static float AgentSensorAngle;
	public static float AgentSensorDistance;
	public static float AgentSensorSize;

	public static float PheremoneWeight;
	public static float HeightWeight;

	public static void LoadSettings(string path) {
		string tomlString = File.ReadAllText(path);
		TomlTable tomlTable = Toml.ToModel(tomlString);

		// ---------------- window params ------------------ \\
		
		TomlTable screenTable = (TomlTable)tomlTable["window"];
		
		Width = (int)(long)screenTable["Width"];
		Height = (int)(long)screenTable["Height"];

		Scaling = (int)(long)screenTable["Scaling"];
		FrameSkip = (int)(long)screenTable["FrameSkip"];

		DrawAgents = (bool)screenTable["DrawAgents"];
		SplashText = (string)screenTable["SplashText"];

		// ------------------ sim params ------------------ \\

		TomlTable paramsTable = (TomlTable)tomlTable["parameters"];
		
		PheremoneDecayRate = (float)(double)paramsTable["PheremoneDecayRate"];
		
		AgentCount = (int)(long)paramsTable["AgentCount"];
		AgentSpeed = (float)(double)paramsTable["AgentSpeed"];
		AgentSensorAngle = (float)(double)paramsTable["AgentSensorAngle"];
		AgentSensorDistance = (float)(double)paramsTable["AgentSensorDistance"];
		AgentSensorSize = (float)(double)paramsTable["AgentSensorSize"];
		
		PheremoneWeight = (float)(double)paramsTable["PheremoneWeight"];
		HeightWeight = (float)(double)paramsTable["HeightWeight"];
	
		return;
	}
}
