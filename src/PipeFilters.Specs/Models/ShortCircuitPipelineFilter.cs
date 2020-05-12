using System;
using System.Text;

namespace PipeFilters.Specs.Models
{
    public class ShortCircuitPipelineFilter : IPipelineFilter<StringBuilder>
    {
        public void Execute(StringBuilder data, Action<StringBuilder> next)
        {
            // do nothing
        }
    }
}