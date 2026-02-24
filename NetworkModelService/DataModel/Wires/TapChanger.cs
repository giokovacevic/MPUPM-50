using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class TapChanger : PowerSystemResource
    {
        private int highStep;
        private float initialDelay;
        private int lowStep;
        private bool ltcFlag;
        private int neutralStep;
        private float neutralU;
        private int normalStep;
        private bool regulationStatus;
        private float subsequentDelay;

        private long tapChangerCtrl = 0;

        public TapChanger(long globalId) : base(globalId)
        {
        }

        public int HighStep { get => highStep; set => highStep = value; }
        public float InitialDelay { get => initialDelay; set => initialDelay = value; }
        public int LowStep { get => lowStep; set => lowStep = value; }
        public bool LtcFlag { get => ltcFlag; set => ltcFlag = value; }
        public int NeutralStep { get => neutralStep; set => neutralStep = value; }
        public float NeutralU { get => neutralU; set => neutralU = value; }
        public int NormalStep { get => normalStep; set => normalStep = value; }
        public bool RegulationStatus { get => regulationStatus; set => regulationStatus = value; }
        public float SubsequentDelay { get => subsequentDelay; set => subsequentDelay = value; }
        public long TapChangerCtrl { get => tapChangerCtrl; set => tapChangerCtrl = value; }

        public override bool Equals(object obj) // ne zaboravi
        {
            if (base.Equals(obj))
            {
                TapChanger x = (TapChanger)obj;
                return (
                    this.highStep == x.highStep &&
                    this.initialDelay == x.initialDelay &&
                    this.lowStep == x.lowStep &&
                    this.ltcFlag == x.ltcFlag &&
                    this.neutralStep == x.neutralStep &&
                    this.neutralU == x.neutralU &&
                    this.normalStep == x.normalStep &&
                    this.regulationStatus == x.regulationStatus &&
                    this.subsequentDelay == x.subsequentDelay &&
                    this.tapChangerCtrl == x.tapChangerCtrl
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
                case ModelCode.TAPCHANGER_HIGHSTEP:
                case ModelCode.TAPCHANGER_INITIALDELAY:
                case ModelCode.TAPCHANGER_LOWSTEP:
                case ModelCode.TAPCHANGER_LTCFLAG:
                case ModelCode.TAPCHANGER_NEUTRALSTEP:
                case ModelCode.TAPCHANGER_NEUTRALU:
                case ModelCode.TAPCHANGER_NORMALSTEP:
                case ModelCode.TAPCHANGER_REGULATIONSTATUS:
                case ModelCode.TAPCHANGER_SUBSEQUENTDELAY:
                case ModelCode.TAPCHANGER_TAPCHANGERCTRL:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TAPCHANGER_HIGHSTEP:
                    property.SetValue(highStep);
                    break;
                case ModelCode.TAPCHANGER_INITIALDELAY:
                    property.SetValue(initialDelay);
                    break;
                case ModelCode.TAPCHANGER_LOWSTEP:
                    property.SetValue(lowStep);
                    break;
                case ModelCode.TAPCHANGER_LTCFLAG:
                    property.SetValue(ltcFlag);
                    break;
                case ModelCode.TAPCHANGER_NEUTRALSTEP:
                    property.SetValue(neutralStep);
                    break;
                case ModelCode.TAPCHANGER_NEUTRALU:
                    property.SetValue(neutralU);
                    break;
                case ModelCode.TAPCHANGER_NORMALSTEP:
                    property.SetValue(normalStep);
                    break;
                case ModelCode.TAPCHANGER_REGULATIONSTATUS:
                    property.SetValue(regulationStatus);
                    break;
                case ModelCode.TAPCHANGER_SUBSEQUENTDELAY:
                    property.SetValue(subsequentDelay);
                    break;
                case ModelCode.TAPCHANGER_TAPCHANGERCTRL:
                    property.SetValue(tapChangerCtrl);
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
                case ModelCode.TAPCHANGER_HIGHSTEP:
                    highStep = property.AsInt();
                    break;
                case ModelCode.TAPCHANGER_INITIALDELAY:
                    initialDelay = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGER_LOWSTEP:
                    lowStep = property.AsInt();
                    break;
                case ModelCode.TAPCHANGER_LTCFLAG:
                    ltcFlag = property.AsBool();
                    break;
                case ModelCode.TAPCHANGER_NEUTRALSTEP:
                    neutralStep = property.AsInt();
                    break;
                case ModelCode.TAPCHANGER_NEUTRALU:
                    neutralU = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGER_NORMALSTEP:
                    normalStep = property.AsInt();
                    break;
                case ModelCode.TAPCHANGER_REGULATIONSTATUS:
                    regulationStatus = property.AsBool();
                    break;
                case ModelCode.TAPCHANGER_SUBSEQUENTDELAY:
                    subsequentDelay = property.AsFloat();
                    break;
                case ModelCode.TAPCHANGER_TAPCHANGERCTRL:
                    tapChangerCtrl = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (tapChangerCtrl != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TAPCHANGER_TAPCHANGERCTRL] = new List<long> { tapChangerCtrl };
            }

            base.GetReferences(references, refType);
        }


        #endregion
    }
}
