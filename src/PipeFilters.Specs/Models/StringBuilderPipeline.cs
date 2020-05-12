using System.Text;

namespace PipeFilters.Specs.Models
{
    public class StringBuilderPipeline : Pipeline<StringBuilder>
    {
        public StringBuilderPipeline(TestPipelineFilterResolver<StringBuilder> testPipelineFilterResolver) : base(
            testPipelineFilterResolver)
        {
        }
    }
}