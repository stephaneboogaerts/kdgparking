﻿using kdgparking.BL.Domain;
using System;

namespace kdgparking.BL
{
    public interface IContractManager
    {
        //Contract
        Contract AddContract(Contract contract);
        //Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end);
        Contract AddContract(Holder holder, Badge badge, DateTime begin, DateTime end, string contractId = null);
        //Contract GetContract(string ContractId);
        Contract GetContract(string Id);
        Contract GetHolderContract(int HolderId);
        Contract ChangeContract(Contract contract);
        void DeleteContract(Contract contract);

        // Contract 
        void ArchiveContract(Contract contract);
        // Badge
        Badge AddBadge(string badgeId);
        Badge GetBadge(string badgeId);
        void ChangeBadgeStatusToActive(Badge badge);
        void ChangeBadgeStatusToLost(Badge badge);
        void ChangeBadgeStatusToDisabled(Badge badge);
        Holder HandleBadgeAssignment(int holderId, string badgeId, DateTime start, DateTime end, string contractId = null);
        Holder HandleBadgeAssignment(Holder holder, string badgeId, DateTime start, DateTime end, string contractId = null);
    }
}
