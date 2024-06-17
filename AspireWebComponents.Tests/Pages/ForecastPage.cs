using Microsoft.Playwright;

namespace AspireWebComponents.Tests.Tests.Pages;

public class ForecastPage(Hooks.Hook hooks)
{
    private readonly IPage _user = hooks.User;
    
    private ILocator BarChart => _user.Locator("svg[data-testid=forecast-chart]");
    
    private ILocator LoadButton => _user.Locator("button[data-testid=load-button]");

    public async Task GoToPageAsync()
    {
        await _user.GotoAsync(hooks.WebFrontendUri);
        await Assertions.Expect(_user).ToHaveURLAsync(hooks.WebFrontendUri);
    }
    
    public async Task AssertBarChartAsync()
    {
        await Assertions.Expect(BarChart).ToBeVisibleAsync();
    }
    
    public async Task ClickLoadButtonAsync()
    {
        await LoadButton.ClickAsync();
    }
}