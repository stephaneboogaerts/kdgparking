using kdgparking.BL.Domain;
using kdgparking.DAL.EF;
using System.Linq;


namespace kdgparking.DAL
{
    public class ContractRepository : IContractRepository
    {
        public OurDbContext ctx { get; }

        public ContractRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public ContractRepository(OurDbContext context)
        {
            ctx = context;
        }

        public Contract CreateContract(Contract contract)
        {
            ctx.Contracts.Add(contract);
            ctx.SaveChanges();

            return contract;
        }

        public Contract ReadContract(string contractId)
        {
            Contract contract = ctx.Contracts.FirstOrDefault(c => c.ContractId == contractId);
            return contract;
        }

        public Contract ReadContract(int Id)
        {
            Contract contract = ctx.Contracts.FirstOrDefault(c => c.Id == Id);
            return contract;
        }

        public Contract ReadHolderContract(int holderId)
        {
            Contract contract = ctx.Contracts.FirstOrDefault(c => c.Holder.Id == holderId);
            return contract;
        }

        public void DeleteContract(Contract contract)
        {
            ctx.Contracts.Remove(contract);
            ctx.SaveChanges();
        }

        public Contract UpdateContract(Contract contract)
        {
            ctx.Entry(contract).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();

            return contract;
        }

        public Badge CreateBadge(Badge badge)
        {
            ctx.Badges.Add(badge);
            ctx.SaveChanges();
            return badge;
        }

        public void UpdateBadge(Badge badge)
        {
            ctx.Entry(badge).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public Badge ReadBadge(string badgeId)
        {
            Badge badge = ctx.Badges.FirstOrDefault(b => b.MifareSerial == badgeId);
            return badge;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
