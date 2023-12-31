public class Simulation {

	public Scene Scene = new Scene();
	public Agent[] Agents = new Agent[Settings.AgentCount];

	Random rnd = new Random();

	public Simulation() {

		for (int i = 0; i < Settings.AgentCount; i++) {
			float h = (float)(2 * MathF.PI * rnd.NextDouble());
			Agents[i] = new Agent(rnd.Next(Settings.Width), rnd.Next(Settings.Height), h, ref rnd);
		}
	}

	public void Step() {
		Scene.Decay();

		ShuffleAgents(ref Agents);
		foreach (Agent a in Agents) {
			a.Sense(Scene);
			a.Move(ref Scene);
		}

		for (int i = Agents.Length -1; i >= 0; i--) {
			switch (Agents[i].State) {
			case Agent.Action.Skip:
				continue;

			case Agent.Action.Delete:
				Scene.Grid[Agents[i].X, Agents[i].Y].IsOccupied = false;
				RemoveAgent(ref Agents, i);
				break;

			case Agent.Action.Spawn:

				int tries = 0;
				while (tries < 18) {
					int x = Agents[i].X + (rnd.Next(3) -1);
					int y = Agents[i].Y + (rnd.Next(3) -1);
					if (Scene.IsOutOfBounds(x, y)) {
						continue;
					}

					if (!Scene.Grid[x, y].IsOccupied) {
						float heading = (float)(2 * MathF.PI * rnd.NextDouble());
						Agents = Agents.Append(new Agent(x, y, heading, ref rnd)).ToArray();
						Scene.Grid[x, y].IsOccupied = true;
						break;
					}
					tries++;
				}
				break;
			}
		}
		return;
	}

	public void RemoveAgent(ref Agent[] source, int index) {
		for (int i = index; i < source.Length -1; i++) {
			source[i] = source[i+1];
		}
		Array.Resize(ref source, source.Length -1);
		return;
	}

	public void ShuffleAgents(ref Agent[] source)
	{
		Random random = new Random();

		for (int i = source.Length - 1; i > 0; i--)
		{
			int j = random.Next(0, i + 1);

			Agent temp = source[i];
			source[i] = source[j];
			source[j] = temp;
		}
		return;
	}
}

