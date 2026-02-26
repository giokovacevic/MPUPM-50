using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RegulatingControl : PowerSystemResource
    {
        private bool discrete;
        private RegulatingControlModeKind mode;
        private PhaseCode monitoredPhase;
        private float targetRange;
        private float targetValue;

        private long terminal = 0;

        public RegulatingControl(long globalId) : base(globalId)
        {

        }

        public bool Discrete { get => discrete; set => discrete = value; }
        public RegulatingControlModeKind Mode { get => mode; set => mode = value; }
        public PhaseCode MonitoredPhase { get => monitoredPhase; set => monitoredPhase = value; }
        public float TargetRange { get => targetRange; set => targetRange = value; }
        public float TargetValue { get => targetValue; set => targetValue = value; }
        public long Terminal { get => terminal; set => terminal = value; }

        public override bool Equals(object obj) // ne zaboravi
        {
            if (base.Equals(obj))
            {
                RegulatingControl x = (RegulatingControl)obj;
                return (
                    this.discrete == x.discrete &&
                    this.mode == x.mode &&
                    this.monitoredPhase == x.monitoredPhase &&
                    this.targetRange == x.targetRange &&
                    this.targetValue == x.targetValue &&
                    this.terminal == x.terminal
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
                case ModelCode.REGULATINGCTRL_DISCRETE:
                case ModelCode.REGULATINGCTRL_MODE:
                case ModelCode.REGULATINGCTRL_MONITOREDPHASE:
                case ModelCode.REGULATINGCTRL_TARGETRANGE:
                case ModelCode.REGULATINGCTRL_TARGETVALUE:
                case ModelCode.REGULATINGCTRL_TERMINAL:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATINGCTRL_DISCRETE:
                    property.SetValue(discrete);
                    break;
                case ModelCode.REGULATINGCTRL_MODE:
                    property.SetValue((short)mode);
                    break;
                case ModelCode.REGULATINGCTRL_MONITOREDPHASE:
                    property.SetValue((short)monitoredPhase);
                    break;
                case ModelCode.REGULATINGCTRL_TARGETRANGE:
                    property.SetValue(targetRange);
                    break;
                case ModelCode.REGULATINGCTRL_TARGETVALUE:
                    property.SetValue(targetValue);
                    break;
                case ModelCode.REGULATINGCTRL_TERMINAL:
                    property.SetValue(terminal);
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
                case ModelCode.REGULATINGCTRL_DISCRETE:
                    discrete = property.AsBool();
                    break;
                case ModelCode.REGULATINGCTRL_MODE:
                    mode = (RegulatingControlModeKind)property.AsEnum();
                    break;
                case ModelCode.REGULATINGCTRL_MONITOREDPHASE:
                    monitoredPhase = (PhaseCode)property.AsEnum();
                    break;
                case ModelCode.REGULATINGCTRL_TARGETRANGE:
                    targetRange = property.AsFloat();
                    break;
                case ModelCode.REGULATINGCTRL_TARGETVALUE:
                    targetValue = property.AsFloat();
                    break;
                case ModelCode.REGULATINGCTRL_TERMINAL:
                    terminal = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (terminal != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULATINGCTRL_TERMINAL] = new List<long> { terminal };
            }

            base.GetReferences(references, refType);
        }


        #endregion
    }
}
