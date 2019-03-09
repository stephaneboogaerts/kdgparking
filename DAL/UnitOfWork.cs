using kdgparking.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.DAL
{
    public class UnitOfWork : IDisposable
    {
        private OurDbContext ctx = new OurDbContext();
        private ContractRepository contractRepo;
        private HolderRepository holderRepo;
        private CompanyRepository companyRepo;

        public ContractRepository ContractRepo
        {
            get
            {
                if (contractRepo == null)
                {
                    contractRepo = new ContractRepository(ctx);
                }
                return contractRepo;
            }
        }

        public HolderRepository HolderRepo
        {
            get
            {
                if (holderRepo == null)
                {
                    holderRepo = new HolderRepository(ctx);
                }
                return holderRepo;
            }
        }

        public CompanyRepository CompanyRepo
        {
            get
            {
                if (companyRepo == null)
                {
                    companyRepo = new CompanyRepository(ctx);
                }
                return companyRepo;
            }
        }

        public void Save()
        {
            ctx.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    ctx.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
