namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
				}
			}
		}

		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
			}
		}

        public static void PopulateTerminalProperties(FTN.Terminal terminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((terminal != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(terminal, rd);

                if (terminal.ConnectedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTED, terminal.Connected));
                }
                if (terminal.PhasesHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TERMINAL_PHASES, (short)GetDMSPhaseCode(terminal.Phases)));
                }
                if (terminal.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TERMINAL_SEQUENCENUMBER, terminal.SequenceNumber));
                }

                // 1 cond equip
                if (terminal.ConductingEquipmentHasValue)
                {
                    long gid = importHelper.GetMappedGID(terminal.ConductingEquipment.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(terminal.GetType().ToString()).Append(" rdfID = \"").Append(terminal.ID);
                        report.Report.Append("\" - Failed to set reference to conductingEquipment: rdfID \"").Append(terminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONDUCTINGEQUIPMENT, gid));
                }

            }
        }

        public static void PopulateTransformerEndProperties(FTN.TransformerEnd transformerEnd, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((transformerEnd != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(transformerEnd, rd);

                // + terminal
                if (transformerEnd.TerminalHasValue)
                {
                    long gid = importHelper.GetMappedGID(transformerEnd.Terminal.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(transformerEnd.GetType().ToString()).Append(" rdfID = \"").Append(transformerEnd.ID);
                        report.Report.Append("\" - Failed to set reference to conductingEquipment: rdfID \"").Append(transformerEnd.Terminal.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TRANSFORMEREND_TERMINAL, gid));
                }
            }
        }

        public static void PopulateTapChanger(FTN.TapChanger tapChanger, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((tapChanger != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(tapChanger, rd);

                if (tapChanger.HighStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_HIGHSTEP, tapChanger.HighStep));
                }
                if (tapChanger.InitialDelayHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_INITIALDELAY, tapChanger.InitialDelay));
                }
                if (tapChanger.LowStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_LOWSTEP, tapChanger.LowStep));
                }
                if (tapChanger.LtcFlagHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_LTCFLAG, tapChanger.LtcFlag));
                }
                if (tapChanger.NeutralStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_NEUTRALSTEP, tapChanger.NeutralStep));
                }
                if (tapChanger.NeutralUHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_NEUTRALU, tapChanger.NeutralU));
                }
                if (tapChanger.NormalStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_NORMALSTEP, tapChanger.NormalStep));
                }
                if (tapChanger.RegulationStatusHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_REGULATIONSTATUS, tapChanger.RegulationStatus));
                }
                if (tapChanger.SubsequentDelayHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_SUBSEQUENTDELAY, tapChanger.SubsequentDelay));
                }

                if (tapChanger.TapChangerControlHasValue)
                {
                    long gid = importHelper.GetMappedGID(tapChanger.TapChangerControl.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(tapChanger.GetType().ToString()).Append(" rdfID = \"").Append(tapChanger.ID);
                        report.Report.Append("\" - Failed to set reference to TapChangerControl: rdfID \"").Append(tapChanger.TapChangerControl.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_TAPCHANGERCTRL, gid));
                }

            }
        }

        public static void PopulateRegulatingControlProperties(FTN.RegulatingControl regulatingControl, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((regulatingControl != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(regulatingControl, rd);

                if (regulatingControl.DiscreteHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCTRL_DISCRETE, regulatingControl.Discrete));
                }
                if (regulatingControl.ModeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCTRL_MODE, (short)GetDMSRegulatingControlModeKind(regulatingControl.Mode)));
                }
                if (regulatingControl.MonitoredPhaseHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCTRL_MONITOREDPHASE, (short)GetDMSPhaseCode(regulatingControl.MonitoredPhase)));
                }
                if (regulatingControl.TargetRangeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCTRL_TARGETRANGE, regulatingControl.TargetRange));
                }
                if (regulatingControl.TargetValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGULATINGCTRL_TARGETVALUE, regulatingControl.TargetValue));
                }

                // + 1 terminal
                if (regulatingControl.TerminalHasValue)
                {
                    long gid = importHelper.GetMappedGID(regulatingControl.Terminal.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(regulatingControl.GetType().ToString()).Append(" rdfID = \"").Append(regulatingControl.ID);
                        report.Report.Append("\" - Failed to set reference to Terminal: rdfID \"").Append(regulatingControl.Terminal.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGULATINGCTRL_TERMINAL, gid));
                }
            }
        }

        public static void PopulateEquipmentProperties(FTN.Equipment equipment, ResourceDescription rd)
        {
            if ((equipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePowerSystemResourceProperties(equipment, rd);
                if (equipment.AggregateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, equipment.Aggregate));
                }
                if (equipment.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALLYINSERVICE, equipment.NormallyInService));
                }
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment conductingEquipment, ResourceDescription rd)
        {
            if ((conductingEquipment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateEquipmentProperties(conductingEquipment, rd);

				// + more terminals
            }
        }

        public static void PopulatePowerTransformerProperties(FTN.PowerTransformer powerTransformer, ResourceDescription rd)
        {
            if ((powerTransformer != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateConductingEquipmentProperties(powerTransformer, rd);

                if (powerTransformer.VectorGroupHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTR_VECTORGROUP, powerTransformer.VectorGroup));
                }

                // + more power transformer ends
            }
        }

        public static void PopulatePowerTransformerEndProperties(FTN.PowerTransformerEnd powerTransformerEnd, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((powerTransformerEnd != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateTransformerEndProperties(powerTransformerEnd, rd, importHelper, report);

                if (powerTransformerEnd.BHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_B, powerTransformerEnd.B));
                }
                if (powerTransformerEnd.B0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_B0, powerTransformerEnd.B0));
                }
                if (powerTransformerEnd.ConnectionKindHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_CONNECTIONKIND, (short)GetDMSWindingConnection(powerTransformerEnd.ConnectionKind)));
                }
                if (powerTransformerEnd.GHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_G, powerTransformerEnd.G));
                }
                if (powerTransformerEnd.G0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_G0, powerTransformerEnd.G0));
                }
                if (powerTransformerEnd.PhaseAngleClockHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_PHASEANGLECLOCK, powerTransformerEnd.PhaseAngleClock));
                }
                if (powerTransformerEnd.RHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_R, powerTransformerEnd.R));
                }
                if (powerTransformerEnd.R0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_R0, powerTransformerEnd.R0));
                }
                if (powerTransformerEnd.RatedSHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_RATEDS, powerTransformerEnd.RatedS));
                }
                if (powerTransformerEnd.RatedUHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_RATEDU, powerTransformerEnd.RatedU));
                }
                if (powerTransformerEnd.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_X, powerTransformerEnd.X));
                }
                if (powerTransformerEnd.X0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.POWERTREND_X0, powerTransformerEnd.X0));
                }

                if (powerTransformerEnd.PowerTransformerHasValue)
                {
                    long gid = importHelper.GetMappedGID(powerTransformerEnd.PowerTransformer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(powerTransformerEnd.GetType().ToString()).Append(" rdfID = \"").Append(powerTransformerEnd.ID);
                        report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(powerTransformerEnd.PowerTransformer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.POWERTREND_POWERTR, gid));
                }

                // + 1 power transformer
            }
        }

        public static void PopulateTapChangerControlProperties(FTN.TapChangerControl tapChangerControl, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((tapChangerControl != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateRegulatingControlProperties(tapChangerControl, rd, importHelper, report);
                if (tapChangerControl.LimitVoltageHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGERCTRL_LIMITVOLTAGE, tapChangerControl.LimitVoltage));
                }
                if (tapChangerControl.LineDropCompensationHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGERCTRL_LINEDROPCOMPENSATION, tapChangerControl.LineDropCompensation));
                }
                if (tapChangerControl.LineDropRHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGERCTRL_LINEDROPR, tapChangerControl.LineDropR));
                }
                if (tapChangerControl.LineDropXHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGERCTRL_LINEDROPX, tapChangerControl.LineDropX));
                }
                if (tapChangerControl.ReverseLineDropRHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGERCTRL_REVERSELINEDROPR, tapChangerControl.ReverseLineDropR));
                }
                if (tapChangerControl.ReverseLineDropXHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGERCTRL_REVERSELINEDROPX, tapChangerControl.ReverseLineDropX));
                }


                // + more tap changers
            }
        }


		#endregion Populate ResourceDescription

		#region Enums convert
		public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
		{
			switch (phases)
			{
				case FTN.PhaseCode.A:
					return PhaseCode.A;
				case FTN.PhaseCode.AB:
					return PhaseCode.AB;
				case FTN.PhaseCode.ABC:
					return PhaseCode.ABC;
				case FTN.PhaseCode.ABCN:
					return PhaseCode.ABCN;
				case FTN.PhaseCode.ABN:
					return PhaseCode.ABN;
				case FTN.PhaseCode.AC:
					return PhaseCode.AC;
				case FTN.PhaseCode.ACN:
					return PhaseCode.ACN;
				case FTN.PhaseCode.AN:
					return PhaseCode.AN;
				case FTN.PhaseCode.B:
					return PhaseCode.B;
				case FTN.PhaseCode.BC:
					return PhaseCode.BC;
				case FTN.PhaseCode.BCN:
					return PhaseCode.BCN;
				case FTN.PhaseCode.BN:
					return PhaseCode.BN;
				case FTN.PhaseCode.C:
					return PhaseCode.C;
				case FTN.PhaseCode.CN:
					return PhaseCode.CN;
				case FTN.PhaseCode.N:
					return PhaseCode.N;
				case FTN.PhaseCode.s12N:
					return PhaseCode.ABN;
				case FTN.PhaseCode.s1N:
					return PhaseCode.AN;
				case FTN.PhaseCode.s2N:
					return PhaseCode.BN;
				default: return PhaseCode.Unknown;
			}
		}

		public static WindingConnection GetDMSWindingConnection(FTN.WindingConnection windingConnection)
		{
			switch (windingConnection)
			{
				case FTN.WindingConnection.D:
					return WindingConnection.D;
				case FTN.WindingConnection.I:
					return WindingConnection.I;
				case FTN.WindingConnection.Z:
					return WindingConnection.Z;
				case FTN.WindingConnection.Y:
					return WindingConnection.Y;
				default:
					return WindingConnection.Y;
			}
		}

        public static RegulatingControlModeKind GetDMSRegulatingControlModeKind(FTN.RegulatingControlModeKind regulatingControlModeKind)
        {
            switch (regulatingControlModeKind)
            {
                case FTN.RegulatingControlModeKind.activePower:
                    return RegulatingControlModeKind.ActivePower;
                case FTN.RegulatingControlModeKind.admittance:
                    return RegulatingControlModeKind.Admittance;
                case FTN.RegulatingControlModeKind.currentFlow:
                    return RegulatingControlModeKind.CurrentFlow;
                case FTN.RegulatingControlModeKind.@fixed:
                    return RegulatingControlModeKind.Fixed;
                case FTN.RegulatingControlModeKind.powerFactor:
                    return RegulatingControlModeKind.PowerFactor;
                case FTN.RegulatingControlModeKind.reactivePower:
                    return RegulatingControlModeKind.ReactivePower;
                case FTN.RegulatingControlModeKind.temperature:
                    return RegulatingControlModeKind.Temperature;
                case FTN.RegulatingControlModeKind.timeScheduled:
                    return RegulatingControlModeKind.TimeScheduled;
                case FTN.RegulatingControlModeKind.voltage:
                    return RegulatingControlModeKind.Voltage;
                default:
                    return RegulatingControlModeKind.Unknown;
            }
        }
        #endregion Enums convert
    }

    /*
	  Unknown = 0x00,
        ActivePower = 0x01,
        Admittance = 0x02,
        CurrentFlow = 0x03,
        Fixed = 0x04,
        PowerFactor = 0x05,
        ReactivePower = 0x06,
        Temperature = 0x07,
        TimeScheduled = 0x08,
        Voltage = 0x09
	 */
}
