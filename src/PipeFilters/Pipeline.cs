using System;
using System.Collections.Generic;

namespace PipeFilters
{
    public class Pipeline<TData> : IPipeline<TData>
    {
        readonly IList<Func<Action<TData, Action<TData>>>> _filterResolvers = new List<Func<Action<TData, Action<TData>>>>();
        readonly IPipelineFilterResolver<TData> _pipelineFilterResolver;
        
        public Pipeline(IPipelineFilterResolver<TData> pipelineFilterResolver)
        {
            _pipelineFilterResolver = pipelineFilterResolver;
        }

        public void UseFilter<TPipelineFilter>() where TPipelineFilter : IPipelineFilter<TData>
        {
            _filterResolvers.Add(() => _pipelineFilterResolver.Resolve(new PipelineFilterInfo { Type = typeof(TPipelineFilter) }).Execute);
        }

        public void UseFilter<TPipelineFilter>(string name) where TPipelineFilter : IPipelineFilter<TData>
        {
            _filterResolvers.Add(() => _pipelineFilterResolver.Resolve(new PipelineFilterInfo { Type = typeof(TPipelineFilter), Name = name }).Execute);
        }

        public void UseFilter(Action<TData, Action<TData>> filter)
        {
            _filterResolvers.Add(() => filter);
        }

        public void Execute(TData data)
        {
            Execute(data, d => { });
        }

        void Execute(TData data, Action<TData> finalFilter)
        {
            var current = 0;

            void NextFilter(TData scopeData)
            {
                Action<TData> nextFilter;
                if (current < _filterResolvers.Count)
                {
                    var filterResolver = _filterResolvers[current++];
                    var filter = filterResolver();
                    nextFilter = p => { filter(scopeData, NextFilter); };
                }
                else
                {
                    nextFilter = finalFilter;
                }

                nextFilter(scopeData);
            }

            NextFilter(data);
        }
    }
}