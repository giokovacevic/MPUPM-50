using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.ClientConnection
{
    public class GDAClient
    {
        private NetworkModelGDAProxy gdaQueryProxy;

        private ModelResourcesDesc modelResourceDesc;

        public GDAClient()
        {
            modelResourceDesc = new ModelResourcesDesc();
        }

        public NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                if (gdaQueryProxy != null)
                {
                    gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
                }

                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                gdaQueryProxy.Open();
                return gdaQueryProxy;
            }
        }

        public string GetValues(long globalId, List<ModelCode> props)
        {
            ResourceDescription rd = null;
            string ss = "";
            try
            {
                rd = GdaQueryProxy.GetValues(globalId, props);
                ss += String.Format("Item with gid: 0x{0:x16}:\n", globalId);

                foreach (Property p in rd.Properties)
                {
                    ss += String.Format("{0} = ", p.Id);
                    ss += FormatProperty(p);
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

            return ss;
        }

        public string GetExtentValues(DMSType type, List<ModelCode> props)
        {
            int iteratorId = 0;
            string ss = "";
            bool gidBool = true;
            ModelCode modelCode = modelResourceDesc.GetModelCodeFromType(type);

            try
            {
                List<ModelCode> properties = props;
                if (!props.Contains(ModelCode.IDOBJ_GID))
                {
                    properties.Add(ModelCode.IDOBJ_GID);
                    gidBool = false;
                }

                iteratorId = GdaQueryProxy.GetExtentValues(modelCode, properties);
                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                ss += String.Format("Items with ModelCode: {0}:\n", modelCode.ToString());

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(2, iteratorId);

                    foreach (var rd in rds)
                    {
                        ss += String.Format("\tItem with gid: 0x{0:x16}\n", rd.Properties.Find(r => r.Id == ModelCode.IDOBJ_GID).AsLong());

                        foreach (Property p in rd.Properties)
                        {
                            if (p.Id == ModelCode.IDOBJ_GID && !gidBool) continue;

                            ss += String.Format("\t\t{0} = ", p.Id);
                            ss += FormatProperty(p);
                        }
                    }
                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }
                GdaQueryProxy.IteratorClose(iteratorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetExtentValues: " + ex.Message);
            }

            return ss;
        }

        public string GetRelatedValues(long sourceGlobalId, Association association, List<ModelCode> props)
        {
            string ss = "";
            int numberOfResources = 2;
            bool gidBool = true;
            try
            {
                List<ModelCode> properties = props;
                if (props.Contains(ModelCode.IDOBJ_GID) == false)
                {
                    properties.Add(ModelCode.IDOBJ_GID);
                    gidBool = false;
                }

                int iteratorId = GdaQueryProxy.GetRelatedValues(sourceGlobalId, properties, association);
                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        ss += String.Format("Item with gid: 0x{0:x16}\n", rds[i].Properties.Find(r => r.Id == ModelCode.IDOBJ_GID).AsLong());

                        foreach (Property p in rds[i].Properties)
                        {
                            if (p.Id == ModelCode.IDOBJ_GID && gidBool == false)
                            {
                            }
                            else
                            {
                                ss += String.Format("\t{0} =", p.Id);
                                ss += FormatProperty(p);
                            }
                        }
                    }
                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }
                GdaQueryProxy.IteratorClose(iteratorId);
            }
            catch (Exception) { }

            return ss;
        }

        public List<long> GetAllGids()
        {
            List<ModelCode> properties = new List<ModelCode>();
            List<long> ids = new List<long>();

            int iteratorId = 0;
            int numberOfResources = 1000;
            DMSType currType = 0;
            properties.Add(ModelCode.IDOBJ_GID);
            try
            {
                foreach (DMSType type in Enum.GetValues(typeof(DMSType)))
                {
                    currType = type;

                    if (type != DMSType.MASK_TYPE)
                    {
                        iteratorId = GdaQueryProxy.GetExtentValues(modelResourceDesc.GetModelCodeFromType(type), properties);
                        int count = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                        while (count > 0)
                        {
                            List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                            for (int i = 0; i < rds.Count; i++)
                            {
                                ids.Add(rds[i].Id);
                            }

                            count = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                        }

                        bool ok = GdaQueryProxy.IteratorClose(iteratorId);

                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ids;
        }

        public List<ResourceDescription> GetExtentValuesObjects(DMSType type, List<ModelCode> properties)
        {
            List<ResourceDescription> result = new List<ResourceDescription>();

            try
            {
                ModelCode typeModelCode = modelResourceDesc.GetModelCodeFromType(type);
                int iteratorId = GdaQueryProxy.GetExtentValues(typeModelCode, properties);

                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(10, iteratorId);
                    if (rds != null)
                    {
                        result.AddRange(rds);
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }
                GdaQueryProxy.IteratorClose(iteratorId);
            }
            catch (Exception e)
            {
                Console.WriteLine("GetExtentValuesObjects failed: {0}", e.Message);
            }

            return result;
        }

        private string FormatProperty(Property p)
        {
            switch (p.Type)
            {
                case PropertyType.Float:
                    return String.Format(" {0}:\n", p.AsFloat());
                case PropertyType.Bool:
                case PropertyType.Int32:
                case PropertyType.Int64:
                case PropertyType.DateTime:
                    return (p.Id == ModelCode.IDOBJ_GID) ?
                           String.Format("0x{0:x16}\n", p.AsLong()) :
                           String.Format("{0}\n", p.AsLong());
                case PropertyType.Reference:
                    return String.Format("0x{0:x16}\n", p.AsReference());
                case PropertyType.String:
                    string val = (p.PropertyValue.StringValue == null) ? String.Empty : p.AsString();
                    return String.Format("{0}\n", val);
                case PropertyType.Enum:
                    return String.Format("{0}\n", p.AsEnum());
                case PropertyType.ReferenceVector:
                    if (p.AsLongs().Count > 0)
                    {
                        string s = "";
                        foreach (long l in p.AsLongs())
                            s += String.Format("0x{0:x16},\n", l);
                        return s;
                    }
                    return "EMPTY\n";
                default:
                    throw new Exception("Invalid property type.");
            }
        }
    }
}
