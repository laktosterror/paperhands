using System.Net.Http;
using System.Text.Json;

namespace paperhands.Model;

public class OpenTriviaClient
{
    private readonly HttpClient httpClient = new();
    private readonly string OpenTriviaUriBase = "https://opentdb.com/";
    private readonly string OpenTriviaUriCategoryPath = "api_category.php";

    public async Task<OpenTriviaCategories> LoadCategoriesAsync()
    {
        var response = await httpClient.GetStringAsync("https://opentdb.com/api_category.php");
        var openTriviaCategories = JsonSerializer.Deserialize<OpenTriviaCategories>(response);
        return openTriviaCategories;
    }

    public async Task<OpenTriviaQuestions> GetQuestionsAsync(int SelectedAmount, int SelectedCategory,
        string SelectedDifficulty)
    {
        var response = await httpClient
            .GetStringAsync(OpenTriviaUriBase +
                            $"api.php?amount={SelectedAmount}&category={SelectedCategory}&difficulty={SelectedDifficulty.ToLower()}&type=multiple");

        var openTriviaquestions = JsonSerializer.Deserialize<OpenTriviaQuestions>(response);
        return openTriviaquestions;
    }
}