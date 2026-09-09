using Godot;
using System;

public partial class Main : Node3D
{
	[Export] public NodePath LinearCurvePath;
	[Export] public NodePath BezierCurvePath;
	[Export] public NodePath CatmullRomCurvePath;
	[Export] public NodePath RatoPath;
	[Export] public NodePath CurveLabelPath;

	private Node3D linearCurve;
	private Node3D bezierCurve;
	private Node3D catmullRomCurve;

	private Rato rato;
	private Label curveLabel;

	public override void _Ready()
	{
		GD.Print("Cena principal carregada!");

		linearCurve = GetNodeOrNull<Node3D>(LinearCurvePath);
		bezierCurve = GetNodeOrNull<Node3D>(BezierCurvePath);
		catmullRomCurve = GetNodeOrNull<Node3D>(CatmullRomCurvePath);

		rato = GetNodeOrNull<Rato>(RatoPath);
		curveLabel = GetNodeOrNull<Label>(CurveLabelPath);

		// Espera as curvas terminarem de gerar seus pontos
		// antes de selecionar a trajetória inicial.
		CallDeferred(nameof(SelectLinear));
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent &&
			keyEvent.Pressed &&
			!keyEvent.Echo)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Key1:
					SelectLinear();
					break;

				case Key.Key2:
					SelectBezier();
					break;

				case Key.Key3:
					SelectCatmullRom();
					break;
			}
		}
	}

	private void SelectLinear()
	{
		SetActiveCurve(linearCurve);

		UpdateInterface(
            "Linear"
		);

		GD.Print("Trajetória selecionada: LINEAR");
	}

	private void SelectBezier()
	{
		SetActiveCurve(bezierCurve);

		UpdateInterface(
            "Bézier Cúbica"
		);

		GD.Print("Trajetória selecionada: BÉZIER CÚBICA");
	}

	private void SelectCatmullRom()
	{
		SetActiveCurve(catmullRomCurve);

		UpdateInterface(
            "Catmull-Rom"
		);

		GD.Print("Trajetória selecionada: CATMULL-ROM");
	}

	private void SetActiveCurve(Node3D activeCurve)
	{
		if (activeCurve == null)
		{
			GD.Print("Curva selecionada não foi encontrada.");
			return;
		}

		if (linearCurve != null)
		{
			linearCurve.Visible =
				activeCurve == linearCurve;
		}

		if (bezierCurve != null)
		{
			bezierCurve.Visible =
				activeCurve == bezierCurve;
		}

		if (catmullRomCurve != null)
		{
			catmullRomCurve.Visible =
				activeCurve == catmullRomCurve;
		}

		if (rato != null)
		{
			rato.SetCurve(activeCurve);
		}
		else
		{
			GD.Print("Nó do rato não foi encontrado.");
		}
	}

	private void UpdateInterface(string activeCurveName)
	{
		if (curveLabel == null)
		{
			return;
		}

		curveLabel.Text =
			"TRAJETÓRIAS\n\n" +
			"[1] Linear\n" +
			"[2] Bézier Cúbica\n" +
			"[3] Catmull-Rom\n\n" +
			"Ativa: " + activeCurveName;
	}
}
