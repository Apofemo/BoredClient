using Microsoft.AspNetCore.Components;

namespace BoredClient.Components;

public partial class LineLoader
{
	[Parameter]
	[EditorRequired]
	public bool IsActive { get; set; }

	private static bool ShowLine { get; set; }

	private static TimeSpan ActiveTime { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		if (IsActive)
		{
			if (ShowLine && DateTime.Now.TimeOfDay - ActiveTime > TimeSpan.FromSeconds(0))
				ActiveTime = ActiveTime.Add(TimeSpan.FromSeconds(1));
			else
			{
				ActiveTime = DateTime.Now.TimeOfDay;
				ShowLine = true;
			}
		}
		else
		{
			while (DateTime.Now.TimeOfDay - ActiveTime < TimeSpan.FromSeconds(1f))
				await Task.Delay(10);

			ShowLine = false;
		}
	}
}
