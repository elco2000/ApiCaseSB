using System.Collections.Generic;

namespace ApiCase.Model.ApicallGeometry
{
    public class Geometry
    {
        public List<string> coordinates { get; set; } = new List<string>();
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
    }

    public partial class Root
    {
        public List<Feature> features { get; set; }
    }
}
