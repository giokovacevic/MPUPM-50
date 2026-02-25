using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Helper
{
    public class PrintHelper
    {
        public string PrintResourceDescription(ResourceDescription rd)
        {
            if (rd == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\nID GID: '0x{0:x16}'", rd.Id));
            sb.AppendLine(string.Format("Type: {0}", ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)).ToString()));
            sb.AppendLine("Properties:");
            for (int i = 0; i < rd.Properties.Count; i++)
            {
                sb.Append("\n");
                sb.AppendLine("Property:");
                sb.AppendLine(string.Format("id: {0}", rd.Properties[i].Id.ToString()));
                sb.Append("value: ");
                switch (rd.Properties[i].Type)
                {
                    case PropertyType.Float:
                        sb.AppendLine(rd.Properties[i].AsFloat().ToString());
                        break;
                    case PropertyType.Bool:
                        sb.AppendLine(String.Format(rd.Properties[i].AsBool().ToString()));
                        break;
                    case PropertyType.Byte:
                    case PropertyType.Int32:
                    case PropertyType.Int64:
                    case PropertyType.TimeSpan:
                    case PropertyType.DateTime:
                        if (rd.Properties[i].Id == ModelCode.IDOBJ_GID)
                        {
                            sb.AppendLine(String.Format("0x{0:x16}", rd.Properties[i].AsLong()));
                        }
                        else
                        {
                            sb.AppendLine(rd.Properties[i].AsLong().ToString());
                        }

                        break;
                    case PropertyType.Enum:
                        try
                        {
                            EnumDescs enumDescs = new EnumDescs();
                            sb.AppendLine(enumDescs.GetStringFromEnum(rd.Properties[i].Id, rd.Properties[i].AsEnum()));
                        }
                        catch (Exception)
                        {
                            sb.AppendLine(rd.Properties[i].AsEnum().ToString());
                        }

                        break;
                    case PropertyType.Reference:
                        sb.AppendLine(String.Format("0x{0:x16}", rd.Properties[i].AsReference()));
                        break;
                    case PropertyType.String:
                        if (rd.Properties[i].PropertyValue.StringValue == null)
                        {
                            rd.Properties[i].PropertyValue.StringValue = String.Empty;
                        }
                        sb.AppendLine(rd.Properties[i].AsString());
                        break;

                    case PropertyType.Int64Vector:
                    case PropertyType.ReferenceVector:
                        var refList = rd.Properties[i].AsLongs();
                        if (refList.Count > 0)
                        {
                            for (int j = 0; j < refList.Count; j++)
                            {
                                sb.AppendLine(String.Format("0x{0:x16}", refList[j])).Append(", ");
                            }
                            sb = sb.Remove(sb.Length - 2, 2);
                        }
                        else
                        {
                            sb.AppendLine("empty long/reference vector");
                        }

                        break;
                    case PropertyType.TimeSpanVector:
                        if (rd.Properties[i].AsLongs().Count > 0)
                        {
                            for (int j = 0; j < rd.Properties[i].AsLongs().Count; j++)
                            {
                                sb.AppendLine(String.Format("0x{0:x16}", rd.Properties[i].AsTimeSpans()[j])).Append(", ");
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty long/reference vector");
                        }

                        break;
                    case PropertyType.Int32Vector:
                        if (rd.Properties[i].AsInts().Count > 0)
                        {
                            for (int j = 0; j < rd.Properties[i].AsInts().Count; j++)
                            {
                                sb.AppendLine(String.Format("{0}", rd.Properties[i].AsInts()[j])).Append(", ");
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty int vector");
                        }

                        break;

                    case PropertyType.DateTimeVector:
                        if (rd.Properties[i].AsDateTimes().Count > 0)
                        {
                            for (int j = 0; j < rd.Properties[i].AsDateTimes().Count; j++)
                            {
                                sb.AppendLine(String.Format("{0}", rd.Properties[i].AsDateTimes()[j])).Append(", ");
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty DateTime vector");
                        }

                        break;

                    case PropertyType.BoolVector:
                        if (rd.Properties[i].AsBools().Count > 0)
                        {
                            for (int j = 0; j < rd.Properties[i].AsBools().Count; j++)
                            {
                                sb.AppendLine(String.Format("{0}", rd.Properties[i].AsBools()[j])).Append(", ");
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty int vector");
                        }

                        break;
                    case PropertyType.FloatVector:
                        if (rd.Properties[i].AsFloats().Count > 0)
                        {
                            for (int j = 0; j < rd.Properties[i].AsFloats().Count; j++)
                            {
                                sb.AppendLine(rd.Properties[i].AsFloats()[j].ToString()).Append(", ");
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty float vector");
                        }

                        break;
                    case PropertyType.StringVector:
                        if (rd.Properties[i].AsStrings().Count > 0)
                        {
                            for (int j = 0; j < rd.Properties[i].AsStrings().Count; j++)
                            {
                                sb.AppendLine(rd.Properties[i].AsStrings()[j]).Append(", ");
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty string vector");
                        }

                        break;
                    case PropertyType.EnumVector:
                        if (rd.Properties[i].AsEnums().Count > 0)
                        {
                            EnumDescs enumDescs = new EnumDescs();

                            for (int j = 0; j < rd.Properties[i].AsEnums().Count; j++)
                            {
                                try
                                {
                                    sb.AppendLine(String.Format("{0}", enumDescs.GetStringFromEnum(rd.Properties[i].Id, rd.Properties[i].AsEnums()[j]))).Append(", ");
                                }
                                catch (Exception)
                                {
                                    sb.AppendLine(String.Format("{0}", rd.Properties[i].AsEnums()[j])).Append(", ");
                                }
                            }

                            sb.AppendLine(sb.ToString(0, sb.Length - 2));
                        }
                        else
                        {
                            sb.AppendLine("empty enum vector");
                        }

                        break;

                    default:
                        throw new Exception("Failed to export Resource Description as XML. Invalid property type.");
                }
            }
            sb.AppendLine("-------------------------------------------------------------------------------------------");
            return sb.ToString();
        }
    }
}
