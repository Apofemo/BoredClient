using BoredClient.Models;
using Microsoft.AspNetCore.Components;

namespace BoredClient.Components;

public partial class BoredCard
{
    [Parameter]
    [EditorRequired]
    public BoredResult Data { get; set; }

    [Parameter]
    [EditorRequired]
    public EventCallback OnBookmarkAddClick { get; set; }

    [Parameter]
    public bool IsFavoriteItem { get; set; }

    private int Accessibility { get; set; }

    private int Price { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (Data.Accessibility <= 0.1f)
            Accessibility = 1;
        else if (Data.Accessibility <= 0.3f)
            Accessibility = 2;
        else if (Data.Accessibility <= 0.5f)
            Accessibility = 3;
        else if (Data.Accessibility <= 0.7f)
            Accessibility = 4;
        else
            Accessibility = 5;

        if (Data.Price < 0.1f)
            Price = 1;
        else if (Data.Price < 0.2f)
            Price = 2;
        else if (Data.Price < 0.3f)
            Price = 3;
        else if (Data.Price < 0.4f)
            Price = 4;
        else
            Price = 5;
    }
}
