
public class Agent {
	public int X;
	public int Y;
	public float Heading;
	public float PheremoneStrength;

	float realX;
	float realY;

	public enum Action {
		Skip,
		Delete,
		Spawn
	}
	
	public Action State;
	Random rnd;

	public Agent(int xStart, int yStart, float heading, ref Random rrnd) {
		X = xStart;
		Y = yStart;
		
		realX = X;
		realY = Y;

		rnd = rrnd;
		Heading = (float)rnd.NextDouble() * 2 * MathF.PI;

	}

	private struct sensor {
		public float Value;
		public int ID;
	}

	public void Sense(Scene scene) {
		sensor[] sensors = new sensor[3];
		int height = scene.Grid[X, Y].Height;

		for (int i = 0; i < 3; i++) {
			sensors[i].ID = i;

			float angle = Heading + (i - 1) * Settings.AgentSensorAngle;
			int x = (int)(realX + MathF.Cos(angle) * Settings.AgentSensorDistance);
			int y = (int)(realY + MathF.Sin(angle) * Settings.AgentSensorDistance);

			if (scene.IsOutOfBounds(x, y)) {
				sensors[i].Value = float.MinValue;
				continue;
			}
			sensors[i].Value = scene.Grid[x, y].Evaluate(height);
		}

		Array.Sort(sensors, (a, b) => b.Value.CompareTo(a.Value));
	
		if (sensors[0].Value == float.MinValue) {
			// random dir
			Heading += (rnd.Next(3) - 1) * Settings.AgentSensorAngle;
		}
		else if (sensors[0].Value == sensors[1].Value) {
			// chose random of top 2
			Heading += (sensors[rnd.Next(1)].ID - 1) * Settings.AgentSensorAngle;
		}
		else {
			Heading += (sensors[0].ID - 1) * Settings.AgentSensorAngle;
		}

		return;
	}
	
	public void Move(ref Scene scene) {
		State = Action.Skip;

		float tmpfX = realX + MathF.Cos(Heading) * Settings.AgentSpeed;
		float tmpfY = realY + MathF.Sin(Heading) * Settings.AgentSpeed;
		int tmpX = (int)Math.Round(tmpfX, 1);
		int tmpY = (int)Math.Round(tmpfY, 1);


		int nCount = scene.GetNeighbourCount(X, Y, 5);
		if (nCount > 15) {
			State = Action.Delete;
			return;
		}

		if (scene.IsOutOfBounds(tmpX, tmpY) || scene.Grid[tmpX, tmpY].IsOccupied) {
			Heading = (float)rnd.NextDouble() * 2 * MathF.PI;
			return;
		}

		scene.Grid[X, Y].IsOccupied = false;
		X = tmpX;
		Y = tmpY;
		scene.Grid[X, Y].IsOccupied = true;
		realX = tmpfX;
		realY = tmpfY;

		scene.Grid[X, Y].PheremoneStrength += 5;

		nCount = scene.GetNeighbourCount(X, Y, 9);
		if (nCount <= 4) {
			State = Action.Spawn;
			return;
		}
		return;
	}
}
