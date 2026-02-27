using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{

    public enum DMSType : short
    {
        MASK_TYPE = unchecked((short)0xFFFF),

        POWERTR                             = 0x0001, // DODATA BAZA
        POWERTREND                          = 0x0002, // dodato
        TERMINAL                            = 0x0003, // dodata baza
        TAPCHANGER                          = 0x0004, // dodata baza
        TAPCHANGERCTRL                      = 0x0005, // DODATO
    }

    [Flags]
    public enum ModelCode : long
    {
        // IDOBJ  gotovo
        IDOBJ                               = 0x1000000000000000,
        IDOBJ_GID                           = 0x1000000000000104,
        IDOBJ_MRID                          = 0x1000000000000207,
        IDOBJ_ALIASNAME                     = 0x1000000000000307,
        IDOBJ_NAME                          = 0x1000000000000407,

        // PSR  gotovo
        PSR                                 = 0x1100000000000000,

        // TERMINAL
        TERMINAL                            = 0x1200000000030000,
        TERMINAL_CONNECTED                  = 0x1200000000030101,
        TERMINAL_PHASES                     = 0x120000000003020a,
        TERMINAL_SEQUENCENUMBER             = 0x1200000000030303,
        TERMINAL_TRANSFORMERENDS            = 0x1200000000030419,
        TERMINAL_REGULATINGCTRLS            = 0x1200000000030519,
        TERMINAL_CONDUCTINGEQUIPMENT        = 0x1200000000030609,

        // TREND gotovo
        TRANSFORMEREND                      = 0x1300000000000000,
        TRANSFORMEREND_TERMINAL             = 0x1300000000000109,

        // TAPCHANGER
        TAPCHANGER                          = 0x1110000000040000,
        TAPCHANGER_HIGHSTEP                 = 0x1110000000040103,
        TAPCHANGER_INITIALDELAY             = 0x1110000000040205,
        TAPCHANGER_LOWSTEP                  = 0x1110000000040303,
        TAPCHANGER_LTCFLAG                  = 0x1110000000040401,
        TAPCHANGER_NEUTRALSTEP              = 0x1110000000040503,
        TAPCHANGER_NEUTRALU                 = 0x1110000000040605,
        TAPCHANGER_NORMALSTEP               = 0x1110000000040703,
        TAPCHANGER_REGULATIONSTATUS         = 0x1110000000040801,
        TAPCHANGER_SUBSEQUENTDELAY          = 0x1110000000040905,
        TAPCHANGER_TAPCHANGERCTRL           = 0x1110000000040a09,

        // REGULATINGCTRL
        REGULATINGCTRL                      = 0x1120000000000000,
        REGULATINGCTRL_DISCRETE             = 0x1120000000000101,
        REGULATINGCTRL_MODE                 = 0x112000000000020a,
        REGULATINGCTRL_MONITOREDPHASE       = 0x112000000000030a,
        REGULATINGCTRL_TARGETRANGE          = 0x1120000000000405,
        REGULATINGCTRL_TARGETVALUE          = 0x1120000000000505,
        REGULATINGCTRL_TERMINAL             = 0x1120000000000609,

        // EQUIPMENT
        EQUIPMENT                           = 0x1130000000000000,
        EQUIPMENT_AGGREGATE                 = 0x1130000000000101,
        EQUIPMENT_NORMALLYINSERVICE         = 0x1130000000000201,

        // TAPCHANGERCTRL
        TAPCHANGERCTRL                      = 0x1121000000050000,
        TAPCHANGERCTRL_LIMITVOLTAGE         = 0x1121000000050105,
        TAPCHANGERCTRL_LINEDROPCOMPENSATION = 0x1121000000050201,
        TAPCHANGERCTRL_LINEDROPR            = 0x1121000000050305,
        TAPCHANGERCTRL_LINEDROPX            = 0x1121000000050405,
        TAPCHANGERCTRL_REVERSELINEDROPR     = 0x1121000000050505,
        TAPCHANGERCTRL_REVERSELINEDROPX     = 0x1121000000050605,
        TAPCHANGERCTRL_TAPCHANGERS          = 0x1121000000050719,

        // CONDUCTINGEQUIPMENT // gotovo
        CONDUCTINGEQUIPMENT                 = 0x1131000000000000,
        CONDUCTINGEQUIPMENT_TERMINALS       = 0x1131000000000119,

        // POWERTR
        POWERTR                             = 0x1131100000010000,
        POWERTR_VECTORGROUP                 = 0x1131100000010107,
        POWERTR_POWERTRENDS                 = 0x1131100000010219,

        // POWERTREND 
        POWERTREND                          = 0x1310000000020000,
        POWERTREND_B                        = 0x1310000000020105,
        POWERTREND_B0                       = 0x1310000000020205,
        POWERTREND_CONNECTIONKIND           = 0x131000000002030a,
        POWERTREND_G                        = 0x1310000000020405,
        POWERTREND_G0                       = 0x1310000000020505,
        POWERTREND_PHASEANGLECLOCK          = 0x1310000000020603,
        POWERTREND_R                        = 0x1310000000020705,
        POWERTREND_R0                       = 0x1310000000020805,
        POWERTREND_RATEDS                   = 0x1310000000020905,
        POWERTREND_RATEDU                   = 0x1310000000020a05,
        POWERTREND_X                        = 0x1310000000020b05,
        POWERTREND_X0                       = 0x1310000000020c05,
        POWERTREND_POWERTR                  = 0x1310000000020d09,


    }

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			 = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	 = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		  = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	  = unchecked((long)0xfffffff000000000),		
	}																		
}


