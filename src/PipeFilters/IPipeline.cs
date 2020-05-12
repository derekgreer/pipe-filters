using System;

namespace PipeFilters
{
    public interface IPipeline<TData>
    {
        void UseFilter(Action<TData, Action<TData>> filter);

        void UseFilter<TPipelineFilter>() where TPipelineFilter : IPipelineFilter<TData>;

        void UseFilter<TPipelineFilter>(string name) where TPipelineFilter : IPipelineFilter<TData>;

        void Execute(TData data);
    }
}