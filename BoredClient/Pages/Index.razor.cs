using BoredClient.Components;
using BoredClient.Models;
using BoredClient.Models.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BoredClient.Pages;

public partial class Index
{
	[Inject]
	private IDialogService DialogService { get; set; }

	[Inject]
	private ISnackbar _snackbar { get; set; }

	private string URL { get; } = "https://www.boredapi.com/api/activity";

	private string FavoritesDir { get; } = FileSystem.Current.AppDataDirectory + "\\Favorites";

	private HttpClient Client { get; set; } = new();

	private BoredResult Result { get; set; }

	private FilterPreferences FilterPreferences { get; set; } = new();

	private bool IsSearching { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		Client.BaseAddress = new Uri(URL);

		Client.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue("application/json"));
	}

	private async Task GetActivity()
	{
		try
		{
			if (!IsSearching)
			{
				IsSearching = true;

				var response = await Client.GetAsync(GetUrlParameters());

				if (response.IsSuccessStatusCode)
				{
					Result = JsonConvert.DeserializeObject<BoredResult>(await response.Content.ReadAsStringAsync());

					Result.Saved = await IsAlreadySaved();
				}
				else
					_snackbar.Add("API doesn't respond", Severity.Error);
			}
		}
		catch
		{
			_snackbar.Add("API doesn't respond", Severity.Error);
		}
		finally
		{
			IsSearching = false;
		}
	}

	private string GetUrlParameters()
	{
		var result = "?";

		if (FilterPreferences.SelectedType != BoredType.Any)
			result += "&type=" + FilterPreferences.SelectedType.ToString().ToLower();

		if (FilterPreferences.SelectedPrice != PriceSelectedType.Any)
			result += "&price=" + FilterPreferences.SelectedPrice switch
			{
				PriceSelectedType.Free => "0",
				PriceSelectedType.Cheap => "0.1",
				PriceSelectedType.Low => "0.2",
				PriceSelectedType.Normal => "0.3",
				PriceSelectedType.Expensive => "0.4",
				_ => string.Empty
			};

		if (FilterPreferences.SelectedAccessibility != AccessibilitySelectedType.Any)
			result += FilterPreferences.SelectedAccessibility switch
			{
				AccessibilitySelectedType.Easy => "&minaccessibility=0.0&maxaccessibility=0.1",
				AccessibilitySelectedType.Easier => "&minaccessibility=0.2&maxaccessibility=0.3",
				AccessibilitySelectedType.Normal => "&minaccessibility=0.4&maxaccessibility=0.5",
				AccessibilitySelectedType.Harder => "&minaccessibility=0.6&maxaccessibility=0.7",
				AccessibilitySelectedType.Hard => "&minaccessibility=0.8&maxaccessibility=1",
				_ => string.Empty,
			};

		if (FilterPreferences.SelectedParticipants != 0)
			result += "&participants=" + FilterPreferences.SelectedParticipants;

		return result;
	}

	private async Task OpenPreferencesDialog()
	{
		var parameters = new DialogParameters
		{
			["Preferences"] = new FilterPreferences
			{
				SelectedType = FilterPreferences.SelectedType,
				SelectedAccessibilityInt = FilterPreferences.SelectedAccessibilityInt,
				SelectedParticipants = FilterPreferences.SelectedParticipants,
				SelectedPriceInt = FilterPreferences.SelectedPriceInt
			}
		};

		var options = new DialogOptions { FullWidth = true };
		var dialog = DialogService.Show<PreferencesDialog>("Filtration", parameters, options);

		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			FilterPreferences = (FilterPreferences)result.Data;
		}
	}

	private async Task OpenFavoritesDialog()
	{
		var favorites = await GetSavedFavorites();

		if (!favorites.Any())
		{
			_snackbar.Add("Nothing saved", Severity.Info);
			return;
		}

		var parameters = new DialogParameters
		{
			["FavoritesDir"] = FavoritesDir
		};

		var options = new DialogOptions { FullWidth = true, Position = DialogPosition.TopCenter, NoHeader = true };
		var dialog = DialogService.Show<FavoritesDialog>("Favorites", parameters, options);

		var result = await dialog.Result;

		if (result.Cancelled && Result != null)
		{
			Result.Saved = await IsAlreadySaved();
		}
	}

	private async Task SaveToFavorites()
	{
		if (File.Exists(FavoritesDir))
		{
			if (Result.Saved)
			{
				_snackbar.Add("Already saved", Severity.Info);
				return;
			}

			Result.Saved = true;

			var favorites = await GetSavedFavorites();

			favorites.Add(Result);

			var serializedData = JsonConvert.SerializeObject(favorites);

			await File.WriteAllTextAsync(FavoritesDir, serializedData);
		}
		else
		{
			var serializedData = JsonConvert.SerializeObject(new List<BoredResult> { Result });

			Result.Saved = true;

			await File.WriteAllTextAsync(FavoritesDir, serializedData);
		}
	}

	private async Task<bool> IsAlreadySaved()
	{
		if (!File.Exists(FavoritesDir))
			File.Create(FavoritesDir);

		return (await GetSavedFavorites()).Any(fav => fav.Key == Result.Key);
	}

	private async Task<List<BoredResult>> GetSavedFavorites()
	{
		if (!File.Exists(FavoritesDir))
			File.Create(FavoritesDir);

		var savedFavoritesText = await File.ReadAllTextAsync(FavoritesDir);

		return JsonConvert.DeserializeObject<List<BoredResult>>(savedFavoritesText) ?? new List<BoredResult>();
	}
}
