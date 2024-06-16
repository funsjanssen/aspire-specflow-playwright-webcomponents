using Microsoft.Playwright;

namespace AspireWebComponents.Tests.Tests.Pages;

public class ForecastPage(Hooks.Hook hooks)
{
    private readonly IPage _user = hooks.User;
    
    private ILocator BarChart => _user.Locator("svg[data-testid=forecast-chart]");

    public async Task AssertBarChart()
    {
        await Assertions.Expect(_user).ToHaveURLAsync(hooks.WebFrontendUri);
        await Assertions.Expect(BarChart).ToBeVisibleAsync();
    }
}