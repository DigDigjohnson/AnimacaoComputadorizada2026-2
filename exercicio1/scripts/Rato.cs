using Godot;
using System;
using System.Collections.Generic;

public partial class Rato : Node3D
{
	[Export] public NodePath CurveNodePath;
	[Export] public float Speed = 5f;

	private List<Node3D> curvePoints = new();
	private int currentIndex = 0;

	public override void _Ready()
	{
		GD.Print("Rato acordou e está pronto para caçar queijo!");

		CallDeferred(nameof(SetupPath));
	}

	private void SetupPath()
	{
		var curveNode = GetNodeOrNull<Node3D>(CurveNodePath);

		if (curveNode == null)
		{
			GD.Print("Nó da curva não encontrado!");
			return;
		}

		LoadCurve(curveNode);
	}

	// Permite trocar a trajetória durante a execução
	public void SetCurve(Node3D curveNode)
	{
		if (curveNode == null)
		{
			GD.Print("Não foi possível trocar a curva: nó inválido.");
			return;
		}

		LoadCurve(curveNode);
	}

	private void LoadCurve(Node3D curveNode)
	{
		// Remove referências da curva anterior
		curvePoints.Clear();

		currentIndex = 0;

		// Coleta os pontos gerados pela nova curva
		foreach (Node child in curveNode.GetChildren())
		{
			if (child is Node3D point)
			{
				curvePoints.Add(point);
			}
		}

		GD.Print(
			$"Rato recebeu a curva {curveNode.Name} " +
			$"com {curvePoints.Count} pontos."
		);

		// Coloca o rato no começo da nova trajetória
		if (curvePoints.Count > 0)
		{
			GlobalPosition = curvePoints[0].GlobalPosition;
		}
	}

	public override void _Process(double delta)
	{
		if (curvePoints.Count < 2 ||
			currentIndex >= curvePoints.Count - 1)
		{
			return;
		}

		var from = GlobalPosition;
		var to = curvePoints[currentIndex + 1].GlobalPosition;

		var toVector = to - from;
		var distanceToNext = toVector.Length();

		if (distanceToNext < 0.01f)
		{
			currentIndex++;
			return;
		}

		var direction = toVector.Normalized();
		var step = Speed * (float)delta;

		if (step >= distanceToNext)
		{
			GlobalPosition = to;
			currentIndex++;
		}
		else
		{
			GlobalPosition += direction * step;
			LookAt(to, Vector3.Up);
		}
	}
}
