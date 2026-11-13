public class GuildCard
{
    private GuildCardData _data;
    private HeroType _heroType;
    private RoomType _roomType;

    public HeroType HeroType => _heroType;
    public RoomType RoomType => _roomType;

    public GuildCard(GuildCardData data)
    {
        _data = data;
        _heroType = data.SummonHero;
        _roomType = data.SummonRoom;
    }
}
