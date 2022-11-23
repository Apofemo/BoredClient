using BoredClient.Models.Enums;

namespace BoredClient.Models;

public sealed record FilterPreferences
{
	public BoredType SelectedType { get; set; }
	public PriceSelectedType SelectedPrice { get; set; }
	public AccessibilitySelectedType SelectedAccessibility { get; set; }
	public int SelectedParticipants { get; set; }
	
	public int SelectedPriceInt
	{
		get => (int)SelectedPrice;
		set => SelectedPrice = (PriceSelectedType)value;
	}

	public int SelectedAccessibilityInt
	{
		get => (int)SelectedAccessibility;
		set => SelectedAccessibility = (AccessibilitySelectedType)value;
	}
}
