using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using NUnit.Framework;
using Webtest.Controllers;

namespace Webtest
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Do_test()
        {
            LetterController contr = new LetterController(new Service());
            var res = (OkNegotiatedContentResult<string>)contr.DoLetterAsync("Кит  рЫб  ")
                .GetAwaiter().GetResult();
            Assert.AreEqual("кит рыб", res.Content);
        }
    }
}