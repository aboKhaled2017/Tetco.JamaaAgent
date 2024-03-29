﻿using Application.Agent.Queries.GetDynamicQueryData;

namespace Application.Common.Interfaces
{
    public interface IDynamicQuery
    {
        Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string query, IEnumerable<Paramter> paramters, int noOfQueries,string definedConnectionStr);
    }
}
