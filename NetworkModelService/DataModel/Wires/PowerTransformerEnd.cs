using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PowerTransformerEnd : TransformerEnd
    {
        private float b;
        private float b0;
        private WindingConnection connectionKind;
        private float g;
        private float g0;
        private int phaseAngleClock;
        private float r;
        private float r0;
        private float ratedS;
        private float ratedU;
        private float x;
        private float x0;

        private long powerTransformer = 0;

        public PowerTransformerEnd(long globalId) : base(globalId)
        {
        }

        public float B { get => b; set => b = value; }
        public float B0 { get => b0; set => b0 = value; }
        public WindingConnection ConnectionKind { get => connectionKind; set => connectionKind = value; }
        public float G { get => g; set => g = value; }
        public float G0 { get => g0; set => g0 = value; }
        public int PhaseAngleClock { get => phaseAngleClock; set => phaseAngleClock = value; }
        public float R { get => r; set => r = value; }
        public float R0 { get => r0; set => r0 = value; }
        public float RatedS { get => ratedS; set => ratedS = value; }
        public float RatedU { get => ratedU; set => ratedU = value; }
        public float X { get => x; set => x = value; }
        public float X0 { get => x0; set => x0 = value; }
        public long PowerTransformer { get => powerTransformer; set => powerTransformer = value; }

        public override bool Equals(object obj) // ne zaboravi
        {
            if (base.Equals(obj))
            {
                PowerTransformerEnd x = (PowerTransformerEnd)obj;
                return (
                    this.b == x.b &&
                    this.b0 == x.b0 &&
                    this.connectionKind == x.connectionKind &&
                    this.g == x.g &&
                    this.g0 == x.g0 &&
                    this.phaseAngleClock == x.phaseAngleClock &&
                    this.r == x.r &&
                    this.r0 == x.r0 &&
                    this.ratedS == x.ratedS &&
                    this.ratedU == x.ratedU &&
                    this.x == x.x &&
                    this.x0 == x.x0 &&
                    this.powerTransformer == x.powerTransformer
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
                case ModelCode.POWERTREND_B:
                case ModelCode.POWERTREND_B0:
                case ModelCode.POWERTREND_CONNECTIONKIND:
                case ModelCode.POWERTREND_G:
                case ModelCode.POWERTREND_G0:
                case ModelCode.POWERTREND_PHASEANGLECLOCK:
                case ModelCode.POWERTREND_R:
                case ModelCode.POWERTREND_R0:
                case ModelCode.POWERTREND_RATEDS:
                case ModelCode.POWERTREND_RATEDU:
                case ModelCode.POWERTREND_X:
                case ModelCode.POWERTREND_X0:
                case ModelCode.POWERTREND_POWERTR:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.POWERTREND_B:
                    property.SetValue(b);
                    break;
                case ModelCode.POWERTREND_B0:
                    property.SetValue(b0);
                    break;
                case ModelCode.POWERTREND_CONNECTIONKIND:
                    property.SetValue((short)connectionKind);
                    break;
                case ModelCode.POWERTREND_G:
                    property.SetValue(g);
                    break;
                case ModelCode.POWERTREND_G0:
                    property.SetValue(g0);
                    break;
                case ModelCode.POWERTREND_PHASEANGLECLOCK:
                    property.SetValue(phaseAngleClock);
                    break;
                case ModelCode.POWERTREND_R:
                    property.SetValue(r);
                    break;
                case ModelCode.POWERTREND_R0:
                    property.SetValue(r0);
                    break;
                case ModelCode.POWERTREND_RATEDS:
                    property.SetValue(ratedS);
                    break;
                case ModelCode.POWERTREND_RATEDU:
                    property.SetValue(ratedU);
                    break;
                case ModelCode.POWERTREND_X:
                    property.SetValue(x);
                    break;
                case ModelCode.POWERTREND_X0:
                    property.SetValue(x0);
                    break;
                case ModelCode.POWERTREND_POWERTR:
                    property.SetValue(powerTransformer);
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
                case ModelCode.POWERTREND_B:
                    b = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_B0:
                    b0 = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_CONNECTIONKIND:
                    connectionKind = (WindingConnection)property.AsEnum();
                    break;
                case ModelCode.POWERTREND_G:
                    g = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_G0:
                    g0 = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_PHASEANGLECLOCK:
                    phaseAngleClock = property.AsInt();
                    break;
                case ModelCode.POWERTREND_R:
                    r = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_R0:
                    r0 = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_RATEDS:
                    ratedS = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_RATEDU:
                    ratedU = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_X:
                    x = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_X0:
                    x0 = property.AsFloat();
                    break;
                case ModelCode.POWERTREND_POWERTR:
                    powerTransformer = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (powerTransformer != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.POWERTREND_POWERTR] = new List<long> { powerTransformer };
            }

            base.GetReferences(references, refType);
        }


        #endregion
    }
}
