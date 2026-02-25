using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClient.ClientCon;

namespace WPFClient.Helper
{
    internal class ParseHelper
    {
        public GDAClient client;
        public ConvertHelper convertHelper;
        public PrintHelper printHelper;

        public ParseHelper()
        {
            this.client = new GDAClient();
            this.convertHelper = new ConvertHelper();
            this.printHelper = new PrintHelper();
        }

        public string EncodeGidToString(long gid)
        {
            return convertHelper.EncodeGidToString(gid);
        }

        public List<string> GetAllGIDs()
        {
            List<string> ret = new List<string>();
            List<long> gids = client.GetAllGIDs();

            if (gids != null)
            {
                ret = (from x in gids select convertHelper.EncodeGidToString(x)).ToList();
            }

            return ret;
        }

        public List<string> GetAllConcreteModelCodes()
        {
            List<string> modeli = new List<string>();

            var codes = client.GetAllConcreteModelCodes();
            if (codes != null)
            {
                modeli = (from x in codes select x.ToString()).ToList();
            }


            return modeli;
        }

        public List<string> GetAllProperties(long gid)
        {
            List<string> ret = new List<string>();
            var props = client.GetAllProperties(gid);

            if (props != null)
            {
                ret = (from x in props select x.ToString()).ToList();
            }

            return ret;
        }

        public List<string> GetAllProps(List<string> allCodes)
        {
            List<ModelCode> all = (from x in allCodes where x != "0 - No filter" select ParseModelCodeFromString(x)).ToList();
            List<string> ret = new List<string>();

            var response = client.GetProperties(all);
            if (response != null)
            {
                ret = (from x in response select x.ToString()).ToList();
            }

            return ret;
        }

        public List<string> GetDMSTypes()
        {
            List<string> retval = new List<string>()
            {
                "0 - No filter",
            };

            foreach (var x in Enum.GetValues(typeof(DMSType)))
            {
                if ((DMSType)x != DMSType.MASK_TYPE)
                    retval.Add(x.ToString());
            }

            return retval;
        }

        public string GetExtentValues(ModelCode model, List<ModelCode> props)
        {
            string ret = string.Empty;

            var rds = client.GetExtentValues(model, props);
            if (rds != null)
            {
                foreach (var rd in rds)
                {
                    ret += printHelper.PrintResourceDescription(rd);
                }
            }

            return ret;
        }

        public List<string> GetModelProperties(ModelCode code)
        {
            List<string> retval = new List<string>();
            var props = client.GetModelProperties(code);

            if (props != null)
            {
                retval = (from x in props select x.ToString()).ToList();
            }

            return retval;
        }

        public List<string> GetReferenceProps(long gid)
        {
            List<string> ret = new List<string>();
            var codes = client.GetReferenceProps(gid);

            if (codes != null)
            {
                ret = (from x in codes select x.ToString()).ToList();
            }

            return ret;
        }

        public string GetRelatedValues(long source, Association association, List<ModelCode> props)
        {
            string ret = string.Empty;
            var rds = client.GetRelatedValues(source, association, props);

            if (rds != null)
            {
                foreach (var rd in rds)
                {
                    ret += printHelper.PrintResourceDescription(rd);
                }
            }
            return ret;
        }

        public List<ModelCode> GetSelected(List<Tuple<string, bool>> items)
        {
            return convertHelper.GetSelected(items);
        }

        public string GetValues(long resourceId, List<ModelCode> propIds)
        {
            string ret = string.Empty;
            var rd = client.GetValues(resourceId, propIds);

            if (rd != null)
            {
                ret = printHelper.PrintResourceDescription(rd);
            }
            return ret;
        }

        public ModelCode ModelCodeFromDMSType(DMSType type)
        {
            return client.ModelCodeFromDMSType(type);
        }


        public DMSType ParseDMSTypeFromString(string dmsType)
        {
            return convertHelper.ParseDMSTypeFromString(dmsType);
        }

        public long ParseGIDFromString(string hexGid)
        {
            return convertHelper.ParseGIDFromString(hexGid);
        }

        public ModelCode ParseModelCodeFromString(string modelCode)
        {
            return convertHelper.ParseModelCodeFromString(modelCode);
        }
    }
}
}
