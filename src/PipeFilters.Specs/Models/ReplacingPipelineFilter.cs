using System;

namespace PipeFilters.Specs.Models
{
    public class ReplacingPipelineFilter<T> : IPipelineFilter<T>
    {
        readonly T _value;

        public ReplacingPipelineFilter(T value)
        {
            _value = value;
        }

        public void Execute(T data, Action<T> next)
        {
            next(_value);
        }
    }
}