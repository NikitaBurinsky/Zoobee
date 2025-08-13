using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ZooStorages.Core
{
    public record OperationResult
    {
		public bool Succeeded;
		public string? Message;
		public HttpStatusCode? ErrCode;
		public bool Failed { 
			get {  return !Succeeded; }
		}
		public OperationResult(bool succeeded, HttpStatusCode? errCode_ = null, string? errorText = null)
		{
			Succeeded = succeeded;
			Message = errorText;
			ErrCode = errCode_;
		}	
		public static OperationResult Success() => new OperationResult(true);
		public static OperationResult SuccessMessage(string message) => new OperationResult(true, null, message);
		public static OperationResult Error(string errString, HttpStatusCode? statusCode)
			=> new OperationResult(false, statusCode, errString);
		public ObjectResult ToProblemDetails()
		{
			var problemDetails = new ProblemDetails
			{
				Title = "Operation Failure",
				Detail = Message,
				Status = (int)ErrCode,
			};
			return new ObjectResult(problemDetails) { StatusCode = (int)ErrCode };
		}
	}
	public record OperationResult<TData> : OperationResult
	{
		public TData? Returns { get; }

		private OperationResult(bool succeeded, HttpStatusCode? statusCode = null, string? errorText = null, TData? data = default)
			: base(succeeded,statusCode, errorText)
		{
			Returns = data;
		}

		public static OperationResult<TData> Success(TData data) => new(true, null, null, data);
		public static new OperationResult<TData> Error(string errorText, HttpStatusCode? statusCode) 
			=> new(false, statusCode, errorText);

	}



}
