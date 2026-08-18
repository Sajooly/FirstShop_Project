using _0_Framework.Application;
using DiscountManagement.Application.Contract.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using System.Collections.Generic;

namespace DiscountManagement.Application
{
    public class ColleagueDiscountApplication : IColleagueDiscountApplication
    {
        private readonly IColleagueDiscountRepository _colleagueDiscountRepository;

        public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository)
        {
            _colleagueDiscountRepository = colleagueDiscountRepository;
        }

        public OperationResult Define(DefineColleagueDiscount command)
        {
            OperationResult operation = new OperationResult();

            if (_colleagueDiscountRepository.Exists(x => x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            _colleagueDiscountRepository.Create(new ColleagueDiscount(command.ProductId, command.DiscountRate));
            _colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditColleagueDiscount command)
        {
            OperationResult operation = new OperationResult();

            var editEntity = _colleagueDiscountRepository.GetBy(command.Id);

            if (editEntity == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);
            if (_colleagueDiscountRepository.Exists(x => x.DiscountRate == command.DiscountRate && x.ProductId == command.ProductId && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            editEntity.Edit(command.ProductId, command.DiscountRate);
            _colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditColleagueDiscount GetDetails(int id)
        {
            return _colleagueDiscountRepository.GetDetails(id);
        }

        public OperationResult Remove(int id)
        {
            OperationResult operation = new OperationResult();

            var entity = _colleagueDiscountRepository.GetBy(id);
            if (entity == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            entity.Remove();
            _colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Restore(int id)
        {
            OperationResult operation = new OperationResult();

            var entity = _colleagueDiscountRepository.GetBy(id);
            if (entity == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            entity.Restore();
            _colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
        {
            return _colleagueDiscountRepository.Search(searchModel);
        }
    }
}
