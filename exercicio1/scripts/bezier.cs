using Godot;
using System;

public partial class bezier : Curve3D
{
	private Vector3 CalculateBezier(
		Vector3 p0,
		Vector3 p1,
		Vector3 p2,
		Vector3 p3,
		float t)
	{
		float u = 1.0f - t;

		return
			u * u * u * p0 +
			3.0f * u * u * t * p1 +
			3.0f * u * t * t * p2 +
			t * t * t * p3;
	}

	public override void GenerateCurvePoints(int curveRes)
	{
		ClearCurvePoints();

		for (int i = 0; i + 3 < controlPoints.Count; i += 3)
		{
			Vector3 p0 = controlPoints[i].GlobalPosition;
			Vector3 p1 = controlPoints[i + 1].GlobalPosition;
			Vector3 p2 = controlPoints[i + 2].GlobalPosition;
			Vector3 p3 = controlPoints[i + 3].GlobalPosition;

			for (int j = 0; j <= curveRes; j++)
			{
				float t = (float)j / curveRes;

				Vector3 point = CalculateBezier(
					p0, p1, p2, p3, t
				);

				AddCurvePoint(point);
			}
		}
	}
}
