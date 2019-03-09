using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using kdgparking;
using System.Web;


namespace kdgParkingTests
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void DirtyInput_CleanOutput()
        {
            kdgparking.Controllers.CompanyController controller = new kdgparking.Controllers.CompanyController();
            string DirtyString = "Stéphane";
            string CleanString = "Stephane";
            string ResultString = controller.CleanString(DirtyString);
            Assert.AreEqual(CleanString, ResultString);
        }
    }
}
