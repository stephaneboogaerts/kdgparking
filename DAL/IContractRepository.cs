using kdgparking.BL.Domain;
using kdgparking.DAL.EF;
using System;

namespace kdgparking.DAL
{
    public interface IContractRepository : IDisposable
    {
        OurDbContext ctx { get; }
        Contract CreateContract(Contract contract);
        //Contract ReadContract(string contractId);
        Contract ReadContract(string Id);
        Contract ReadHolderContract(int holderId);
        Contract UpdateContract(Contract contract);
        void DeleteContract(Contract contract);

        //ContractHistory CreateContractHistory(ContractHistory contractHist);
        
        // Badge
        //BadgeHistory CreateBadgeHistory(BadgeHistory badgeHistory);
        //void UpdateBadgeHistory(BadgeHistory badgeHistory);
        Badge CreateBadge(Badge badge);
        Badge ReadBadge(string badgeId);
        void UpdateBadge(Badge badge);
    }
}
