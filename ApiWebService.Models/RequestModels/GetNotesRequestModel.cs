﻿namespace ApiWebService.Models.RequestModels
{
    public class GetNotesRequestModel
    {
        public Guid PersonId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}