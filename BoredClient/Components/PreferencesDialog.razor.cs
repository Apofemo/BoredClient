using BoredClient.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BoredClient.Components;

public partial class PreferencesDialog
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public FilterPreferences Preferences { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Preferences == null)
            Preferences = new();
    }

    void Submit() 
        => MudDialog.Close(DialogResult.Ok<FilterPreferences>(Preferences));

    void Cancel() 
        => MudDialog.Cancel();

    void Clear()
        => Preferences = new();
}
