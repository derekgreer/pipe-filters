namespace PipeFilters
{
    public interface IPipelineFilterResolver<TData>
    {
        IPipelineFilter<TData> Resolve(PipelineFilterInfo pipelineFilterInfo);
    }
}