using ETravel.DAL;
using System.Threading.Tasks;

namespace ETravel.BAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<User> _UserRepository;
        private IRepository<Attachment> _AttachmentRepository;
        private IRepository<AttachmentSet> _AttachmentSetRepository;

        private ETravelEntities _dbContext;
        
        public UnitOfWork()
        {
            _dbContext = new ETravelEntities();
        }

        public ETravelEntities dbContext
        {
            get
            {
               if(this._dbContext == null)
                    _dbContext = new ETravelEntities();

                return _dbContext;
            }
            set
            {
                this._dbContext = value;
            }
        }

        public IRepository<Attachment> AttachmentRepository
        {
            get
            {
                if (_AttachmentRepository == null)
                    _AttachmentRepository = new Repository<Attachment>(_dbContext);
                  
                    return this._AttachmentRepository;
            }

            set
            {
                _AttachmentRepository = value;
            }
        }

        public IRepository<AttachmentSet> AttachmentSetRepository
        {
            get
            {
                if (_AttachmentSetRepository == null)
                    _AttachmentSetRepository = new Repository<AttachmentSet>(_dbContext);

                return _AttachmentSetRepository;
            }

            set
            {
                _AttachmentSetRepository = value;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (_UserRepository == null)
                    _UserRepository = new Repository<User>(_dbContext);

                return _UserRepository;
            }

            set
            {
                _UserRepository = value;
            }
        }

        public void Dispose()
        {

            if (_AttachmentRepository != null)
                _AttachmentRepository.Dispose();

            if (_AttachmentSetRepository != null)
                _AttachmentSetRepository.Dispose();

            if (_UserRepository != null)
                _UserRepository.Dispose();
            
            _dbContext.Dispose();
        }

        public void SaveChanges(bool save = true)
        {
            if (save)
                _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync(bool save = true)
        {

            if (save)
                await _dbContext.SaveChangesAsync();
        }
    }
}
