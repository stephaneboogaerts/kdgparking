using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;
using OfficeOpenXml;
using System.IO;
using Syroot.Windows.IO;
using kdgparking.DAL.EF;

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
            // Validatie gebeurt in InputHolder
            return repo.CreateContract(contract);
        }

        public Contract GetContract(int Id)
        {
            return repo.ReadContract(Id);
        }

        //public Contract GetContract(string ContractId)
        //{
        //    return repo.ReadContract(ContractId);
        //}

        public Contract GetHolderContract(int HolderId)
        {
            return repo.ReadHolderContract(HolderId);
        }

        public Contract ChangeContract(Contract contract)
        {
            // Validatie gebeurt in InputHolder
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
                BadgeStatus = BadgeStatus.Active
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
            badge.BadgeStatus = BadgeStatus.Active;
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
            Badge badge = repo.ReadBadge(badgeId);
            if (badge == null)
            {
                badge = this.AddBadge(badgeId);
            }

            if (holder.Contracts == null || holder.Contracts.Count == 0)
            {
                holder.Contracts = new List<Contract>();
                Contract contract = AddContract(holder, badge, start, end, contractId);
                holder.Contracts.Add(contract);
            }
            else if (holder.Contracts.FirstOrDefault(h => h.Archived == false).Badge.BadgeId != badge.BadgeId)
            {
                // Wanneer een nieuwe badge zou worden toegewezen wordt de vorige gearchiveerd
                this.ArchiveContract(holder.Contracts.FirstOrDefault(h => h.Archived == false));
                Contract contract = AddContract(holder, badge, start, end, contractId);
                holder.Contracts.Add(contract);
            }
            return holder;
        }

        private Contract AddBadgeHistory(Holder holder, Badge badge, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}
