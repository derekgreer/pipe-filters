using Autofac;

namespace PipeFilters.Autofac
{
    public class AutofacPipelineFilterResolver<TData> : IPipelineFilterResolver<TData>
    {
        readonly IComponentContext _componentContext;

        public AutofacPipelineFilterResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public IPipelineFilter<TData> Resolve(PipelineFilterInfo pipelineFilterInfo)
        {
            if (!string.IsNullOrWhiteSpace(pipelineFilterInfo.Name))
            {
                return (IPipelineFilter<TData>)_componentContext.ResolveNamed(pipelineFilterInfo.Name, pipelineFilterInfo.Type);
            }
            else
            {
                return (IPipelineFilter<TData>) _componentContext.Resolve(pipelineFilterInfo.Type);
            }
        }
    }
}