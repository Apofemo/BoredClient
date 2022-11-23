using BoredClient.Models.Enums;

namespace BoredClient.Models;

public sealed record BoredResult
{
	public int Key { get; init; }
	public string Activity { get; init; } = string.Empty;
	public BoredType Type { get; init; }
	public int Participants { get; init; }
	public float Price { get; init; }
	public string Link { get; init; }
	public float Accessibility { get; init; }
	public bool Saved { get; set; }
}
