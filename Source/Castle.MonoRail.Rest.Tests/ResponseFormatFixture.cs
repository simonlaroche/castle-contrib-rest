﻿using System.Linq;
using Castle.MonoRail.Rest.Mime;
using MbUnit.Framework;
using Rhino.Mocks;

namespace Castle.MonoRail.Rest.Tests
{
    [TestFixture]
    public class ResponseFormatFixture
    {
        private MockRepository mocks;
        private IControllerBridge bridge;
        private ResponseFormatInternal format;
        private string handlerInvoked;

        [Test]
        public void IfRespondWithMimeAll_ThenFirstResponder_ShouldBeInvoked()
        {
            mocks = new MockRepository();
            bridge = mocks.DynamicMock<IControllerBridge>();

            using (mocks.Record())
            {
                SetupResult.For(bridge.ControllerAction).Return("Show");
            }
            
			string handlerInvoked = "";
			MimeTypes mimeTypes = new MimeTypes();
			mimeTypes.RegisterBuiltinTypes();

            format = (ResponseFormatInternal)new ResponseFormat(mimeTypes);
            format.AddRenderer("html", response => handlerInvoked = "html");
            format.AddRenderer("xml", response => handlerInvoked = "xml");

            format.RespondWith("all", bridge);
            Assert.AreEqual("html", handlerInvoked);
        }
    }
}
