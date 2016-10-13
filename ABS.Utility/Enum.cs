using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Utility
{
    public class Enum
    {
    }

    public enum ItemType
    {
        FinishGood = 1,
        RawMaterial = 2,
        Yarn = 3,
        FixedAsset = 4,
        Chemical = 5
    }
    public enum workFlowTranEnum_IsDelete
    {
        False = 0,
        True = 1,
    }
    
    public enum workFlowTranEnum_IsApproved
    {
        False = 0,
        True = 1,
    }
    public enum workFlowTranEnum_IsUpdate
    {
        False = 0,
        True = 1,
    }
    public enum workFlowTranEnum_IsDeclained
    {
        False = 0,
        True = 1,
    }
    public enum workFlowTranEnum_MessageName
    {
        Created = 1,
        Appoved=2,
        Declined=3,
       
    }

    public enum WorkFlowTranProcess
    {
        insert = 0,
        Appoved = 1,
        Declined = 2,

    }


    public enum workFlowTranEnum_Status
    {
        DeActive = 0,
        Active = 1,
    }
    
    public enum ResponseMessage
    {
        Success = 1,
        Error = 0,
        Invalid = -1,
        Exception =-2,
    }
}
