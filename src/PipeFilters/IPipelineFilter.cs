using System;

namespace PipeFilters
{
    public interface IPipelineFilter<TData>
    {
        void Execute(TData data, Action<TData> next);
    }
}