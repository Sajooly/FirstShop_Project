using AccountManagement.Application.Contract.Account;
using System.Collections.Generic;
using _0_Framework.Application;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();

            var account = _accountRepository.GetBy(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);


            var password = _passwordHasher.Hash(command.Password);
            account.ChangePassword(password);

            _accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult CreateAccount(CreateAccount commnad)
        {
            var operation = new OperationResult();

            if (_accountRepository.Exists(x => x.UserName == commnad.UserName || x.Mobile == commnad.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var password = _passwordHasher.Hash(commnad.Password);
            var fileName = _fileUploader.Upload(commnad.ProfilePhoto, "profilePhotos");

            _accountRepository.Create(new Account(commnad.FullName, commnad.UserName, password, commnad.Mobile, commnad.RoleId, fileName));


            _accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult EditAccount(EditAccount commnad)
        {
            var operation = new OperationResult();

            var account = _accountRepository.GetBy(commnad.Id);

            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_accountRepository.Exists(a => a.UserName == commnad.UserName || a.Mobile == commnad.Mobile && a.Id != commnad.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var fileName = _fileUploader.Upload(commnad.ProfilePhoto, "profilePhotos");
            account.Edit(commnad.FullName, commnad.UserName, commnad.Mobile, commnad.RoleId, fileName);

            _accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditAccount GetDetailsBy(int id)
        {
            return _accountRepository.GetDetailsBy(id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}
