public class Resource
{
    private ResourceType _type;

    public ResourceType Type => _type;

    public Resource(ResourceType type)
    {
        _type = type;
    }
}
