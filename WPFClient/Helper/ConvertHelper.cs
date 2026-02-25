using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Helper
{
    public class ConvertHelper
    {
        public string EncodeGidToString(long gid)
        {
            return string.Format("0x{0:x16}", gid);
        }

        public List<ModelCode> GetSelected(List<Tuple<string, bool>> items)
        {
            return (from x in items where x.Item2 select ParseModelCodeFromString(x.Item1)).ToList();
        }

        public DMSType ParseDMSTypeFromString(string dmsType)
        {
            return (DMSType)Enum.Parse(typeof(DMSType), dmsType);
        }

        public long ParseGIDFromString(string hexGid)
        {
            return Convert.ToInt64(Int64.Parse(hexGid.Remove(0, 2), System.Globalization.NumberStyles.HexNumber));
        }

        public ModelCode ParseModelCodeFromString(string modelCode)
        {
            return (ModelCode)Enum.Parse(typeof(ModelCode), modelCode);
        }
    }
}
