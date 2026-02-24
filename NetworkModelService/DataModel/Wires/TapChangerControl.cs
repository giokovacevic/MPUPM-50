using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class TapChangerControl : RegulatingControl
    {
        private float limitVoltage;
        private bool lineDropCompensation;
        private float lineDropR;
        private float lineDropX;
        private float reverseLineDropR;
        private float reverseLineDropX;

        private List<long> tapChangers = new List<long>();

        public TapChangerControl(long globalId) : base(globalId)
        {
        }

        public float LimitVoltage { get => limitVoltage; set => limitVoltage = value; }
        public bool LineDropCompensation { get => lineDropCompensation; set => lineDropCompensation = value; }
        public float LineDropR { get => lineDropR; set => lineDropR = value; }
        public float LineDropX { get => lineDropX; set => lineDropX = value; }
        public float ReverseLineDropR { get => reverseLineDropR; set => reverseLineDropR = value; }
        public float ReverseLineDropX { get => reverseLineDropX; set => reverseLineDropX = value; }
        public List<long> TapChangers { get => tapChangers; set => tapChangers = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                TapChangerControl x = (TapChangerControl)obj;
                return (
                    this.limitVoltage == x.limitVoltage &&
                    this.lineDropCompensation == x.lineDropCompensation &&
                    this.lineDropR == x.lineDropR &&
                    this.lineDropX == x.lineDropX &&
                    this.reverseLineDropR == x.reverseLineDropR &&
                    this.reverseLineDropX == x.reverseLineDropX &&
                    CompareHelper.CompareLists(this.tapChangers, x.tapChangers)
                    );
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.TAPCHANGERCTRL_LIMITVOLTAGE:
                case ModelCode.TAPCHANGERCTRL_LINEDROPCOMPENSATION:
                case ModelCode.TAPCHANGERCTRL_LINEDROPR:
                case ModelCode.TAPCHANGERCTRL_LINEDROPX:
                case ModelCode.TAPCHANGERCTRL_REVERSELINEDROPR:
                case ModelCode.TAPCHANGERCTRL_REVERSELINEDROPX:
                case ModelCode.TAPCHANGERCTRL_TAPCHANGERS:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TAPCHANGERCTRL_LIMITVOLTAGE:
                    property.SetValue(limitVoltage);
                    break;
                case ModelCode.TAPCHANGERCTRL_LINEDROPCOMPENSATION:
                    property.SetValue(lineDropCompensation);
                    break;
                case ModelCode.TAPCHANGERCTRL_LINEDROPR:
                    property.SetValue(lineDropR);
                    break;
                case ModelCode.TAPCHANGERCTRL_LINEDROPX:
                    property.SetValue(lineDropX);
                    break;
                case ModelCode.TAPCHANGERCTRL_REVERSELINEDROPR:
                    property.SetValue(reverseLineDropR);
                    break;
                case ModelCode.TAPCHANGERCTRL_REVERSELINEDROPX:
                    property.SetValue(reverseLineDropX);
                    break;
                case ModelCode.TAPCHANGERCTRL_TAPCHANGERS:
                    property.SetValue(tapChangers);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TAPCHANGERCTRL_LIMITVOLTAGE:
                    limitVoltage = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGERCTRL_LINEDROPCOMPENSATION:
                    lineDropCompensation = property.AsBool();
                    break;
                case ModelCode.TAPCHANGERCTRL_LINEDROPR:
                    lineDropR = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGERCTRL_LINEDROPX:
                    lineDropX = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGERCTRL_REVERSELINEDROPR:
                    reverseLineDropR = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGERCTRL_REVERSELINEDROPX:
                    reverseLineDropX = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #region IReference implementation
        public override bool IsReferenced
        {
            get
            {
                return tapChangers.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (tapChangers != null && tapChangers.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TAPCHANGERCTRL_TAPCHANGERS] = new List<long>(tapChangers);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TAPCHANGER_TAPCHANGERCTRL:
                    tapChangers.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TAPCHANGER_TAPCHANGERCTRL:
                    if (tapChangers.Contains(globalId))
                    {
                        tapChangers.Remove(globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion
    }
}
