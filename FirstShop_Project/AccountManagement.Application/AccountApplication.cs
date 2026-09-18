using _0_Framework.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAuthHelper _authHelper;
        private readonly IRoleRepository _roleRepository;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _authHelper = authHelper;
            _roleRepository = roleRepository;
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

        public OperationResult RegisterAccount(RegisterAccount commnad)
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

            if (_accountRepository.Exists(a => (a.UserName == commnad.UserName || a.Mobile == commnad.Mobile) && a.Id != commnad.Id))
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

        public OperationResult Login(LoginAccount command)
        {
            var operation = new OperationResult();

            //این روش خودم بود ک شاید از نظر پرفورمنس بهتر باشه ولی از جهت های دیگه خوب نیس.برای فهمیدنش سرچ بزن متد لاگینی ک نوشتی رو ببین.
            //var password = _passwordHasher.Hash(command.Password);
            //if (!_accountRepository.Exists(x => x.UserName == command.UserName && x.Password == password))
            //    operation.Failed(ApplicationMessages.UserNotExist);








            //var account = _accountRepository.GetBy(command.UserName);
            var _account = _accountRepository.GetBy(x => x.UserName == command.UserName);

            if (_account == null)
                return operation.Failed(ApplicationMessages.UserNotExist);

            var result = _passwordHasher.Check(_account.Password, command.Password);
            if (!result.Verified)
                return operation.Failed(ApplicationMessages.UserNotExist);


            var permissions = _roleRepository.GetBy(_account.RoleId).Permissions.Select(x => x.Code).ToList();

            _authHelper.SignIn(new AuthViewModel(_account.Id, _account.RoleId, _account.FullName, _account.UserName, permissions));
            return operation.Succedded();
        }

        public void LogOut()
        {
            _authHelper.SignOut();
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }

        public List<AccountViewModel> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }
    }
}
