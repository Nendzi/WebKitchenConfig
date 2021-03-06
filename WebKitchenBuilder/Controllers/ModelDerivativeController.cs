﻿/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Forge Partner Development
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

using Autodesk.Forge;
using Autodesk.Forge.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebKitchenBuilder.Controllers
{
    [ApiController]
    public class ModelDerivativeController : ControllerBase
    {
        /// <summary>
        /// Start the translation job for a give bucketKey/objectName
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/forge/modelderivative/jobs")]
        public async Task<dynamic> TranslateObject([FromBody] TranslateObjectModel objModel)
        {
            dynamic oauth = await OAuthController.GetInternalAsync();

            // prepare the payload
            List<JobPayloadItem> outputs = new List<JobPayloadItem>()
              {
               new JobPayloadItem(
                 JobPayloadItem.TypeEnum.Svf,
                 new List<JobPayloadItem.ViewsEnum>()
                 {
                   JobPayloadItem.ViewsEnum._2d,
                   JobPayloadItem.ViewsEnum._3d
                 })
              };

            JobPayload job;
            //job = new JobPayload(new JobPayloadInput(objModel.objectName), new JobPayloadOutput(outputs));
            job = new JobPayload(new JobPayloadInput(objModel.objectName, true, "Main element.iam"), new JobPayloadOutput(outputs));

            // start the translation
            DerivativesApi derivative = new DerivativesApi();
            derivative.Configuration.AccessToken = oauth.access_token;
            dynamic jobPosted = await derivative.TranslateAsync(job);
            return jobPosted;
        }

        /// <summary>
        /// Start the translation job for a give bucketKey/objectName with zip file
        /// </summary>
        /// <param name="zipObjModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/forge/modelderivative/v2/designdata/job")]
        public async Task<dynamic> TranslateZipObject([FromBody] TranslateZipObjectModel zipObjModel)
        {
            dynamic oauth = await OAuthController.GetInternalAsync();

            List<JobPayloadItem> outputs = new List<JobPayloadItem>()
            {
                new JobPayloadItem(JobPayloadItem.TypeEnum.Svf,
                new List<JobPayloadItem.ViewsEnum>()
                {
                    JobPayloadItem.ViewsEnum._2d,
                    JobPayloadItem.ViewsEnum._3d
                })
            };

            JobPayload job;
            job = new JobPayload(new JobPayloadInput(zipObjModel.objectName, true, "Main element.iam"), new JobPayloadOutput(outputs));

            DerivativesApi derivative = new DerivativesApi();
            derivative.Configuration.AccessToken = oauth.access_token;
            dynamic jobPosted = await derivative.TranslateAsync(job);
            return jobPosted;
        }

        /// <summary>
        /// Model for TranslateObject method
        /// </summary>
        public class TranslateObjectModel
        {
            public string bucketKey { get; set; }
            public string objectName { get; set; }
        }

        public class TranslateZipObjectModel
        {
            public string bucketKey { get; set; }
            public string objectName { get; set; }
            public string assemblyName { get; set; }
        }
    }


}
