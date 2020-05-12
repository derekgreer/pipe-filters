using System;

namespace PipeFilters.Specs.Models
{
    public class SpyPipelineFilter<T> : IPipelineFilter<T>
    {
        public T Value { get; set; }

        public void Execute(T data, Action<T> next)
        {
            Value = data;
        }
    }
}