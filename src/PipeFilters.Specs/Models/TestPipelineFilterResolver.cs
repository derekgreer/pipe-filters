using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeFilters.Specs.Models
{
    public class TestPipelineFilterResolver<T> : IPipelineFilterResolver<T>
    {
        readonly IList<Registration> _registrations = new List<Registration>();

        public IPipelineFilter<T> Resolve(PipelineFilterInfo pipelineFilterInfo)
        {
            return (IPipelineFilter<T>) _registrations
                .FirstOrDefault(r =>
                    r.Type == pipelineFilterInfo.Type
                    && r.Name == pipelineFilterInfo.Name)
                ?.Instance;
        }

        public void RegisterType<TFilter>(TFilter filter, string name = null)
        {
            _registrations.Add(new Registration {Type = typeof(TFilter), Name = name, Instance = filter});
        }

        class Registration
        {
            public Type Type { get; set; }
            public string Name { get; set; }
            public object Instance { get; set; }
        }
    }
}