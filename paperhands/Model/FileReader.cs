using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using paperhands.ViewModel;
using Wpf.Ui;

namespace paperhands.Model;

public class FileReader(string dataPath, MainWindowViewModel _mainWindowViewModel)
{
    private readonly string DataPath = dataPath;

    public async Task WriteToFileAsync(ObservableCollection<QuestionPackViewModel> packs)
    {
        try
        {
            var json = JsonSerializer.Serialize(packs, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(DataPath, json);
        }
        catch
        {
           _mainWindowViewModel.ShowErrorSnackbarMessage("Error", "Could not write to file, is it open in another program?");
        }
    }

    public async Task<ObservableCollection<QuestionPackViewModel>> ReadFromFileAsync()
    {
        if (!File.Exists(DataPath))
        {
            ObservableCollection<QuestionPackViewModel> newPackCollection = [];
            var newPack = new QuestionPackViewModel(new QuestionPack());
            newPack.Questions.Add(new Question("Why is the sky so blue?", "Dont worry about it!",
                "Blue is not a color!", "What about the colorblind?", "Something with light."));
            newPackCollection.Add(newPack);

            await WriteToFileAsync(newPackCollection);
        }

        try
        {
            var json = File.ReadAllTextAsync(DataPath);
            var packs =
                JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json.GetAwaiter().GetResult());
            return packs;
        }
        catch
        {
            _mainWindowViewModel.ShowErrorSnackbarMessage("Error", "Could not read from file!");
        }

        return null;
    }
}