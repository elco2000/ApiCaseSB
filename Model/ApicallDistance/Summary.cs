using System.Collections.Generic;

namespace ApiCase.Model.ApicallDistance
{
    public class Summary
    {
        public double distance { get; set; }
    }

    public class Properties
    {
        public Summary summary { get; set; }
    }

    public class Feature
    {
        public Properties properties { get; set; }
    }

    public class Root
    {
        public List<Feature> features { get; set; }
    }
}
