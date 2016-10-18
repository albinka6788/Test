
using BHIC.DML.WC.DTO;

namespace BHIC.DML.WC.DataContract
{
    public interface IOraganisationDataProvider
    {
        bool AddOraganisationDetails(OraganisationDTO organisation);
    }
}
