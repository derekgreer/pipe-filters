using System.Text;
using Machine.Specifications;
using PipeFilters.Specs.Models;

namespace PipeFilters.Specs
{
    public class PipelineSpecs
    {
        [Subject("Pipeline")]
        public class when_executing_a_pipeline_registered_with_a_mutating_filter
        {
            static IPipeline<StringBuilder> _pipeline;
            static StringBuilder _builder;

            Establish context = () =>
            {
                var testPipelineFilterResolver = new TestPipelineFilterResolver<StringBuilder>();
                testPipelineFilterResolver.RegisterType(new AppendingPipelineFilter("."));
                _pipeline = new StringBuilderPipeline(testPipelineFilterResolver);
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _builder = new StringBuilder();
            };

            Because of = () => _pipeline.Execute(_builder);

            It should_mutate_the_data = () => _builder.ToString().ShouldEqual("...");
        }

        [Subject("Pipeline")]
        public class when_executing_a_pipeline_registered_with_a_named_filter
        {
            static IPipeline<int> _pipeline;
            static SpyPipelineFilter<int> _filterSpy;

            Establish context = () =>
            {
                var testPipelineFilterResolver = new TestPipelineFilterResolver<int>();
                testPipelineFilterResolver.RegisterType(new ReplacingPipelineFilter<int>(1), "named");
                testPipelineFilterResolver.RegisterType(new ReplacingPipelineFilter<int>(2));
                _filterSpy = new SpyPipelineFilter<int>();
                testPipelineFilterResolver.RegisterType(_filterSpy);
                _pipeline = new Pipeline<int>(testPipelineFilterResolver);
                _pipeline.UseFilter<ReplacingPipelineFilter<int>>("named");
                _pipeline.UseFilter<SpyPipelineFilter<int>>();
            };

            Because of = () => _pipeline.Execute(0);

            It should_use_the_named_filter = () => _filterSpy.Value.ShouldEqual(1);
        }

        [Subject("Pipeline")]
        public class when_executing_a_pipeline_registered_with_a_replacing_filter
        {
            static IPipeline<int> _pipeline;
            static SpyPipelineFilter<int> _filterSpy;

            Establish context = () =>
            {
                var testPipelineFilterResolver = new TestPipelineFilterResolver<int>();
                testPipelineFilterResolver.RegisterType(new ReplacingPipelineFilter<int>(1), "first");
                testPipelineFilterResolver.RegisterType(new ReplacingPipelineFilter<int>(2), "second");
                _filterSpy = new SpyPipelineFilter<int>();
                testPipelineFilterResolver.RegisterType(_filterSpy);
                _pipeline = new Pipeline<int>(testPipelineFilterResolver);
                _pipeline.UseFilter<ReplacingPipelineFilter<int>>("first");
                _pipeline.UseFilter<ReplacingPipelineFilter<int>>("second");
                _pipeline.UseFilter<SpyPipelineFilter<int>>();
            };

            Because of = () => _pipeline.Execute(0);

            It should_replace_the_data = () => _filterSpy.Value.ShouldEqual(2);
        }

        [Subject("Pipeline")]
        public class when_executing_a_pipeline_registered_with_a_short_circuiting_filter
        {
            static IPipeline<StringBuilder> _pipeline;
            static StringBuilder _builder;

            Establish context = () =>
            {
                var testPipelineFilterResolver = new TestPipelineFilterResolver<StringBuilder>();
                testPipelineFilterResolver.RegisterType(new AppendingPipelineFilter("."));
                testPipelineFilterResolver.RegisterType(new ShortCircuitPipelineFilter());
                _pipeline = new StringBuilderPipeline(testPipelineFilterResolver);
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _pipeline.UseFilter<ShortCircuitPipelineFilter>();
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _pipeline.UseFilter<AppendingPipelineFilter>();
                _builder = new StringBuilder();
            };

            Because of = () => _pipeline.Execute(_builder);

            It should_short_circuit = () => _builder.ToString().ShouldEqual("...");
        }

        [Subject("Pipeline")]
        public class when_executing_a_pipeline_registered_with_a_delegate_filter
        {
            static IPipeline<int> _pipeline;
            static bool _wasCalled;

            Establish context = () =>
            {
                _pipeline = new Pipeline<int>(null);
                _pipeline.UseFilter((data, next) => _wasCalled = true);
            };

            Because of = () => _pipeline.Execute(0);

            It should_use_the_delegate_filter = () => _wasCalled.ShouldBeTrue();
        }
    }
}