﻿using gp_backend.Core.Models;

namespace gp_backend.Api.Repositories.Interfaces
{
    public interface IFeedBackRepo : IGenericRepo<FeedBack>
    {
        Task<List<FeedBack>> GetAllAsync();
    }
}
