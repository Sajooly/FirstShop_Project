using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class SlideApplication : ISlideApplication
    {
        private readonly ISlideRepository _slideRepository;

        public SlideApplication(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
        }

        public OperationResult Create(CreateSlide command)
        {
            var operation = new OperationResult();
            _slideRepository.Create(new Slide(command.Picture, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.BtnText, command.Link));
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditSlide command)
        {
            var operation = new OperationResult();
            var slide = _slideRepository.GetBy(command.Id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Edit(command.Picture, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.BtnText, command.Link);
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditSlide GetDetails(int id)
        {
            return _slideRepository.GetDetails(id);
        }

        public List<SlideViewModel> GetList()
        {
            return _slideRepository.GetList();
        }

        public OperationResult RemoveSlide(int id)
        {
            var operation = new OperationResult();

            var slide = _slideRepository.GetBy(id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Remove();
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult RestoreSlide(int id)
        {
            var operation = new OperationResult();

            var slide = _slideRepository.GetBy(id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            slide.Restore();
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }
    }
}
