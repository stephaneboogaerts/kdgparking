using kdgparking.BL.Domain;
using kdgparking.DAL;
using kdgparking.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kdgparking.BL
{
    public class ContractManager : IContractManager
    {
        private readonly IContractRepository repo;

        public ContractManager()
        {
            repo = new ContractRepository();
        }

        public ContractManager(OurDbContext context)
        {
            repo = new ContractRepository(context);
        }

        public Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end)
        {
            //Elk contract heeft een holder nodig
            HolderManager holderMng = new HolderManager(repo.ctx);
            Holder holder = holderMng.GetHolder(holderId);
            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end
            };

            return this.AddContract(contract);
        }

        public Contract AddContract(Holder holder, DateTime begin, DateTime end)
        {
            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end
            };

            return this.AddContract(contract);
        }
        
        public Contract AddContract(Contract contract)
        {
            return repo.CreateContract(contract);
        }

        public Contract GetContract(int Id)
        {
            return repo.ReadContract(Id);
        }

        public Contract GetHolderContract(int HolderId)
        {
            return repo.ReadHolderContract(HolderId);
        }

        public Contract ChangeContract(Contract contract)
        {
            return repo.UpdateContract(contract);
        }

        public void DeleteContract(Contract contract)
        {
            repo.DeleteContract(contract);
        }
        
        public void ArchiveContract(Contract contract)
        {
            contract.EndDate = DateTime.Now;
            contract.Archived = true;
            repo.UpdateContract(contract);
        }

        public Badge AddBadge(int badgeId)
        {
            Badge badge = new Badge()
            {
                BadgeId = badgeId,
                BadgeStatus = BadgeStatus.Ok
            };
            return this.AddBadge(badge);
        }

        private Badge AddBadge(Badge badge)
        {
            return repo.CreateBadge(badge);
        }

        public Badge GetBadge(int badgeId)
        {
            return repo.ReadBadge(badgeId);
        }

        public void ChangeBadgeStatusToActive(Badge badge)
        {
            badge.BadgeStatus = BadgeStatus.Ok;
            this.ChangeBadge(badge);
        }

        public void ChangeBadgeStatusToLost(Badge badge)
        {
            badge.BadgeStatus = BadgeStatus.Lost;
            this.ChangeBadge(badge);
        }

        public void ChangeBadgeStatusToDisabled(Badge badge)
        {
            badge.BadgeStatus = BadgeStatus.Disabled;
            this.ChangeBadge(badge);
        }

        private void ChangeBadge(Badge badge)
        {
            repo.UpdateBadge(badge);
        }

        public Contract AddContract(Holder holder, Badge badge, DateTime begin, DateTime end, string contractId = null)
        {
            Contract contract = new Contract()
            {
                Holder = holder,
                Badge = badge,
                StartDate = begin,
                EndDate = end,
                ContractId = contractId,
                Archived = false
            };
            return contract;
        }

        public Holder HandleBadgeAssignment(int holderId, int badgeId, DateTime start, DateTime end, string contractId = null)
        {
            HolderManager holdMgr = new HolderManager(repo.ctx);
            Holder holder = holdMgr.GetHolderWithBadges(holderId);
            return HandleBadgeAssignment(holder, badgeId, start, end, contractId);
        }

        public Holder HandleBadgeAssignment(Holder holder, int badgeId, DateTime start, DateTime end, string contractId = null)
        {
            HolderManager holdMgr = new HolderManager(repo.ctx);

            Badge badge = repo.ReadBadge(badgeId);
            if (badge == null)
            {
                badge = this.AddBadge(badgeId);
            }

            // Wanneer 
            if (holder.Contracts == null)
            {
                holder.Contracts = new List<Contract>();
                //Contract contract = AddContract(holder, badge, start, end, contractId);
                //holder.Contracts.Add(contract);
            }
            // Er bestaan geen actieve contracten, we maken een nieuwe aan
            if (holder.Contracts.FirstOrDefault(h => h.Archived == false) == null || holder.Contracts.Count == 0)
            {
                Contract contract = AddContract(holder, badge, start, end, contractId);
                holder.Contracts.Add(contract);
                holdMgr.ChangeHolder(holder);
            }
            // Er bestaat wel een actief contract, we gaan aftoetsen of de badges overeenkomen
            // Zoniet wordt het vorige "contract" gearchiveerd en wordt er een nieuwe aangemaakt met de nieuwe badge
            else if (holder.Contracts.FirstOrDefault(h => h.Archived == false).Badge.BadgeId != badge.BadgeId)
            {
                // Wanneer een nieuwe badge zou worden toegewezen wordt de vorige gearchiveerd
                this.ArchiveContract(holder.Contracts.FirstOrDefault(h => h.Archived == false));
                Contract contract = AddContract(holder, badge, start, end, contractId);
                holder.Contracts.Add(contract);
                holdMgr.ChangeHolder(holder);
            }
            return holder;
        }

        private Contract AddBadgeHistory(Holder holder, Badge badge, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}
