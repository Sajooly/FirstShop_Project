using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class SlideApplication : ISlideApplication
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IFileUploader _fileUploader;

        public SlideApplication(ISlideRepository slideRepository, IFileUploader fileUploader)
        {
            _slideRepository = slideRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateSlide command)
        {
            var operation = new OperationResult();

            var fileName = _fileUploader.Upload(command.Picture, "slides");

            _slideRepository.Create(new Slide(fileName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.BtnText, command.Link));
            _slideRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditSlide command)
        {
            var operation = new OperationResult();
            var slide = _slideRepository.GetBy(command.Id);
            if (slide == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var fileName = _fileUploader.Upload(command.Picture, "slides");

            slide.Edit(fileName, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.BtnText, command.Link);
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
