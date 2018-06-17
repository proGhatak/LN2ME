using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic.ServiceContracts;

namespace LN2ME.Controllers.CRUD
{
    public class ResumeController : CommonBase.Base.BaseAPIController
    {
        private BusinessLogic.ServiceContracts.Interface.IResume resume = null;

        private BusinessLogic.ServiceContracts.Interface.IResume Resume
        {
            get
            {
                if (resume == null)
                {
                    resume = new BusinessLogic.ServiceContracts.CRUD.Resume();
                }

                return resume;
            }

        }


        //Public Methods to work on the resume actions
    }
}
