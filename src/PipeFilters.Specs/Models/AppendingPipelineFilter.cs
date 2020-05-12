using System;
using System.Text;

namespace PipeFilters.Specs.Models
{
    public class AppendingPipelineFilter : IPipelineFilter<StringBuilder>
    {
        readonly string _value;

        public AppendingPipelineFilter(string value)
        {
            _value = value;
        }

        public void Execute(StringBuilder data, Action<StringBuilder> next)
        {
            data.Append(_value);
            next(data);
        }
    }
}