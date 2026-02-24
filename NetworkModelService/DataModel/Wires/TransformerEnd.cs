using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class TransformerEnd : IdentifiedObject
    {
        private long terminal = 0;

        public TransformerEnd(long globalId) : base(globalId)
        {
        }

        public long Terminal { get => terminal; set => terminal = value; }

        public override bool Equals(object obj) // ne zaboravi
        {
            if (base.Equals(obj))
            {
                TransformerEnd x = (TransformerEnd)obj;
                return (
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
                case ModelCode.TRANSFORMEREND_TERMINAL:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TRANSFORMEREND_TERMINAL:
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
                case ModelCode.TRANSFORMEREND_TERMINAL:
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
                references[ModelCode.TRANSFORMEREND_TERMINAL] = new List<long> { terminal };
            }

            base.GetReferences(references, refType);
        }


        #endregion
    }
}
