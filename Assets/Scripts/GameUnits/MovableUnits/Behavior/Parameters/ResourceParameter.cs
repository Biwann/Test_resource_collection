public sealed class ResourceParameter : IBehaviorStrategyParameter
{
    public ResourceParameter(Resource resource)
    {
        Resource = resource;
    }

    public Resource Resource { get; }
}