using BoredClient.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;

namespace BoredClient.Components;

public partial class FavoritesDialog
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Parameter]
    public string FavoritesDir { get; set; }

    private List<BoredResult> Favorites = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var savedFavoritesText = await File.ReadAllTextAsync(FavoritesDir);

        Favorites = JsonConvert.DeserializeObject<List<BoredResult>>(savedFavoritesText);
    }

    private async Task RemoveFromFavorites(int key)
    {
        var itemToRemove = Favorites.FirstOrDefault(fav => fav.Key == key);

        if (itemToRemove == null)
            Snackbar.Add("Item can't be removed", Severity.Error);
        else
        {
            Favorites.Remove(itemToRemove);

            var serializedData = JsonConvert.SerializeObject(Favorites);

            await File.WriteAllTextAsync(FavoritesDir, serializedData);
        }

        if (!Favorites.Any())
            MudDialog.Cancel();
    }
}
