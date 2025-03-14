﻿namespace Microsoft.NetConf2021.Maui.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public class CategoryViewModel : BaseViewModel
{
    private string text;

    public string Id { get; set; }

    private Category category;
    public Category Category
    {
        get { return category; }
        set { SetProperty(ref category, value); }
    }

    private List<ShowViewModel> shows;
    private readonly ShowsService showsService;
    private readonly SubscriptionsService subscriptionsService;

    public List<ShowViewModel> Shows
    {
        get { return shows; }
        set { SetProperty(ref shows, value); }
    }

    public string Text
    {
        get { return text; }
        set
        {
            SetProperty(ref text, value);
        }
    }

    public ICommand SubscribeCommand {get; set;}
    public ICommand SearchCommand { get; set; }

    public CategoryViewModel(ShowsService shows, SubscriptionsService subs)
    {
        showsService = shows;
        subscriptionsService = subs;

        SubscribeCommand = new AsyncCommand<ShowViewModel>(SubscribeCommandExecute);
        SearchCommand = new MvvmHelpers.Commands.Command(OnSearchCommand);
    }


    public async Task InitializeAsync()
    {
        await LoadCategoryAsync();
        var shows = await showsService.GetShowsByCategoryAsync(new Guid(Id));

        Shows = LoadShows(shows);
    }

    private async Task LoadCategoryAsync()
    {
        var allCategories = await showsService.GetAllCategories();
        Category = allCategories?.First(c => c.Id == new Guid(Id));
    }

    private async Task SubscribeCommandExecute(ShowViewModel vm)
    {
        await subscriptionsService.UnSubscribeFromShowAsync(vm.Show);
        OnPropertyChanged(nameof(vm.IsSubscribed));
    }

    private async void OnSearchCommand()
    {
        var shows = await showsService.SearchShowsAsync(new Guid(Id), Text);
        Shows = LoadShows(shows);
    }

    private List<ShowViewModel> LoadShows(IEnumerable<Show> shows)
    {
        var showList = new List<ShowViewModel>();
        if (shows == null)
        {
            return showList;
        }

        foreach (var show in shows)
        {
            var showVM = new ShowViewModel(show, subscriptionsService.IsSubscribed(show.Id));
            showList.Add(showVM);
        }

        return showList;
    }
}
