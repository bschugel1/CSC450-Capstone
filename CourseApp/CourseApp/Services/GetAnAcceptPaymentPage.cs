using System;
using System.Collections.Generic;
using System.Linq;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using CourseApp.Models;
using CourseApp.Configuration;
using Microsoft.AspNetCore.Mvc;
using CourseApp.Helpers;

namespace CourseApp.Services
{
    public class GetAnAcceptPaymentPage
    {
        public static ANetApiResponse Run(TransactionModel model, AuthorizeNetPaymentSettings settings, IUrlHelper _urlHelper)
        {
            Console.WriteLine("GetAnAcceptPaymentPage Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = settings.ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = settings.TransactionKey                
            };

            settingType[] options = new settingType[5];

            options[0] = new settingType();
            options[0].settingName = settingNameEnum.hostedPaymentButtonOptions.ToString();
            options[0].settingValue = "{\"text\": \"Pay\"}";

            options[1] = new settingType();
            options[1].settingName = settingNameEnum.hostedPaymentOrderOptions.ToString();
            options[1].settingValue = "{\"show\": false}";

            options[2] = new settingType();
            options[2].settingName = settingNameEnum.hostedPaymentShippingAddressOptions.ToString();
            options[2].settingValue = "{\"show\": false, \"required\": false}";

            options[3] = new settingType();
            options[3].settingName = settingNameEnum.hostedPaymentIFrameCommunicatorUrl.ToString();
            options[3].settingValue = $"{{\"url\":\"{_urlHelper.AbsoluteContent(settings.IframeURL)}\"  }}";

            options[4] = new settingType();
            options[4].settingName = settingNameEnum.hostedPaymentReturnOptions.ToString();
            options[4].settingValue = "{\"showReceipt\": false}";

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize capture only
                amount = model.Amount,
                refTransId = model.Id.ToString(),
                customer = new customerDataType
                {
                    id = model.Id.ToString()
                },               
                lineItems = new List<lineItemType>
                {
                      new lineItemType
                    {
                        name = new string(model.Course.Name.Take(31).ToArray()),
                        itemId = model.CourseId.ToString(),
                        unitPrice = model.Course.Price ?? 0m,
                        description = new string(model.Course.Name.Take(254).ToArray()),
                        quantity = 1m
                    }
                }.ToArray()
            };

            var request = new getHostedPaymentPageRequest();
            request.transactionRequest = transactionRequest;

            request.hostedPaymentSettings = options;

            // instantiate the controller that will call the service
            var controller = new getHostedPaymentPageController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("Message code : " + response.messages.message[0].code);
                Console.WriteLine("Message text : " + response.messages.message[0].text);
                Console.WriteLine("Token : " + response.token);
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                Console.WriteLine("Failed to get hosted payment page");
            }
            response.sessionToken = response.token;
            return response;
        }
    }
}