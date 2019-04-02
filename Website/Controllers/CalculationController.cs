using Website.Models;
using Shared.Entities;
using System;
using System.Web.Mvc;
using Website.Code;
using Business.Services;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;

namespace Website.Controllers
{
    [Authorize]
    public class CalculationController : BaseController
    {
        private const string previewFileName = "calculation_preview.png";

        /// <summary>
        /// Returns the default view containg the overview.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Returns the form for creating a new calculation.
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            return View();
        }


        /// <summary>
        /// Returns the details form for an existing calculation by its id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var calculation = new CalculationService().GetViewById(id);

            CalculationViewModel calculationModel = GetViewModelFromCalculation(calculation);

            return View(calculationModel);
        }


        /// <summary>
        /// Returns the view model for a domain entity.
        /// </summary>
        /// <param name="calculation">The calculation.</param>
        /// <returns></returns>
        private CalculationViewModel GetViewModelFromCalculation(Calculation calculation)
        {
            string stateName = null;

            if(calculation is CalculationView)
            {
                stateName = ((CalculationView)calculation).StateName;
            }

            var model = new CalculationViewModel();
            model.Id = calculation.Id;
            model.Width = calculation.Width;
            model.Height = calculation.Height;
            model.BorderX = calculation.BorderX;
            model.BorderY = calculation.BorderY;
            model.DrillingPointDistanceX = calculation.DrillingPointDistanceX;
            model.DrillingPointDistanceY = calculation.DrillingPointDistanceY;
            model.SealingSlabDiameter = calculation.SealingSlabDiameter;
            model.Depth = calculation.Depth;
            model.PixelsPerMeter = calculation.PixelsPerMeter;
            model.StandardDerivationOffset = calculation.StandardDerivationOffset;
            model.StandardDerivationRadius = calculation.StandardDerivationRadius;
            model.StandardDerivationDrillingPoint = calculation.StandardDerivationDrillingPoint;
            model.Iterations = calculation.Iterations;
            model.StateName = stateName;
            model.UnsetAreaResult = calculation.UnsetAreaResult;
            model.WaterLevelDifference = calculation.WaterLevelDifference;
            model.SealingSlabThickness = calculation.SealingSlabThickness;
            model.PermeabilityOfSoleWithoutUnsetArea = calculation.PermeabilityOfSoleWithoutUnsetArea;
            model.PermeabilityOfSoleAtUnsetArea = calculation.PermeabilityOfSoleAtUnsetArea;
            model.ResidualWaterResult = calculation.ResidualWaterResult;

            if(!calculation.IsTransient)
            {
                model.MaxIteration = new SealingSlabService().GetMaxIteration(calculation.Id);
            }

            return model;
        }


        /// <summary>
        /// Returns the domain entity for a view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private Calculation GetCalculationFromViewModel(CalculationViewModel model)
        {
            var calculation = new Calculation
            {
                Id = model.Id ?? 0,
                UserId = UserId,
                Width = model.Width,
                Height = model.Height,
                BorderX = model.BorderX,
                BorderY = model.BorderY,
                DrillingPointDistanceX = model.DrillingPointDistanceX,
                DrillingPointDistanceY = model.DrillingPointDistanceY,
                SealingSlabDiameter = model.SealingSlabDiameter,
                Depth = model.Depth,
                PixelsPerMeter = model.PixelsPerMeter,
                StandardDerivationOffset = model.StandardDerivationOffset,
                StandardDerivationRadius = model.StandardDerivationRadius,
                StandardDerivationDrillingPoint = model.StandardDerivationDrillingPoint,
                TimeStamp = DateTime.Now,
                Iterations = model.Iterations,
                WaterLevelDifference = model.WaterLevelDifference,
                SealingSlabThickness = model.SealingSlabThickness,
                PermeabilityOfSoleWithoutUnsetArea = model.PermeabilityOfSoleWithoutUnsetArea,
                PermeabilityOfSoleAtUnsetArea = model.PermeabilityOfSoleAtUnsetArea,
                ResidualWaterResult = model.ResidualWaterResult
            };

            return calculation;
        }


        /// <summary>
        /// Executes the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Execute(CalculationViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = ExecuteActionWithValidation(() =>
                {
                    var calculation = GetCalculationFromViewModel(model);

                    var calculationService = new CalculationService();
                    calculationService.Execute(calculation);

                    // Rebind View
                    model = GetViewModelFromCalculation(calculationService.GetViewById(calculation.Id));
                    
                    SetSuccessMessage("The calculation is enqueued.");
                });

                if (status)
                {
                    // Clear ModelState otherwise Id will stay 0
                    ModelState.Clear();
                    return PartialView("Form", model);
                }
            }

            return PartialView("Form");
        }


        /// <summary>
        /// Validates the base section of the form and returns a status message whether
        /// the preview picture can be created or not.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult BasePreview(CalculationViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = ExecuteActionWithValidation(() =>
                {
                    model.HasBasePreview = true;

                    SetSuccessMessage("Parameters are valid for preview.");
                });

                if (status)
                {
                    return PartialView("Form", model);
                }
            }

            return PartialView("Form");
        }


        /// <summary>
        /// Validates the derivation section of the form and returns a status message whether
        /// the preview picture can be created or not.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult DerivationPreview(CalculationViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = ExecuteActionWithValidation(() =>
                {
                    model.HasDerivationPreview = true;

                    SetSuccessMessage("Parameters are valid for preview.");
                });

                if (status)
                {
                    return PartialView("Form", model);
                }
            }

            return PartialView("Form");
        }


        /// <summary>
        /// Returns the base preview image file for a view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public FileContentResult BasePreviewImage(CalculationViewModel model)
        {
            var calculation = GetCalculationFromViewModel(model);
            var image = new CalculationService().GetBasePreviewImage(calculation);

            return File(GetBytesFromImage(image), MimeMapping.GetMimeMapping(previewFileName), previewFileName);
        }


        /// <summary>
        /// Returns the derivation preview image file for a view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public FileContentResult DerivationPreviewImage(CalculationViewModel model)
        {
            var calculation = GetCalculationFromViewModel(model);
            var image = new CalculationService().GetDerivationPreviewImage(calculation);

            return File(GetBytesFromImage(image), MimeMapping.GetMimeMapping(previewFileName), previewFileName);
        }


        /// <summary>
        /// Returns an image for a calculations iteration.
        /// </summary>
        /// <param name="calculationId">The calculation identifier.</param>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        public FileContentResult IterationImage(int calculationId, int iteration)
        {
            string fileName = $"calculation{calculationId}_iteration_{iteration}.png";

            var image = new CalculationService().GetIterationImage(calculationId, iteration);

            return File(GetBytesFromImage(image), MimeMapping.GetMimeMapping(fileName), fileName);
        }


        /// <summary>
        /// Returns a CSV-File for a calculation id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public FileContentResult Export(int id)
        {
            string fileName = $"Export {id}.csv";

            byte[] data = new CalculationService().GetCSVExport(id);

            return File(data, MimeMapping.GetMimeMapping(fileName), fileName);
        }


        /// <summary>
        /// Creates a copy of a calculation by its id and returns a new form.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Clone(int id)
        {
            if (ModelState.IsValid)
            {
                Calculation clone = null;

                bool status = ExecuteActionWithValidation(() =>
                {
                    clone = new CalculationService().Clone(id);

                    SetSuccessMessage("The calculation has been cloned.");
                });

                if (status)
                {
                    ModelState.Clear();
                    return View("New", GetViewModelFromCalculation(clone));
                }
            }

            var calculation = new CalculationService().GetViewById(id);

            return View("Details", GetViewModelFromCalculation(calculation));
        }


        /// <summary>
        /// Sets a caluclations state to deleting. The batchworker will then delete
        /// the calculation from the database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                bool status = ExecuteActionWithValidation(() =>
                {
                    new CalculationService().Delete(id);

                    SetSuccessMessage("The calculation will be deleted.");
                });

                if (status)
                {
                    ModelState.Clear();
                    return View("New");
                }
            }

            var calculation = new CalculationService().GetViewById(id);

            return View("Details", GetViewModelFromCalculation(calculation));
        }


        /// <summary>
        /// Allows deleting from an ajax request in the grid.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public JsonResult DeleteAsync(int id)
        {
            Delete(id);

            return Json(GetValidationMessage(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Sets the state of the calculation from the model to cancelling. The batchworker
        /// will then cancel the calculation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Cancel(CalculationViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = ExecuteActionWithValidation(() =>
                {
                    new CalculationService().Cancel(model.Id.Value);

                    SetSuccessMessage("The calculation will be cancelled.");
                });

                if (status)
                {
                    return PartialView("Form", model);
                }
            }

            return PartialView("Form");
        }


        /// <summary>
        /// Allows cancelling from an ajax request in the grid.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public JsonResult CancelAsync(int id)
        {
            Cancel(new CalculationViewModel() { Id = id });

            return Json(GetValidationMessage(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Returns the byte array of an image object. This can be used for writing files to the response.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        private byte[] GetBytesFromImage(Image image)
        {
            if(image == null)
            {
                return null;
            }

            byte[] bytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                bytes = ms.ToArray();
            }

            return bytes;
        }


        /// <summary>
        /// Returns the grid data which is sorted and paged in a json format.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public JsonResult GetGridData(JqGridSortAndPagingParameters parameters)
        {
            var data = new CalculationService().GetForGrid(parameters.MapSortAndPagingParameters());
            var ret = data.GetJsonDataForGrid(parameters);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}