using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Terminal : IdentifiedObject
    {
        private bool connected;
        private PhaseCode phases;
        private int sequenceNumber;

        private List<long> transformerEnds = new List<long>();
        private List<long> regulatingCtrls = new List<long>();

        private long conductingEquipment = 0;

        public bool Connected { get => connected; set => connected = value; }
        public PhaseCode Phases { get => phases; set => phases = value; }
        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }
        public List<long> TransformerEnds { get => transformerEnds; set => transformerEnds = value; }
        public List<long> RegulatingCtrls { get => regulatingCtrls; set => regulatingCtrls = value; }
        public long ConductingEquipment { get => conductingEquipment; set => conductingEquipment = value; }

        public Terminal(long globalId) : base(globalId)
        {
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            switch(property)
            {
                case ModelCode.TERMINAL_CONNECTED:
                case ModelCode.TERMINAL_PHASES:
                case ModelCode.TERMINAL_SEQUENCENUMBER:
                case ModelCode.TERMINAL_TRANSFORMERENDS:
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                case ModelCode.TERMINAL_REGULATINGCTRLS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override bool Equals(object obj) // ne zaboravi
        {
            if (base.Equals(obj))
            {
                Terminal x = (Terminal)obj;
                return (this.connected == x.connected
                    && this.phases == x.phases
                    && this.sequenceNumber == x.sequenceNumber
                    && this.conductingEquipment == x.conductingEquipment
                    && CompareHelper.CompareLists(this.transformerEnds, x.transformerEnds)
                    && CompareHelper.CompareLists(this.regulatingCtrls, x.regulatingCtrls));
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

        public override void GetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.TERMINAL_CONNECTED:
                    property.SetValue(connected);
                    break;
                case ModelCode.TERMINAL_PHASES:
                    property.SetValue((short)phases);
                    break;
                case ModelCode.TERMINAL_SEQUENCENUMBER:
                    property.SetValue(sequenceNumber);
                    break;
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                    property.SetValue(conductingEquipment);
                    break;
                case ModelCode.TERMINAL_TRANSFORMERENDS:
                    property.SetValue(transformerEnds);
                    break;
                case ModelCode.TERMINAL_REGULATINGCTRLS:
                    property.SetValue(regulatingCtrls);
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
                case ModelCode.TERMINAL_CONNECTED:
                    connected = property.AsBool();
                    break;
                case ModelCode.TERMINAL_PHASES:
                    phases = (PhaseCode)property.AsEnum();
                    break;
                case ModelCode.TERMINAL_SEQUENCENUMBER:
                    sequenceNumber = property.AsInt();
                    break;
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                    conductingEquipment = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion

        #region IReference implementation
        public override bool IsReferenced
        {
            get
            {
                return transformerEnds.Count > 0 || regulatingCtrls.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if(conductingEquipment != 0 && (refType == TypeOfReference.Reference || refType  == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_CONDUCTINGEQUIPMENT] = new List<long> { conductingEquipment };
            }

            if(transformerEnds!= null && transformerEnds.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both)) {
                references[ModelCode.TERMINAL_TRANSFORMERENDS] = new List<long>(transformerEnds);
            }

            if (regulatingCtrls != null && regulatingCtrls.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_REGULATINGCTRLS] = new List<long>(regulatingCtrls);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch(referenceId)
            {
                case ModelCode.TRANSFORMEREND_TERMINAL:
                    transformerEnds.Add(globalId);
                    break;
                case ModelCode.REGULATINGCTRL_TERMINAL:
                    regulatingCtrls.Add(globalId);
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
                case ModelCode.TRANSFORMEREND_TERMINAL:
                    if(transformerEnds.Contains(globalId))
                    {
                        transformerEnds.Remove(globalId);
                    }
                    break;
                case ModelCode.REGULATINGCTRL_TERMINAL:
                    if (regulatingCtrls.Contains(globalId))
                    {
                        regulatingCtrls.Remove(globalId);
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
