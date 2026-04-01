namespace APIwebbanhangnhe.Models.ViewModels;

public class HomeIndexViewModel
{
    public IReadOnlyCollection<Category> Categories { get; init; } = Array.Empty<Category>();

    public IReadOnlyCollection<PlayerCard> FeaturedCards { get; init; } = Array.Empty<PlayerCard>();
}
