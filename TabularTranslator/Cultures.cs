using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabularTranslator
{

    public class ModelTranslations
    {
        public Referenceculture ReferenceCulture { get; set; }
        public Culture[] cultures { get; set; }
    }

    public class Referenceculture
    {
        public string name { get; set; }
        public string id { get; set; }
        public ReferenceModel model { get; set; }
    }

    public class ReferenceModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public ReferenceTable[] tables { get; set; }
        public ReferencePerspective[] perspectives { get; set; }
        public ReferenceRole[] roles { get; set; }
    }

    public class ReferenceTable
    {
        public string name { get; set; }
        public string description { get; set; }
        public ReferenceColumn[] columns { get; set; }
        public ReferenceMeasure[] measures { get; set; }
        public ReferenceHierarchy[] hierarchies { get; set; }
    }

    public class ReferenceColumn
    {
        public string name { get; set; }
        public string description { get; set; }
        public string displayFolder { get; set; }
    }

    public class ReferenceMeasure
    {
        public string name { get; set; }
        public string description { get; set; }
        public string displayFolder { get; set; }

        public ReferenceKpi kpi { get; set; }
    }

    public class ReferenceKpi
    {
        public string description { get; set; }
        public string displayFolder { get; set; }
        // public string translatedDescription { get; set; } wrong property??
    }

    public class ReferenceHierarchy
    {
        public string name { get; set; }
        public string description { get; set; }
        public string displayFolder { get; set; }
        public ReferenceLevel[] levels { get; set; }
    }

    public class ReferenceLevel
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ReferencePerspective
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ReferenceRole
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Culture
    {
        public string name { get; set; }
        public Translations translations { get; set; }
    }

    public class Translations
    {
        public Model model { get; set; }
    }

    public class Model
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
        public Table[] tables { get; set; }
        public Perspective[] perspectives { get; set; }
        public Role[] roles { get; set; }
    }

    public class Table
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
        public Column[] columns { get; set; }
        public Measure[] measures { get; set; }
        public Hierarchy[] hierarchies { get; set; }
    }

    public class Column
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
        public string translatedDisplayFolder { get; set; }
    }

    public class Measure
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
        public string translatedDisplayFolder { get; set; }
        public Kpi kpi { get; set; }
    }

    public class Kpi
    {
        public string translatedDescription { get; set; }
        public string translatedDisplayFolder { get; set; }
    }
    public class Hierarchy
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
        public string translatedDisplayFolder { get; set; }
        public Level[] levels { get; set; }
    }

    public class Level
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
    }

    public class Perspective
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
    }

    public class Role
    {
        public string name { get; set; }
        public string translatedCaption { get; set; }
        public string translatedDescription { get; set; }
    }


}