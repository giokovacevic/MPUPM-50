using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PowerTransformer : ConductingEquipment
    {
        private string vectorGroup = string.Empty;

        private List<long> powerTransformerEnds = new List<long>();

        public PowerTransformer(long globalId) : base(globalId)
        {

        }

        public string VectorGroup { get => vectorGroup; set => vectorGroup = value; }
        public List<long> PowerTransformerEnds { get => powerTransformerEnds; set => powerTransformerEnds = value; }

        public override bool Equals(object obj) 
        {
            if (base.Equals(obj))
            {
                PowerTransformer x = (PowerTransformer)obj;
                return (
                    this.vectorGroup.Equals(x.vectorGroup) &&
                    CompareHelper.CompareLists(this.powerTransformerEnds, x.powerTransformerEnds)
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
                case ModelCode.POWERTR_VECTORGROUP:
                case ModelCode.POWERTR_POWERTRENDS:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.POWERTR_VECTORGROUP:
                    property.SetValue(vectorGroup);
                    break;
                case ModelCode.POWERTR_POWERTRENDS:
                    property.SetValue(powerTransformerEnds);
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
                case ModelCode.POWERTR_VECTORGROUP:
                    vectorGroup = property.AsString();
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
                return powerTransformerEnds.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (powerTransformerEnds != null && powerTransformerEnds.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.POWERTR_POWERTRENDS] = new List<long>(powerTransformerEnds);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.POWERTREND_POWERTR:
                    powerTransformerEnds.Add(globalId);
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
                case ModelCode.POWERTREND_POWERTR:
                    if (powerTransformerEnds.Contains(globalId))
                    {
                        powerTransformerEnds.Remove(globalId);
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
