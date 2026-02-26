using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class ConductingEquipment : Equipment
    {
        private List<long> terminals = new List<long>();

        public ConductingEquipment(long globalId) : base(globalId)
        {
        }

        public override bool Equals(object obj) // ne zaboravi
        {
            if (base.Equals(obj))
            {
                ConductingEquipment x = (ConductingEquipment)obj;
                return (
                    CompareHelper.CompareLists(this.terminals, x.terminals)
                    );
            }
            else
            {
                return false;
            }
        }

        public List<long> Terminals { get => terminals; set => terminals = value; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.CONDUCTINGEQUIPMENT_TERMINALS:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONDUCTINGEQUIPMENT_TERMINALS:
                    property.SetValue(terminals);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        #region IReference implementation
        public override bool IsReferenced
        {
            get
            {
                return terminals.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {

            if (terminals != null && terminals.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONDUCTINGEQUIPMENT_TERMINALS] = new List<long>(terminals);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                    terminals.Add(globalId);
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
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                    if (terminals.Contains(globalId))
                    {
                        terminals.Remove(globalId);
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
